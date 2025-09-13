namespace GM.MODEL.ViewModel.User
{
    public class UserInfoViewModel : StandardUser
    {
       


    }

    public class AccountInfoViewModel
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string RoleCode { get; set; }
        public string CoreRoles { get; set; }
    }
}