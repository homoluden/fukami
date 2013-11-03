using AdvanceMath;
using CustomBodies.Models;
using Interfaces;
using Physics2DDotNet;
using System.Collections.Generic;
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

        public override BoneModel GetModelDuplicate()
        {
            if (Model == null)
            {

                var mid = this.Length * 0.5;

                Model = new BoneModel
                {
                    Length = this.Length,
                    Thickness = this.Thickness,
                    ChildSlots = new List<IConnectionSlot> { 
                        new ConnectionSlotModel
                            {
                                IsOccupied = false,
                                Size = 15,
                                MaxChildMass = 15,
                                MaxChildSize = 100,
                                DistanceFromCenter = mid + 4.0f,
                                Direction = 0.4,
                                Orientation = 1.15
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
                                Direction = -0.4,
                                Orientation = -1.15
                            }
                    }
                };
            }
            
            return (BoneModel)Model.Duplicate();
        }
    }
}
