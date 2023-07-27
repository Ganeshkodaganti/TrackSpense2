using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;
using TrackSpense.Client.Models;
using TrackSpense.Client.ValidationModels;
namespace TrackSpense.Client.Pages;

public partial class AddTransaction
{


    List<CategoryModel> categories = new List<CategoryModel>();
    TransactionModel transactionModel = new TransactionModel();
    TransactionValidation transactionValidator = new TransactionValidation();

    MudForm? form;

    string APIErrorMessage = string.Empty;

    private void HandleCategoryChange(string value)
    {
        transactionModel.TransactionCategory = value;
    }

    private async Task AddTransactionAsync()
    {
        await form.Validate();
        if (form.IsValid)
        {
           
            //transactionModel.TransactionDate = DateTime.Now;
            string token = await localStorage.GetItemAsync<string>("jwt-access-token");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            string userId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Use the user ID as needed
                transactionModel.UserId = userId;
                Console.WriteLine(userId);
            }



            var jsonPayload = System.Text.Json.JsonSerializer.Serialize(transactionModel);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("/Transaction/AddTransaction", requestContent);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await response.Content
                .ReadFromJsonAsync<Dictionary<string, List<string>>>();
                if (errors.Count > 0)
                {
                    foreach (var item in errors)
                    {
                        foreach (var errorMessage in item.Value)
                        {
                            APIErrorMessage = $"{errorMessage} | ";
                        }
                    }
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TransactionModel transactions = JsonConvert.DeserializeObject<TransactionModel>(responseContent);
                List<TransactionModel> list = transactionState.Value;
                list.Insert(0,transactions);
                transactionState.SetValue(list);

                //emptying transaction values 
                transactionModel.TransactionName="";
                transactionModel.TransactionAmount =0;
                transactionModel.TransactionCategory = "";
                transactionModel.TransactionDescription = "";

                Snackbar.Add("Transaction added successfully", MudBlazor.Severity.Success);
                nav.NavigateTo("/");
            }
            else
            {
                APIErrorMessage = "Failed To Add Transaction Please Try After SomeTime";
            }
        }

    }
    protected override async Task OnInitializedAsync()
    {
        if (categoryState.Value == null)
        {
            string token = await localStorage.GetItemAsync<string>("jwt-access-token");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            string userId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

            string url = $"/Category/GetCategories?userId={userId}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();

                categories = JsonConvert.DeserializeObject<List<CategoryModel>>(responseContent);
                categoryState.SetValue(categories);

            }
        }
        else
        {
            categories=categoryState.Value;
        }
    }
}
