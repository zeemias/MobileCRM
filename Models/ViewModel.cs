using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class ViewModel
    {
        public Credit Credit { get; set; }
        public List<Story> Story { get; set; }
        public List<Comment> Comment { get; set; }
    }
}