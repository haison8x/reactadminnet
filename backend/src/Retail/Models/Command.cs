namespace Retail.Models;

public class Command
{
    public int id { get; set; }
    public string reference { get; set; }
    public DateTime date { get; set; }
    public int customer_id { get; set; }
    public List<Basket> basket { get; set; }
    public double total_ex_taxes { get; set; }
    public double delivery_fees { get; set; }
    public double tax_rate { get; set; }
    public double taxes { get; set; }
    public double total { get; set; }
    public string status { get; set; }
    public bool returned { get; set; }
}
