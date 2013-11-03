using Interfaces;
namespace Fukami.ViewModels
{
    public class NodeGeneViewModel : BaseGeneViewModel<IConnectionSlot>
    {
        public double Size
        {
            get { return Model.Size; }
            set
            {
                Model.Size = value;
                RaisePropertyChanged("Size");
            }
        }


        public override IConnectionSlot GetModel()
        {
            return Model.Duplicate();
        }
    }
}
