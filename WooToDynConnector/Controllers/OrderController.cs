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
        //This Task is creating an endpoint for receiving the WooCommerce orders. This endpoint is used in the webhook creation at WooCommerce.
        //It receives the data in Json format from WooCommerce and the calls the Task to post it to Business Central.
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveWooOrder([FromBody] JObject orderJson)
        {
            var wooOrder = orderJson.ToObject<WooOrder>();

            var success = await PostOrderToBusinessCentral(wooOrder);
            if (success)
                return Ok();
            return StatusCode(500, "Failed to push to BC");
        }

        //This Task posts a WooCommerce order to Business Central. It ensures that the Json Object String sent to BC is in the correct format.
        //Also it takes into account the two versions of Business Central authentications that are used in the project.
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

                    if (res.IsSuccessStatusCode)
                        return true;

                    if ((int)res.StatusCode == 401 || (int)res.StatusCode == 403)
                    {
                        tryBasicAuth = true;
                    }
                }
            }
            catch (HttpRequestException ex) {tryBasicAuth = true;}

            if (tryBasicAuth)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            Encoding.UTF8.GetBytes("admin:Password")));

                    var url = "http://bc-container:7048/BC/ODataV4/CreateSalesOrder_CreateOrder?company=CRONUS%20Danmark%20A%2FS";
                    var res = await client.PostAsync(url, content);

                    return res.IsSuccessStatusCode;
                }
            }
            return false;
        }
    }
}

