namespace Point.Of.Sale.Shopping.Cart.Database.Model;

public class ShoppingCart
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int ItemCount { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
}
