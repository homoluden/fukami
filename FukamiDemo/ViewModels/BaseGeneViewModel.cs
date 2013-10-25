using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdvanceMath;
using FukamiDemo.Commands;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using WorldControllers;

namespace FukamiDemo.ViewModels
{
    public class BaseGeneViewModel : BaseViewModel
    {
        #region Properties

        private ulong _id;

        public ulong Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        
        #endregion


        #region AddGeneToWorldCommand

        ICommand _addGeneToWorldCommand;
        public ICommand AddGeneToWorldCommand
        {
            get
            {
                return _addGeneToWorldCommand ??
                       (_addGeneToWorldCommand = new RelayCommand(AddGeneToWorldCommandExecute, AddGeneToWorldCommandCanExecute));
            }
        }

        private void AddGeneToWorldCommandExecute(object parameter)
        {
            
        }

        private bool AddGeneToWorldCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // AddGeneToWorldCommand


        #region Ctors

        public BaseGeneViewModel()
        {
        }

        public BaseGeneViewModel(ulong id, string category, string description)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException("category");
            }

            Id = id;
            Category = category;
            Description = description;
        }
        #endregion
    }
}
