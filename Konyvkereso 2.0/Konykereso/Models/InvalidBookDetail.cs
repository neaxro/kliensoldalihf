using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvkereso.Models
{
    public class InvalidBookDetail: BookDetail
    {
        public string title { get; set; }

        public InvalidBookDetail(string title)
        {
            this.title = title;
        }
    }
}
