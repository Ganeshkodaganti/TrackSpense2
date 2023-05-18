using FluentValidation;
using TrackSpense.Client.Models;
using TrackSpense.Client.Pages;

namespace TrackSpense.Client.ValidationModels;
public class LoginValidation : AbstractValidator<LoginModel>
    {
        public LoginValidation()
        {
            RuleFor(_ => _.UserName).NotEmpty()
               .WithMessage("Invalid Username");

            RuleFor(_ => _.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MaximumLength(16).WithMessage("Invalid Password")
                    .Matches(@"[A-Z]+").WithMessage("Invalid Password")
                    .Matches(@"[a-z]+").WithMessage("Invalid Password")
                    .Matches(@"[0-9]+").WithMessage("Invalid Password")
                    .Matches(@"[\@\!\?\*\.]+").WithMessage("Invalid Password");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<LoginModel>.CreateWithOptions((LoginModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }

