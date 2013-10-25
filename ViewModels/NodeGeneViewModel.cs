namespace Fukami.ViewModels
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
