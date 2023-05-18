using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using TrackSpense.Client.Models;
using TrackSpense.Client.ValidationModels;
using static MudBlazor.CategoryTypes;

namespace TrackSpense.Client.Pages;

public partial class EditTransaction
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public TransactionModel transaction { get; set; } = new TransactionModel();

    List<CategoryModel> categories=new List<CategoryModel>();
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task UpdateTransaction(TransactionModel transaction)
    {
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(transaction);
        var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


        var response = await _httpClient.PutAsync("/Transaction/UpdateTransaction", requestContent);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errors = await response.Content
            .ReadFromJsonAsync<Dictionary<string, List<string>>>();

        }
        else if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            nav.NavigateTo("/", true);
            Snackbar.Add("Transaction updated succesfully", MudBlazor.Severity.Success);

        }


    }


    protected override async Task OnInitializedAsync()
    {
        string url = $"/Category/GetCategories?userId={transaction.UserId}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {

            var responseContent = await response.Content.ReadAsStringAsync();

            categories = JsonConvert.DeserializeObject<List<CategoryModel>>(responseContent);

        }

    }
}
