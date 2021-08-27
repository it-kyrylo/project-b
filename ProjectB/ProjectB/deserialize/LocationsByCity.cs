 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.deserialize
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
