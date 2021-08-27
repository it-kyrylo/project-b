using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectB.deserialize
{
    public class Suggestion
    {
        public string Group { get; set; }

        public ICollection<Entity> Entities { get; set; }
    }
}
