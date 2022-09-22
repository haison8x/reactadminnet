namespace Retail.Models;

public class Invoice
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public int command_id { get; set; }
    public int customer_id { get; set; }
    public double total_ex_taxes { get; set; }
    public double delivery_fees { get; set; }
    public double tax_rate { get; set; }
    public double taxes { get; set; }
    public double total { get; set; }
}