using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFMVVM.IHM.Services.Contracts;

namespace WPFMVVM.IHM.Services
{
    public class InternalUserData : IInternalUserData
    {
        public InternalUserData(string username,
            string email,
            string hashedPassword,
            string[] roles)
        {
            Username = username;
            Email = email;
            HashedPassword = hashedPassword;
            Roles = roles;
        }
        public string Username
        {
            get;
            private set;
        }

        public string Email
        {
            get;
            private set;
        }

        public string HashedPassword
        {
            get;
            private set;
        }

        public string[] Roles
        {
            get;
            private set;
        }
    }
}
