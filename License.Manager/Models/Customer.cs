using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace License.Manager.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }
}
