using BitirmeProjesi.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BitirmeProjesi.DTO
{


    public class RecommendResponse 
    {
        public List<ProductDto> Recommendations { get; set; }
        [JsonIgnore] // JSON'dan gelmeyecekse
        public int UserId { get; set; }

    }

    public class ProductDto
    {
        [JsonProperty("p_id")]
        public int PId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("p_attributes")]
        public string? PAttributes { get; set; }

        [JsonProperty("colour")]
        public string? Colour { get; set; }

        [JsonProperty("avg_rating")]
        public double? AvgRating { get; set; }

        [JsonProperty("ratingcount")]
        public double? RatingCount { get; set; }

        [JsonProperty("img")]
        public string? Img { get; set; }

    }
}
