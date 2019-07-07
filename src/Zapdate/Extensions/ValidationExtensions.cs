using Zapdate.Core.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Zapdate.Core.Domain;

namespace Zapdate.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> IsSemanticVersion<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => SemVersion.TryParse(x, out _))
                .WithMessage("The version must be a semantic version by https://semver.org/");
        }

        public static IRuleBuilderOptions<T, string> IsHash<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => Hash.TryParse(x, out _)).WithMessage("The property must be a valid hash.");
        }

        public static IRuleBuilderOptions<T, string> IsSha256Hash<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => Hash.TryParse(x, out var hash) && hash.Value.IsSha256Size)
                .WithMessage("The property must be a SHA256 valid hash.");
        }

        public static IRuleBuilderOptions<T, string> IsValidCultureName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x =>
            {
                try
                {
                    CultureInfo.GetCultureInfo(x);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }).WithMessage("The property must be a valid culture name.");
        }

        public static IRuleBuilderOptions<T, IEnumerable<TItem>> IsUniqueList<T, TItem, TKey>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder,
            Func<TItem, TKey> getKey, string? message = null)
        {
            return ruleBuilder.Must(x => x == null || x.Select(x => getKey(x)).Distinct().Count() == x.Count())
                .WithMessage(message ?? "The items must have a unique key");
        }
    }
}
