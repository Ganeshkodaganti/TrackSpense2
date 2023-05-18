using FluentValidation;
using TrackSpense.Client.Models;

namespace TrackSpense.Client.ValidationModels;


public class CategoryValidation : AbstractValidator<CategoryModel>
{
    public CategoryValidation() { }
    private readonly HttpClient _httpClient;
    public CategoryValidation(HttpClient httpClient)
    {
        _httpClient = httpClient;

        RuleFor(_ => _.CategoryName).NotEmpty();
      

    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CategoryModel>.CreateWithOptions((CategoryModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

}
