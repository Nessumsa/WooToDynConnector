using Newtonsoft.Json;

namespace WooToDynConnector.Models
{
    public class WooOrder
    {
        //This class contains multiple variables that can be fetched from WooCommerce and used for order creation in Business Central.
        //Each variable has a JsonProperty which corresponds to the respective variable in the received Json Object.
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("customer_id")]
        public string? CustomerId { get; set; }
        [JsonProperty("date_created")]
        public DateTime? DateCreated { get; set; }
        //Line Items are each product from the order, which is contained in a List.
        [JsonProperty("line_items")]
        public List<LineItem>? LineItems { get; set; } = new List<LineItem>();
    }
}
