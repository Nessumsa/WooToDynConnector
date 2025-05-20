using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WooToDynConnector.Models
{
    public class WooCustomer
    {
        //This class contains multiple variables that can be fetched from WooCommerce and used for customer creation in Business Central.
        //Each variable has a JsonProperty which corresponds to the respective variable in the received Json Object.
        [JsonProperty("id")]
        public int? WooCommerceId { get; set; }
        [JsonProperty("first_name")]
        public string? Name { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("phone")]
        public string? PhoneNo { get; set; }
        [JsonProperty("address_1")]
        public string? Address { get; set; }
        [JsonProperty("city")]
        public string? City { get; set; }
        [JsonProperty("postcode")]
        public string? Postcode { get; set; }
        [JsonProperty("country")]
        public string? Country { get; set; }
    }
}
