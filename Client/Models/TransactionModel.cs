namespace TrackSpense.Client.Models
{
    public class TransactionModel
    {
        public string? TransactionId { get; set; } = "";

        public string TransactionName { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionCategory { get; set; }
        public DateTime? TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public virtual string? UserId { get; set; } = "";
    }
}
