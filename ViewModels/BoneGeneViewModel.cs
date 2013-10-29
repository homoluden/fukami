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
                    //new ConnectionSlotModel
                    //    {
                    //        IsOccupied = false,
                    //        MaxMass = 15,
                    //        MaxSize = 15,
                    //        RelativePosition = new ALVector2D{Angular = MathHelper.PiOver2, X = -10.0f, Y = mid - 10}
                    //    },
                    new ConnectionSlotModel
                        {
                            IsOccupied = false,
                            MaxMass = 15,
                            MaxSize = 15,
                            RelativePosition = new ALVector2D{Angular = 0.0f, X = 0.0f, Y = mid + 10}
                        }, 
                    //new ConnectionSlotModel
                    //    {
                    //        MaxMass = 15,
                    //        MaxSize = 15,
                    //        RelativePosition = new ALVector2D{Angular = -MathHelper.PiOver2 - 0.3, X = 10.0f, Y = mid - 10}
                    //    }
                }
            };
        }
    }
}
