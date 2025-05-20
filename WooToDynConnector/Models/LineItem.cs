using Newtonsoft.Json;

namespace WooToDynConnector.Models
{
    public class LineItem
    {
        //This class contains multiple variables that can be fetched from WooCommerce and used for Order Line creation in Business Central.
        //Each variable has a JsonProperty which corresponds to the respective variable in the received Json Object.
        [JsonProperty("product_id")]
        public string? ItemNo { get; set; }
        [JsonProperty("quantity")]
        public int? Quantity { get; set; }
        [JsonProperty("price")]
        public float? Price { get; set; }
    }
}
