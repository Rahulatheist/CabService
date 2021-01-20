﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCabBooker.Models
{
    public class StructuredFormatting
    {
        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }
    public class Prediction
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("structured_formatting")]
        public StructuredFormatting StructuredFormatting { get; set; }

    }
    public class AutoCompletePlaceForGoogleMap
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("predictions")]
        public List<Prediction> AutoCompletePlaces { get; set; }
    }
}