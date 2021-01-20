using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCabBooker.Models
{
    public class MapDirection
    {
    
        [JsonProperty("geocoded_waypoints")]
        public IList<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        [JsonProperty("routes")]
        public IList<Route> Routes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

}
