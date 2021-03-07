
using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.ExtensionMethods;

using FluentValidation;

namespace BR.MadenIlan.Clients.Shared.Validations.FluentValidation.Auth
{
    public class SignUpDtoValidator : AbstractValidator<SignUpDTO>
    {
        public SignUpDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş geçilemez")
                .EmailAddress().WithMessage("Email formatı hatalı");
            RuleFor(x => x.Password).Password();
        }
    }
}
