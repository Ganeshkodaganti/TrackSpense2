namespace TrackSpense.Client.Models
{
    public class CategoryModel
    {
        public string? CategoryId { get; set; } = "";
        public string CategoryName { get; set; }
        public virtual string? UserId { get; set; }
    }
}
