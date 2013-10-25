using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukamiDemo.ViewModels
{
    public class BoneGeneViewModel : ConnectorGeneViewModel
    {
        private double _thick;

        public double Thickness
        {
            get { return _thick; }
            set
            {
                _thick = value;
                RaisePropertyChanged("Thickness");
            }
        }

        private double _length;

        public double Length
        {
            get { return _length; }
            set
            {
                _length = value;
                RaisePropertyChanged("Length");
            }
        }


    }
}
