using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WooToDynConnector.Models;
using System.IO;
using System.Diagnostics;

namespace WooToDynConnector.Controllers
{
    
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveWooOrder([FromBody] JObject orderJson)
        {
            var wooOrder = orderJson.ToObject<WooOrder>();

            var success = await PostOrderToBusinessCentral(wooOrder);
            if (success)
                return Ok();
            return StatusCode(500, "Failed to push to BC");
        }

        [HttpPost("create")]
        public async Task<bool> PostOrderToBusinessCentral(WooOrder order)
        {
            var orderJsonAsString = JsonConvert.SerializeObject(new
            {
                WooOrderId = order.Id,
                WooCommerceCustomerId = order.CustomerId,
                LineItems = order.LineItems?.Select(x => new
                {
                    ItemNo = x.ItemNo,
                    Quantity = x.Quantity
                })
            });

            var payload = new { orderJson = orderJsonAsString };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            bool tryBasicAuth = false;

            // 1. Prøv DefaultCredentials
            try
            {
                var handler = new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                    PreAuthenticate = true
                };

                using (var client = new HttpClient(handler))
                {
                    var url = "http://localhost:7048/BC170/ODataV4/CreateSalesOrder_CreateOrder?company=CRONUS%20UK%20Ltd.";
                    var res = await client.PostAsync(url, content);

                    Debug.WriteLine($"DefaultCredentials Status: {res.StatusCode}");

                    if (res.IsSuccessStatusCode)
                        return true;

                    if ((int)res.StatusCode == 401 || (int)res.StatusCode == 403)
                    {
                        Debug.WriteLine("DefaultCredentials autoriseringsfejl – forsøger Basic Auth...");
                        tryBasicAuth = true;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Netværksfejl ved brug af DefaultCredentials: " + ex.Message);
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

                    var url = "http://bc-container:7048/BC/ODataV4/CreateSalesOrder_CreateOrder?company=CRONUS%20Danmark%20A%2FS";
                    var res = await client.PostAsync(url, content);

                    Debug.WriteLine($"Basic Auth Status: {res.StatusCode}");

                    return res.IsSuccessStatusCode;
                }
            }

            return false;
        }


    }
}

