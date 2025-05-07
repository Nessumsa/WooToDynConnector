using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WooToDynConnector.Models
{
    public class WooCustomer
    {
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

        public override string ToString()
        {
            return $"WooCommerceId: {WooCommerceId}, Name: {Name}, Email: {Email}, PhoneNo:{PhoneNo}, Address: {Address}, City: {City}, Postcode: {Postcode}, Country: {Country} "+Environment.NewLine;
        }
    }
}
