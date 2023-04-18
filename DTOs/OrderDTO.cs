using MovieStore.Models;

namespace MovieStore.DTOs
{
    public class OrderDTO
    {
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public double DiscountPrice { get; set; }
        public List<int> MovieIds { get; set; }
        public string CouponCode { get; set; }


    }
}
