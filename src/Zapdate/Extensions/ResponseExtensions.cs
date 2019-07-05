using Zapdate.Core.Dto;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using FluentValidation.Results;
using System.Linq;

namespace Zapdate.Extensions
{
    public static class ResponseExtensions
    {
        private static IImmutableDictionary<string, int> ErrorStatusCodes { get; } =
                new Dictionary<ErrorType, HttpStatusCode>
                {
                    {ErrorType.ValidationError, HttpStatusCode.BadRequest},
                    {ErrorType.Authentication, HttpStatusCode.Unauthorized},
                    {ErrorType.InvalidOperation, HttpStatusCode.BadRequest},
                    {ErrorType.NotFound, HttpStatusCode.NotFound},
                }.ToImmutableDictionary(x => x.Key.ToString(), x => (int)x.Value);

        public static ActionResult ToActionResult(this IBusinessErrors status)
        {
            if (!status.HasError)
                return new OkResult();

            return ToActionResult(status.Error!);
        }

        public static ActionResult ToActionResult(this Error error)
        {
            var httpCode = ErrorStatusCodes[error.Type];

            if (error.Fields?.Count > 0)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                error = new Error(error.Type, error.Message, error.Code, ConvertDictionaryKeysToCamelCase(error.Fields)); // clone
#pragma warning restore CS8604 // Possible null reference argument.
            }

            return new ObjectResult(error) { StatusCode = httpCode };
        }

        public static ActionResult ToActionResult(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
                return new OkResult();

            var error = new Error(ErrorType.ValidationError.ToString(), "Validation of the object model failed",
                (int)ErrorCode.FieldValidation, validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));

            return ToActionResult(error);
        }

        private static IReadOnlyDictionary<string, TValue> ConvertDictionaryKeysToCamelCase<TValue>(IReadOnlyDictionary<string, TValue> dictionary)
        {
            var newDic = new Dictionary<string, TValue>();
            foreach (var item in dictionary)
            {
                newDic.Add(item.Key.ToCamelCase(), item.Value);
            }

            return newDic;
        }
    }
}