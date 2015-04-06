using CustomBodies.Models;
using FarseerPhysics.Common;
using Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Fukami.ViewModels
{
    public class CoreGeneViewModel : BaseGeneViewModel<CoreModel>
    {
        #region Fields

        readonly Random _rnd = new Random(Environment.TickCount);
        
        #endregion


        #region Properties

        public ulong MaxHealth
        {
            get { return Model.MaxHealth; }
        }

        public float Size
        {
            get { return Model.Size; }
        }

        public Transform StartingTransform
        {
            get { return Model.StartingTransform; }
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
                var randomOffset = StartingTransform.p + new Vector2(_rnd.Next(-100, 100)*0.1f, _rnd.Next(-100, 100)*0.1f);
                var randomRotation = new Rot(StartingTransform.q.GetAngle() + _rnd.Next(-100, 100) * 0.003f);

                Model = new CoreModel
                {
                    StartingTransform = new Transform(ref randomOffset, ref randomRotation) ,
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
