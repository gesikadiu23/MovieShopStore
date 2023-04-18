namespace MovieStore.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public int MovieId { get; set; }
    }
}
