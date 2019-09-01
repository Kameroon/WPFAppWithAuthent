using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVM.IHM.ViewModels
{
    public class AdminViewModel: ViewModelBase
    {

        private string _info;

        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                NotifyPropertyChanged("Info");
            }
        }


        public AdminViewModel()
        {

        }
    }
}
