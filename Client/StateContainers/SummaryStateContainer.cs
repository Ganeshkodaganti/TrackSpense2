using TrackSpense.Client.Models;

namespace TrackSpense.Client.StateContainers
{
    public class SummaryStateContainer
    {
        public List<SummaryItem> Value { get; set; } = null;
        public event Action OnStateChange;
        public void SetValue(List<SummaryItem> value)
        {
            Value = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
