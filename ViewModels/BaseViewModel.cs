using System.ComponentModel;

namespace Fukami.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private BaseViewModel _parent;

        public BaseViewModel ParentViewModel
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged("ParentViewModel");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
