
using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.ExtensionMethods;

using FluentValidation;

namespace BR.MadenIlan.Clients.Shared.Validations.FluentValidation.Auth
{
    public class SignInDtoValidation:AbstractValidator<SignInDTO>
    {
        public SignInDtoValidation()
        {
            RuleFor(x=>x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");
            RuleFor(x => x.Password).Password();
        }
    }
}
