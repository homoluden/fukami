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
        private CoreModel _model;

        public CoreModel Model
        {
            get { return _model; }
            set 
            { 
                _model = value;
                RaisePropertyChanged("");
            }
        }

        public ulong MaxHealth
        {
            get { return Model.MaxHealth; }
            //set
            //{
            //    Model.MaxHealth = value;
            //    RaisePropertyChanged("MaxHealth");
            //}
        }

        public double Size
        {
            get { return Model.Size; }
            //set
            //{
            //    Model.Size = value;
            //    RaisePropertyChanged("Size");
            //}
        }

        public ALVector2D SpawningPosition
        {
            get { return Model.StartPosition; }
            //set
            //{
            //    Model.StartPosition = value;
            //    RaisePropertyChanged("SpawningPosition");
            //}
        }

        public IEnumerable<IConnectionSlot> ConnectionSlots
        {
            get { return Model.ConnectionSlots; }
            //set
            //{
            //    Model.ConnectionSlots = value;
            //    RaisePropertyChanged("ConnectionSlots");
            //}
        }

        #endregion

        #region Public Methods

        public override CoreModel GetModel()
        {
            if (Model == null)
            {
                Model = new CoreModel
                {
                    StartPosition = this.SpawningPosition + new ALVector2D(_rnd.Next(-100, 100) * 0.003, _rnd.Next(-100, 100) * 0.1, _rnd.Next(-100, 100) * 0.1),
                    Size = this.Size,
                    Mass = 200,
                    ConnectionSlots = new []
                        {
                            new ConnectionSlotModel
                                {
                                    IsOccupied = false,
                                    MaxMass = 15,
                                    MaxSize = 15,
                                    DistanceFromCenter = this.Size + 10.0f,
                                    Direction = MathHelper.PiOver2 + 0.3f,
                                    Orientation = -0.6f
                                },
                            new ConnectionSlotModel
                                {
                                    IsOccupied = false,
                                    MaxMass = 15,
                                    MaxSize = 15,
                                    DistanceFromCenter = this.Size + 10.0f,
                                    Direction = 0.0f,
                                    Orientation = 0.0f
                                }, 
                            new ConnectionSlotModel
                                {
                                    MaxMass = 15,
                                    MaxSize = 15,
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
