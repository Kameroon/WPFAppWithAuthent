using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WPFMVVM.IHM.Models;
using WPFMVVM.IHM.Services.Contracts;

namespace WPFMVVM.IHM.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //private class InternalUserData : IInternalUserData
        //{
        //    public InternalUserData(string username,
        //        string email,
        //        string hashedPassword,
        //        string[] roles)
        //    {
        //        Username = username;
        //        Email = email;
        //        HashedPassword = hashedPassword;
        //        Roles = roles;
        //    }
        //    public string Username
        //    {
        //        get;
        //        private set;
        //    }

        //    public string Email
        //    {
        //        get;
        //        private set;
        //    }

        //    public string HashedPassword
        //    {
        //        get;
        //        private set;
        //    }

        //    public string[] Roles
        //    {
        //        get;
        //        private set;
        //    }
        //}

        private static readonly List<IInternalUserData> _users = new List<IInternalUserData>()
        {
            new InternalUserData("Mark", "mark@company.com",
                "MB5PYIsbI2YzCUe34Q5ZU2VferIoI4Ttd+ydolWV0OE=",
                new string[] { "Administrators" }),
            new InternalUserData("John", "john@company.com",
                "hMaLizwzOQ5LeOnMuj+C6W75Zl5CXXYbwDSHWW9ZOXc=",
                new string[] { })
        };       // dw19Q8xM4ZOpOuTTdrBYh7VJImakLW6xvThLHWVRfFM=

        public IUser AuthenticateUser(string username, string clearTextPassword)
        {
            var res = CalculateHash(clearTextPassword, username);

            //InternalUserData userData = _users.FirstOrDefault(u => u.Username.Equals(username)
            //    && u.HashedPassword.Equals(CalculateHash(clearTextPassword, u.Username)));
            IInternalUserData userData = _users.FirstOrDefault(u => u.Username.Equals(username));
            if (userData == null)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            return new User(userData.Username, userData.Email, userData.Roles);
        }

        private string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }

}
