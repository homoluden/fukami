using CustomBodies.Models;
using Interfaces;
using System.Collections.Generic;
namespace Fukami.ViewModels
{
    public class BoneGeneViewModel : BaseGeneViewModel<BoneModel>
    {
        public float Thickness
        {
            get { return Model.Thickness; }
            set
            {
                Model.Thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }

        public float Length
        {
            get { return Model.Length; }
            set
            {
                Model.Length = value;
                RaisePropertyChanged("Length");
            }
        }

        public override BoneModel GetModelDuplicate()
        {
            return Model.Duplicate();
        }

        #region Citors

        public BoneGeneViewModel()
            : this(0, "Bone", string.Empty)
        {
        }

        protected BoneGeneViewModel(ulong id, string category, string description)
            : base(id, category, description)
        {
            const float mid = 10.0f;

            Model = new BoneModel
            {
                Length = mid * 2,
                Thickness = 3.0f,
                ChildSlots = new List<IConnectionSlot> { 
                        new ConnectionSlotModel
                            {
                                IsOccupied = false,
                                Size = 15,
                                MaxChildMass = 15,
                                MaxChildSize = 100,
                                DistanceFromCenter = mid + 4.0f,
                                Direction = 0.4f,
                                Orientation = 1.15f
                            },
                        new ConnectionSlotModel
                            {
                                IsOccupied = false,
                                Size = 10,
                                MaxChildMass = 15,
                                MaxChildSize = 100,
                                DistanceFromCenter = mid + 6.0f,
                                Direction = 0.0f,
                                Orientation = 0.0f
                            }, 
                        new ConnectionSlotModel
                            {
                                IsOccupied = false,
                                Size = 15,
                                MaxChildMass = 15,
                                MaxChildSize = 100,
                                DistanceFromCenter = mid + 4.0f,
                                Direction = -0.4f,
                                Orientation = -1.15f
                            }
                    }
            };
        }

        #endregion
    }
}
