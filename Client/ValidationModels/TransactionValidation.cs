using FluentValidation;
using TrackSpense.Client.Models;

namespace TrackSpense.Client.ValidationModels;

public class TransactionValidation : AbstractValidator<TransactionModel>
{
    public TransactionValidation() { }
    private readonly HttpClient _httpClient;
    public TransactionValidation(HttpClient httpClient)
    {
        _httpClient = httpClient;

        RuleFor(_ => _.TransactionName).NotEmpty();
        RuleFor(_ => _.TransactionAmount).NotEmpty();
        RuleFor(_ => _.TransactionDescription).NotEmpty();
        RuleFor(_=>_.TransactionCategory).NotEmpty();

    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<TransactionModel>.CreateWithOptions((TransactionModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

}


