using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WooToDynConnector.Models;
using System.IO;

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

            System.IO.File.AppendAllText("C:\\Users\\Ronnie\\Documents\\testLog.txt", wooOrder.ToString());

            var success = await PostStudentInsert(wooOrder.Id.ToString());
            if (success)
                return Ok();
            return StatusCode(200, "OK");
            
            //var success = await PostOrderToBusinessCentral(wooOrder);
            //if (success)
            //    return Ok();
            //return StatusCode(500, "Failed to push order to BC");
        }

        [HttpPost("testBC")]
        public async Task<bool> PostStudentInsert(string name)
        {
            var payload = new
            {
                Student_no = "444",
                First_Name = name,
                Last_Name = "Doe",
                Birthday = "2000-01-01",
                Education = "DMU",
                Phone_no = "12345678"
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:Password")));

            var url = "http://bc-container:7048/BC/ODataV4/Company('CRONUS%20Danmark%20A%2FS')/StudentOData";
            var res = await client.PostAsync(url, content);

            return res.IsSuccessStatusCode;
        }

        private async Task<bool> PostOrderToBusinessCentral(WooOrder order)
        {
            var payload = new
            {
                externalOrderId = order.Id,
                customerNo = order.CustomerId.ToString(),
                orderDate = order.DateCreated.ToString("yyyy-MM-dd"),
                lines = order.LineItems.Select(x => new
                {
                    itemNo = x.Sku,
                    quantity = x.Quantity,
                    unitPrice = x.Price
                })
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password")));

            var url = "https://yourbc/api/yourcompany/orders/v1.0/SalesOrders";
            var res = await client.PostAsync(url, content);

            return res.IsSuccessStatusCode;
        }
    }
}
