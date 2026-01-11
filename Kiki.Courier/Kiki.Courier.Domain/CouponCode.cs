namespace Kiki.Courier.Domain
{
    public class CouponCode
    {
        public string Code { get; set; }
        public int MinimumWeight { get; set; }
        public int MaximumWeight { get; set; }
        public int DiscountPercentage { get; set; }
        public int MinimumDistance { get; set; }
        public int MaximumDistance { get; set; }
    }
}
