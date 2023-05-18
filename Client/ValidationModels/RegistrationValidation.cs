using FluentValidation;
using System.Text.Json;
using System.Text.RegularExpressions;
using TrackSpense.Client.Models;

namespace TrackSpense.Client.ValidationModels;

public class RegistrationValidation :AbstractValidator<RegistrationModel>
{
    public RegistrationValidation() { }
    private readonly HttpClient _httpClient;
    public RegistrationValidation(HttpClient httpClient) {
        _httpClient = httpClient;

        RuleFor(_ => _.UserName).NotEmpty();
        RuleFor(_ => _.Email).EmailAddress().NotEmpty().EmailAddress();

        //Configured password validation rules.
        RuleFor(_ => _.Password).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");

        RuleFor(_ => _.ConfirmPassword).Equal(_ => _.Password).WithMessage("ConfirmPassword must equal Password");

    }   
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<RegistrationModel>.CreateWithOptions((RegistrationModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

}