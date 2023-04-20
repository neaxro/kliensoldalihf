using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvkereso.Model
{
    public class Docs
    {
        public int Cover_i { get; set; }
        public bool Has_fulltext { get; set; }
        public int Edition_count { get; set; }
        public string Title { get; set; }
        public string[] Author_name { get; set; }
        public int First_publish_year { get; set; }
        public string Key { get; set; }
        public string[] Ia { get; set; }
        public string[] Author_key { get; set; }
        public bool Public_scan_b { get; set; }

        public string CoverUrl{ get; set; }
    }

}
