using SQLite;

namespace FavoriteLocations.Models
{
    public enum Category
    {
        Visited = 0,
        Wished  = 1,
        Known   = 2
    }
    
    public class FavoriteLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserIdentifier { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        
        public Category Category { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}