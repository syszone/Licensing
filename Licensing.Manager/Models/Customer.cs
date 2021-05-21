using System;

namespace Licensing.Manager.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }
}
