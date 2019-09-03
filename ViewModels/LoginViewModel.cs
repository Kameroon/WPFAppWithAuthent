using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFMVVM.IHM.Authent;
using WPFMVVM.IHM.Commands;
using WPFMVVM.IHM.Models;
using WPFMVVM.IHM.Services.Contracts;
using WPFMVVM.IHM.ViewModels.Contracts;
using WPFMVVM.IHM.Views;

namespace WPFMVVM.IHM.ViewModels
{
    public class LoginViewModel : ViewModelBase, IViewModel
    {
        #region -- Event send data --
        //public static event OnNewData NotifyDataReady;
        //delegate void OnNewData(IUser Data);

        ////all the other classes register
        //MainClass.OnNewData += Listner;

        //public void Listener(IUser Data)
        //{
        //    var MyLocalCopy = Data;
        //} 
        #endregion

        #region --   --
        private readonly IAuthenticationService _authenticationService;
        private string _username;
        private string _status;
        private string _role;
        #endregion

        #region -- Properties --
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                if (string.IsNullOrEmpty(_username))
                {

                }
                NotifyPropertyChanged("Username");
            }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrators") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }

        private bool _isAdminViewVisible;

        public bool IsAdminViewVisible
        {
            get { return _isAdminViewVisible; }
            set
            {
                _isAdminViewVisible = value;
                NotifyPropertyChanged("IsAdminViewVisible");
            }
        }
                
        private bool _isUserAuthenticated;

        public bool IsUserAuthenticated
        {
            get { return _isUserAuthenticated; }
            set
            {
                _isUserAuthenticated = value;
                NotifyPropertyChanged("IsUserAuthenticated");
            }
        }

        private bool _isNotAuthenticated;

        public bool IsNotAuthenticated
        {
            get { return _isNotAuthenticated; }
            set
            {
                _isNotAuthenticated = value;
                NotifyPropertyChanged("IsNotAuthenticated");
            }
        }
        #endregion   
        
        public DelegateCommand ShowViewCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        private AdminViewModel _adminViewModel { get; set; }
        #region --   --
        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            _adminViewModel = new AdminViewModel();

            IsAdminViewVisible = false;
            IsUserAuthenticated = false;
            IsNotAuthenticated = true;
            
            // --  Show send mail form command  --            
            //ShowViewCommand = new DelegateCommand(x => ShowView());
            LoginCommand = new DelegateCommand(Login, CanLogin);
            LogoutCommand = new DelegateCommand(Logout, CanLogout);
            ShowViewCommand = new DelegateCommand(OnShowView, null);
        }
        #endregion

        #region --  --
        
        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox?.Password;
            try
            {
                //Validate credentials through the authentication service
                IUser user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                #region --  --
                _role = user.Roles.FirstOrDefault();
                if (_role == null)
                {
                    IsAdminViewVisible = false;
                    IsUserAuthenticated = true;
                    IsNotAuthenticated = false;


                }
                else
                {
                    IsUserAuthenticated = false;
                    IsAdminViewVisible = true;
                    IsNotAuthenticated = false;

                    AdminView adminView = new AdminView();
                    adminView.DataContext = _adminViewModel;

                    _adminViewModel.Info = _role;
                }
                #endregion

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                //_loginCommand.RaiseCanExecuteChanged();
                //_logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;  
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                //_loginCommand.RaiseCanExecuteChanged();
                //_logoutCommand.RaiseCanExecuteChanged();

                //IsAdminAuthenticated = false;
                IsUserAuthenticated = false;
                IsNotAuthenticated = true;

                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            //return IsAuthenticated;
            return true;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void OnShowView(object parameter)
        {
            try
            {
                Status = string.Empty;
                //IView view;
                if (parameter == null)
                {

                }
                else
                {

                }
            }
            catch (SecurityException)
            {
                Status = "You are not authorized!";
            }
        }

        #endregion
    }
}
