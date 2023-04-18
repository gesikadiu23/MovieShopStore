namespace MovieStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<Movie> Movies { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
    }
}
