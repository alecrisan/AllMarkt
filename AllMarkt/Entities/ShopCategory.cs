namespace AllMarkt.Entities
{
    public class ShopCategory
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
