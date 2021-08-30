using System.Collections.Generic;

namespace ProjectB.Clients.Models
{
    public class LocationsByCity
    {
        public string Term { get; set; }

        public long MoreSuggestions { get; set; }

        public string AutoSuggestInstance { get; set; }

        public string TrackingId { get; set; }

        public bool MisSpellingFallBack { get; set; }

        
        public ICollection<Suggestion> Suggestions { get; set; }
    }
}
