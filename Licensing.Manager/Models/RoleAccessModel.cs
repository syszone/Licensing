using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class RoleAccessModel
    {
        public string RoleId { get; set; }
        public List<RoleItem> RolesItem { get; set; }
    }
    public partial class RoleItem
    {
        public int Id { get; set; }
        public bool Selected { get; set; }
    }
}
