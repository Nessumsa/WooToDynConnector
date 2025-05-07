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
            [HttpPost("receive")]
            public async Task<IActionResult> ReceiveWooCustomer([FromBody] JObject orderJson)
            {
                var wooCustomer = orderJson.ToObject<WooCustomer>();

                System.IO.File.AppendAllText("C:\\Users\\Ronnie\\Documents\\testCustomerLog.txt", wooCustomer.ToString());

            var success = await PostCustomerToBusinessCentral(wooCustomer);
            if (success)
                return Ok();
            return StatusCode(500, "Failed to push customer to BC");
        }




            private async Task<bool> PostCustomerToBusinessCentral(WooCustomer customer)
            {
            var customerJsonString = JsonConvert.SerializeObject(new
            {
                Name = customer.Name,
                Email = customer.Email
            });

            var payload = new
            {
                customerJson = customerJsonString
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:Password")));

            var url = "http://bc-container:7048/BC/ODataV4/CreateCustomer_InsertCustomerWS?company=CRONUS%20Danmark%20A%2FS";
            var res = await client.PostAsync(url, content);

            if (!res.IsSuccessStatusCode)
            {
                var handler = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                client = new HttpClient(handler);
                using (client)
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }
                url = "http://localhost:7048/BC170/ODataV4/NewCustomerWS_InsertCustomerWS?company=CRONUS%20UK%20Ltd";
                res = await client.PostAsync(url, content);
            }

            return res.IsSuccessStatusCode;
            }
        }
    
}
