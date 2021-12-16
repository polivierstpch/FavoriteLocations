using SQLite;

namespace FavoriteLocations.Models
{
    public class Configuration
    {   
        [PrimaryKey]
        public string UserIdentifier { get; set; }
        
        public bool ShowKnownLocations { get; set; }
        
        public bool ShowWishedLocations { get; set; }

        public bool ShowVisitedLocations { get; set; }

        public double LatitudeDegrees { get; set; }

        public double LongitudeDegrees { get; set; }
    }
}