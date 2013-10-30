using AdvanceMath;
using CustomBodies.Models;
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



        public override BoneModel GetModel()
        {
            var mid = this.Length * 0.5;

            return new BoneModel
            {
                Length = this.Length,
                Thickness = this.Thickness,
                ChildSlots = new List<ConnectionSlotModel> { 
                    new ConnectionSlotModel
                        {
                            IsOccupied = false,
                            MaxMass = 15,
                            MaxSize = 15,
                            DistanceFromCenter = mid + 5.0f,
                            Direction = 0.3,
                            Orientation = 1.15
                        },
                    new ConnectionSlotModel
                        {
                            IsOccupied = false,
                            MaxMass = 15,
                            MaxSize = 15,
                            DistanceFromCenter = mid + 5.0f,
                            Direction = 0.0f,
                            Orientation = 0.0f
                        }, 
                    new ConnectionSlotModel
                        {
                            MaxMass = 15,
                            MaxSize = 15,
                            DistanceFromCenter = mid + 5.0f,
                            Direction = -0.3,
                            Orientation = -1.15
                        }
                }
            };
        }
    }
}
