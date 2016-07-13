using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class PublishArticleModel
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
