using Newtonsoft.Json;

namespace WooToDynConnector.Models
{
    public class WooOrder
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("customer_id")]
        public string? CustomerId { get; set; }
        [JsonProperty("date_created")]
        public DateTime? DateCreated { get; set; }
        [JsonProperty("line_items")]
        public List<LineItem>? LineItems { get; set; } = new List<LineItem>();

        public override string ToString()
        {
            return $"ID: {Id}, CustomerID: {CustomerId}, Date: {DateCreated} + Line Items: {LineItems?[0].ToString()} | {LineItems?[1].ToString()}" + Environment.NewLine;
        }
    }
}
