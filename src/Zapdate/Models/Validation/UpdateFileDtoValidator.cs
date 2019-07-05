using FluentValidation;
using Zapdate.Extensions;
using Zapdate.Models.Universal;

namespace Zapdate.Models.Validation
{
    public class UpdateFileDtoValidator : AbstractValidator<UpdateFileDto>
    {
        public UpdateFileDtoValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.Hash).NotEmpty().IsSha256Hash();
        }
    }
}
