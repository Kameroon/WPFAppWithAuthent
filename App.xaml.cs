using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using WPFMVVM.IHM.Authent;
using WPFMVVM.IHM.Models;
using WPFMVVM.IHM.Services;
using WPFMVVM.IHM.Services.Contracts;
using WPFMVVM.IHM.ViewModels;
using WPFMVVM.IHM.ViewModels.Contracts;
using WPFMVVM.IHM.Views;

namespace WPFMVVM.IHM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            //Create a custom principal with an anonymous identity at startup  
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IUser, User>();
            container.RegisterType<IInternalUserData, InternalUserData>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();

            var loginViewModel = container.Resolve<LoginViewModel>();
            var loginView = new LoginWindow
            {
                DataContext = loginViewModel
            };
            loginView.Show();
        }
    }
}
