using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Credit> Credits { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}