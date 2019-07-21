using FluentValidation;
using Zapdate.Server.Extensions;
using Zapdate.Server.Models.Universal;

namespace Zapdate.Server.Models.Validation
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
