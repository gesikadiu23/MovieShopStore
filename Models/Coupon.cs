namespace MovieStore.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public int CouponValue { get; set; }
        public DateTime CouponExpirationDate { get; set; }
    }
}
