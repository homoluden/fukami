using AdvanceMath;
using CustomBodies.Models;
using Interfaces;
using Physics2DDotNet;
using System;
using System.Collections.Generic;
namespace Fukami.ViewModels
{
    public class BoneGeneViewModel : BaseGeneViewModel<BoneModel>
    {
        private double _thick;
        public double Thickness
        {
            get { return Model.Thickness; }
            set
            {
                Model.Thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }

        private double _length;
        public double Length
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
            return (BoneModel)Model.Duplicate();
        }

        #region Citors

        public BoneGeneViewModel()
            : this(0, "Bone", string.Empty)
        {
        }

        protected BoneGeneViewModel(ulong id, string category, string description)
            : base(id, category, description)
        {
            var mid = 10.0;

            Model = new BoneModel
            {
                Length = mid * 2,
                Thickness = 3.0,
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

        #endregion
    }
}
