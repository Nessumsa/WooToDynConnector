//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using System.Net.Http.Headers;
//using System.Text;
//using WooToDynConnector.Models;

//namespace WooToDynConnector.Controllers
//{
//    public class CustomerController
//    {
//        [ApiController]
//        [Route("api/customers")]
//        public class OrderController : ControllerBase
//        {
//            [HttpPost("receive")]
//            public async Task<IActionResult> ReceiveWooCustomer([FromBody] JObject orderJson)
//            {
//                var wooCustomer = orderJson.ToObject<WooCustomer>();

//                System.IO.File.AppendAllText("C:\\Users\\Ronnie\\Documents\\testLog.txt", wooCustomer.ToString());

//                return StatusCode(200, "OK");
//                //var success = await PostToBusinessCentral(wooCustomer);
//                //if (success)
//                //    return Ok();
//                //return StatusCode(500, "Failed to push customer to BC");
//            }

            


//            //private async Task<bool> PostCustomerToBusinessCentral(WooCustomer customer)
//            //{
//            //    var payload = new
//            //    {
//            //        externalOrderId = order.Id,
//            //        customerNo = order.CustomerId.ToString(),
//            //        orderDate = order.DateCreated.ToString("yyyy-MM-dd"),
//            //        lines = order.LineItems.Select(x => new
//            //        {
//            //            itemNo = x.Sku,
//            //            quantity = x.Quantity,
//            //            unitPrice = x.Price
//            //        })
//            //    };

//            ////http://bc-container:7048/BC/ODataV4/Company('CRONUS%20Danmark%20A%2FS')/StudentOData
//            ////http://bc-container:7047/BC/WS/CRONUS%20Danmark%20A%2FS/Codeunit/StudentService

//            //    var json = JsonConvert.SerializeObject(payload);
//            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

//            //    var client = new HttpClient();
//            //    client.DefaultRequestHeaders.Authorization =
//            //        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:Password")));

//            //    var url = "http://bc-container:7047/BC/WS/CRONUS%20Danmark%20A%2FS/Codeunit/StudentService";
//            //    var res = await client.PostAsync(url, content);

//            //    return res.IsSuccessStatusCode;
//            //}
//        }
//    }
//}
