using System.Security.Claims;
using System.Transactions;

namespace TrackSpense.Client.Shared;

public partial class NavMenu
{
    private string UserDisplayName(List<Claim> claims)
    {
        var UserName = claims.Where(_ => _.Type == "UserName").Select(_ => _.Value).FirstOrDefault();

        if (!string.IsNullOrEmpty(UserName))
        {
            return $"{UserName}";
        }
        var email = claims.Where(_ => _.Type == "Email").Select(_ => _.Value).FirstOrDefault();

        nav.NavigateTo("/login", true);
        return email;
    }

    private async void DeleteToken()
    {

        await localStorage.ClearAsync();
        transactionState.Value = null;
        categoryState.Value = null;
        sumState.Value = null;

        nav.NavigateTo("/", true);
    }

}
