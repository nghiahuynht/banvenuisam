using System.ComponentModel.DataAnnotations;

namespace GM.MODEL.ViewModel.Common
{
    public class UserIdentityViewModel
    {
        [Required]
        public int UserId { get; set; }
    }
}