using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvkereso.Models
{

    public class BookDetail
    {
        public string first_publish_date { get; set; }
        public string key { get; set; }
        public string title { get; set; }
        public Author[] authors { get; set; }
        public Type type { get; set; }
        public int[] covers { get; set; }
        public string description { get; set; }
        public string[] lc_classifications { get; set; }
        public Link[] links { get; set; }
        public string[] dewey_number { get; set; }
        public string[] subject_places { get; set; }
        public string[] subject_people { get; set; }
        public Excerpt[] excerpts { get; set; }
        public string[] subjects { get; set; }
        public int latest_revision { get; set; }
        public int revision { get; set; }
        public Created created { get; set; }
        public Last_Modified last_modified { get; set; }
    }

    public class Type
    {
        public string key { get; set; }
    }

    public class Created
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Last_Modified
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Author
    {
        public Author1 author { get; set; }
        public Type1 type { get; set; }
    }

    public class Author1
    {
        public string key { get; set; }
    }

    public class Type1
    {
        public string key { get; set; }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
        public Type2 type { get; set; }
    }

    public class Type2
    {
        public string key { get; set; }
    }

    public class Excerpt
    {
        public string excerpt { get; set; }
        public string comment { get; set; }
        public Author2 author { get; set; }
    }

    public class Author2
    {
        public string key { get; set; }
    }

}
