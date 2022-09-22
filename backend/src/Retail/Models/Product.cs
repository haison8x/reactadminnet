namespace Retail.Models;

public class Product
{
    public int id { get; set; }
    public int category_id { get; set; }
    public string reference { get; set; }
    public double width { get; set; }
    public double height { get; set; }
    public double price { get; set; }
    public string thumbnail { get; set; }
    public string image { get; set; }
    public string description { get; set; }
    public int stock { get; set; }
    public int sales { get; set; }
}