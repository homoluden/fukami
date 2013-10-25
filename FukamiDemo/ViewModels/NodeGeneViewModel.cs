using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukamiDemo.ViewModels
{
    public class NodeGeneViewModel : ConnectorGeneViewModel
    {
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

    }
}
