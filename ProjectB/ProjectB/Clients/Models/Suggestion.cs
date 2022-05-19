﻿namespace ProjectB.Clients.Models;

public class Suggestion
{
    public string Group { get; set; }

    [JsonPropertyName("entities")]
    public ICollection<CityProperty> CityProperties { get; set; }
}
