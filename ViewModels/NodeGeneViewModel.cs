using Interfaces;
namespace Fukami.ViewModels
{
    public class NodeGeneViewModel : BaseGeneViewModel<IConnectionSlot>
    {
        public float Size
        {
            get { return Model.Size; }
            set
            {
                Model.Size = value;
                RaisePropertyChanged("Size");
            }
        }


        public override IConnectionSlot GetModelDuplicate()
        {
            return Model.Duplicate();
        }
    }
}
