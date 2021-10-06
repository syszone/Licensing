using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class AspNetUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
    public class RoleIdViewModel
    {
        public string RoleId { get; set; }
    }
    public class SearchMenuItemViewModel
    {
        public string UserId { get; set; }
    }
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public int ParentManuName { get; set; }
        public bool HasAccess { get; set; } = false;
        public string UserId { get; set; }
        public string URL { get; set; }
    }
}
