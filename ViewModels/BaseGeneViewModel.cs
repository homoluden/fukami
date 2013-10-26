using System;
using System.Windows.Input;
using Fukami.ViewModels.Commands;

namespace Fukami.ViewModels
{
    public abstract class BaseGeneViewModel<T> : BaseGeneViewModel
    {


        #region Public Methods

        public abstract T GetModel();

        #endregion


        #region Ctors

        protected BaseGeneViewModel()
        {
            
        }

        protected BaseGeneViewModel(ulong id, string category, string description)
            : base(id, category, description)
        {
        }

        #endregion
    }

    public abstract class BaseGeneViewModel : BaseViewModel
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


        #region Ctors

        protected BaseGeneViewModel()
        {
        }

        protected BaseGeneViewModel(ulong id, string category, string description)
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
