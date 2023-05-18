using TrackSpense.Client.Models;

namespace TrackSpense.Client.StateContainers;

public class TransactionStateContainer
{

    public List<TransactionModel> Value { get; set; } = null;
    public event Action OnStateChange;
    public void SetValue(List<TransactionModel> value)
    {
        Value = value;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnStateChange?.Invoke();

}
