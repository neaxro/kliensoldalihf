using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvkereso.Model
{

    public class SearchResult
    {
        public int Start { get; set; }
        public int Num_found { get; set; }
        public List<Docs> Docs { get; set; }
    }

}
