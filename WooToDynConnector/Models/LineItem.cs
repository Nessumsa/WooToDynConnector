namespace WooToDynConnector.Models
{
    public class LineItem
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public override string ToString()
        {
            return $"Sku: {Sku}, Quantity: {Quantity}, Price: {Price}";
        }
    }
}
