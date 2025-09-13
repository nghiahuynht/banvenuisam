using System.ComponentModel.DataAnnotations;

namespace GM.MODEL.ViewModel.User
{
    public class StandardUser
    {
        public int? UserId { get; set; }
        public string Code { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        public string RoleCode { get; set; }
       
        public string CoreRoles { get; set; }
    }

}