namespace TrackSpense.Shared.BusinessModels;

public class Business_Category
{
    public string CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public virtual string? UserId { get; set; }
}
