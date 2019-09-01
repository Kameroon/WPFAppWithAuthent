using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFMVVM.IHM.Models;

namespace WPFMVVM.IHM.Services.Contracts
{
    public interface IAuthenticationService
    {
        IUser AuthenticateUser(string username, string password);
    }
}
