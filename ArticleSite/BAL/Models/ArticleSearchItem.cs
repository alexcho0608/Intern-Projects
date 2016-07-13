using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ArticleSearchItem
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public bool Status { get; set; }
    }
}
