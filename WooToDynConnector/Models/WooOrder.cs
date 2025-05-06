using Newtonsoft.Json;

namespace WooToDynConnector.Models
{
    public class WooOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public List<LineItem> LineItems { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, CustomerID: {CustomerId}, Date: {DateCreated}" + Environment.NewLine;
        }
    }
}
