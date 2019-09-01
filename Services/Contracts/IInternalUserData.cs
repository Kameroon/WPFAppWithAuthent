namespace WPFMVVM.IHM.Services.Contracts
{
    public interface IInternalUserData
    {
        string Email { get; }
        string HashedPassword { get; }
        string[] Roles { get; }
        string Username { get; }
    }
}