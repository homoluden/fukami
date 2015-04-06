using System;
using CustomBodies.Models;
using System.Collections.Generic;
using Physics2DDotNet;
using AdvanceMath;
using Interfaces;

namespace Fukami.ViewModels
{
    public class CoreGeneViewModel : BaseGeneViewModel<CoreModel>
    {
        #region Fields

        Random _rnd = new Random(1133);
        
        #endregion


        #region Properties

        public ulong MaxHealth
        {
            get { return Model.MaxHealth; }
        }

        public double Size
        {
            get { return Model.Size; }
        }

        public ALVector2D SpawningPosition
        {
            get { return Model.StartPosition; }
        }

        public IEnumerable<IConnectionSlot> ConnectionSlots
        {
            get { return Model.ConnectionSlots; }
        }

        #endregion

        #region Public Methods

        public override CoreModel GetModelDuplicate()
        {
            if (Model == null)
            {
                Model = new CoreModel
                {
                    StartPosition = this.SpawningPosition + new ALVector2D(_rnd.Next(-100, 100) * 0.003, _rnd.Next(-100, 100) * 0.1, _rnd.Next(-100, 100) * 0.1),
                    Size = this.Size,
                    Density = 200,
                    ConnectionSlots = new []
                        {
                            new ConnectionSlotModel
                                {
                                    IsOccupied = false,
                                    Size = 15,
                                    MaxChildMass = 15,
                                    MaxChildSize = 100,
                                    DistanceFromCenter = this.Size + 10.0f,
                                    Direction = MathHelper.PiOver2 + 0.3f,
                                    Orientation = -0.6f
                                },
                            new ConnectionSlotModel
                                {
                                    IsOccupied = false,
                                    Size = 15,
                                    MaxChildMass = 15,
                                    MaxChildSize = 100,
                                    DistanceFromCenter = this.Size + 10.0f,
                                    Direction = 0.0f,
                                    Orientation = 0.0f
                                }, 
                            new ConnectionSlotModel
                                {
                                    IsOccupied = false,
                                    Size = 15,
                                    MaxChildMass = 15,
                                    MaxChildSize = 100,
                                    DistanceFromCenter = this.Size + 10.0f,
                                    Direction = -MathHelper.PiOver2 - 0.3f,
                                    Orientation = 0.6f
                                }
                        }
                };
            }
            return (CoreModel)Model.Clone();
        }

        #endregion

        public CoreGeneViewModel()
        {
        }

        public CoreGeneViewModel(ulong id, string category, string description)
            : base(id, category, description)
        {

        }
    }
}
