namespace Retail.Models;

using Newtonsoft.Json;

public class Customer
{
    public int id { get; set; }

    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    public string zipcode { get; set; }
    public string city { get; set; }
    public string stateAbbr { get; set; }
    public string avatar { get; set; }
    public DateTime? birthday { get; set; }
    public DateTime? first_seen { get; set; }
    public DateTime? last_seen { get; set; }
    public bool has_ordered { get; set; }
    public DateTime? latest_purchase { get; set; }
    public bool has_newsletter { get; set; }

    [JsonProperty(PropertyName = "groups")]
    public List<string> groupList => string.IsNullOrWhiteSpace(groups) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(groups);

    [JsonIgnore]
    public string groups { get; set; }
    public int nb_commands { get; set; }
    public double total_spent { get; set; }
}
