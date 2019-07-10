using FluentValidation;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Models.Request;

namespace Zapdate.Models.Validation
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequestDto>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.RsaKeyStorage).IsInEnum();
            RuleFor(x => x.Name).MinimumLength(6).NotEmpty()
                .When(x => x.RsaKeyStorage == KeyStorage.ServerEncrypted);
        }
    }
}
