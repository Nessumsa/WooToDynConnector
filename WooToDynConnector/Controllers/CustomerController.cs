//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using System.Net.Http.Headers;
//using System.Text;
//using WooToDynConnector.Models;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using WooToDynConnector.Models;

namespace WooToDynConnector.Controllers
{
        [ApiController]
        [Route("api/customers")]
        public class CustomerController : ControllerBase
        {
            //This Task is creating an endpoint for receiving the WooCommerce customers. This endpoint is used in the webhook creation at WooCommerce.
            //It receives the data in Json format from WooCommerce and the calls the Task to post it to Business Central.
            [HttpPost("receive")]
            public async Task<IActionResult> ReceiveWooCustomer([FromBody] JObject orderJson)
            {
                var wooCustomer = orderJson.ToObject<WooCustomer>();

            var success = await PostCustomerToBusinessCentral(wooCustomer);
            if (success)
                return Ok();
            return StatusCode(500, "Failed to push customer to BC");
        }

            //This Task posts a WooCommerce customer to Business Central. It ensures that the Json Object String sent to BC is in the correct format.
            //Also it takes into account the two versions of Business Central authentications that are used in the project.
            private async Task<bool> PostCustomerToBusinessCentral(WooCustomer customer)
            {
            var customerJsonString = JsonConvert.SerializeObject(new
            {
                WooCommerceId = customer.WooCommerceId,
                Name = customer.Name,
                Email = customer.Email
            });

            var payload = new
            {
                customerJson = customerJsonString
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            bool tryBasicAuth = false;

            try
            {
                var handler = new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                    PreAuthenticate = true
                };

                using (var client = new HttpClient(handler))
                {
                    var url = "http://localhost:7048/BC170/ODataV4/CreateCustomer_InsertCustomerWS?company=CRONUS%20UK%20Ltd.";
                    var res = await client.PostAsync(url, content);

                    if (res.IsSuccessStatusCode)
                        return true;

                    if ((int)res.StatusCode == 401 || (int)res.StatusCode == 403)
                    {
                        tryBasicAuth = true;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                tryBasicAuth = true;
            }

            if (tryBasicAuth)
            {
                // 2. Prøv Basic Auth
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            Encoding.UTF8.GetBytes("admin:Password")));

                    var url = "http://bc-container:7048/BC/ODataV4/CreateCustomer_InsertCustomerWS?company=CRONUS%20Danmark%20A%2FS";
                    var res = await client.PostAsync(url, content);

                    return res.IsSuccessStatusCode;
                }
            }

            return false;
        }
    }
}
