
using BR.MadenIlan.Clients.Shared.DTOs.Auth;

using FluentValidation;

namespace BR.MadenIlan.Clients.Shared.Validations.FluentValidation.Auth
{
    public class SignInDtoValidator:AbstractValidator<SignInDTO>
    {
        public SignInDtoValidator()
        {
            RuleFor(x=>x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş geçilemez");
        }
    }
}
