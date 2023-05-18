using TrackSpense.Client.Models;

namespace TrackSpense.Client.StateContainers;

public class CategoryStateContainer
{
    public List<CategoryModel> Value { get; set; } = null;
    public event Action OnStateChange;
    public void SetValue(List<CategoryModel> value)
    {
        Value = value;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnStateChange?.Invoke();

}
