using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public int ParentManuName { get; set; }
        public bool HasAccess { get; set; }
    }
}
