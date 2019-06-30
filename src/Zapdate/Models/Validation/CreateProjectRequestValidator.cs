using FluentValidation;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Models.Request;

namespace Zapdate.Models.Validation
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequestDto>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.ProjectName).NotEmpty();
            RuleFor(x => x.RsaKeyStorage).IsInEnum();
            RuleFor(x => x.ProjectName).MinimumLength(6).NotEmpty()
                .When(x => x.RsaKeyStorage == KeyStorage.ServerEncrypted);
        }
    }
}
