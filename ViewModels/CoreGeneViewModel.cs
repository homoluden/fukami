using System.Collections.Generic;

namespace Fukami.ViewModels
{
    public class CoreGeneViewModel : BaseGeneViewModel
    {
        #region Properties

        private int _maxHealth;

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                RaisePropertyChanged("MaxHealth");
            }
        }

        private double _size;

        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                RaisePropertyChanged("Size");
            }
        }

        private IList<object> _connSlots;

        public IList<object> ConnectionSlots
        {
            get { return _connSlots; }
            set
            {
                _connSlots = value;
                RaisePropertyChanged("ConnectionSlots");
            }
        }

        #endregion

        public CoreGeneViewModel()
        {
        }

        public CoreGeneViewModel(ulong id, string category, string description)
            : base(id, category, description)
        {

        }
    }
}
