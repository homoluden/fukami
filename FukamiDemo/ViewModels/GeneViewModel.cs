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
    public class GeneViewModel : BaseViewModel
    {
        #region Properties

        public ulong Id { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }
        
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

        public GeneViewModel(ulong id, string category, string description)
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
