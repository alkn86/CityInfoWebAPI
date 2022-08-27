namespace CityInfo.API.Models
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            this.Cities = new List<CityDto>()
            {
                new CityDto(){ Id = 1, Name = "Odessa", Description = "Odessa City", PointsOfInterest = {
                        new PointOfInterestDto() { Id = 1, Name = "Opera theater", Description = "Visit at night" },
                        new PointOfInterestDto() { Id = 2, Name = "Shevchenko Park", Description = "Good to ride a bake" },
                        new PointOfInterestDto() { Id = 3, Name = "Poskot", Description = "Criminal district" }
                    }
                },
                new CityDto(){ Id = 2, Name = "Kharkiv", Description = "Khrkiv City", PointsOfInterest = {
                        new PointOfInterestDto() {Id = 1, Name = "Saltovka", Description = "Something to buy"},
                        new PointOfInterestDto() {Id = 2, Name = "University", Description = "Nice place to stutdy"}
                    }
                    
      
                },
                new CityDto(){ Id = 3, Name = "Kherson", Description = "I love watermelons", PointsOfInterest = {
                    new PointOfInterestDto() {Id = 1, Name = "Downtown", Description = "Nice place to raise a meeting"}
                    }
                }
            };

        }
    }
}
