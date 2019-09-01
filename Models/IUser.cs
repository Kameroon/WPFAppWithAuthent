namespace WPFMVVM.IHM.Models
{
    public interface IUser
    {
        string Email { get; set; }
        string[] Roles { get; set; }
        string Username { get; set; }
    }
}