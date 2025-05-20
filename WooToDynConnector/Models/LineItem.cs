using Newtonsoft.Json;

namespace WooToDynConnector.Models
{
    public class LineItem
    {
        [JsonProperty("product_id")]
        public string? ItemNo { get; set; }
        [JsonProperty("quantity")]
        public int? Quantity { get; set; }
        [JsonProperty("price")]
        public float? Price { get; set; }

        public override string ToString()
        {
            return $"ItemNo: {ItemNo}, Quantity: {Quantity}, Price: {Price}";
        }
    }
}
