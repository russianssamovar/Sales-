using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    public class OrderBooks
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int OrderId { get; set; }
    }
}
