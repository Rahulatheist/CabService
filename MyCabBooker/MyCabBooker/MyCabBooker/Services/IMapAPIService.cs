using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCabBooker.Services
{
    public interface  IMapAPIService
    {
        Task<GoogleDirection> GetMyDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
        Task<GooglePlaceAutoCompleteResult> GetNewPlaces(string text);
        Task<GooglePlace> GetMyPlaceDetails(string placeId);
    }
}
