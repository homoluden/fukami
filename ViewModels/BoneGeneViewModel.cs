using CustomBodies.Models;
namespace Fukami.ViewModels
{
    public class BoneGeneViewModel : BaseGeneViewModel<BoneModel>
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



        public override BoneModel GetModel()
        {
            return new BoneModel
            {
                Length = this.Length,
                Thickness = this.Thickness
            };
        }
    }
}
