namespace TestPdf.Model;

public class InvoiceModel
{
    public string InvoiceNumber { get; set; }
    public DateTime Date { get; set; }
    public string Customer { get; set; }
    public List<InvoiceItem> Items { get; set; } = new();
    public decimal Total => Items?.Sum(x => x.Total) ?? 0;
}
public class InvoiceItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price;
}


