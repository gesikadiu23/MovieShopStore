namespace MovieStore.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Actor> Actors { get; set;}
        public ICollection<Review> Reviews { get; set; }    
        public double price { get; set; }
        public int QuantityAvailable { get; set; }
        //public double price
        
    }
}
