namespace RazorPageFormLayout;

public class ProductDetails
{
    public string ProductName { get; set; }
    public decimal SellPrice { get; set; }
    public int QuantityInStock { get; set; }

    public ProductDetails(string productName, decimal sellPrice, int qty)
    {
        ProductName = productName;
        SellPrice = sellPrice;
        QuantityInStock = qty;
    }
}
