namespace NFM.Domain.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}
