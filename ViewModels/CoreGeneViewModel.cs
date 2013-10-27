using System;
using CustomBodies.Models;
using System.Collections.Generic;
using Physics2DDotNet;

namespace Fukami.ViewModels
{
    public class CoreGeneViewModel : BaseGeneViewModel<CoreModel>
    {
        #region Fields
        Random _rnd = new Random(1133);
        #endregion

        #region Properties

        private int _maxHealth;

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                RaisePropertyChanged("MaxHealth");
            }
        }

        private double _size;

        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                RaisePropertyChanged("Size");
            }
        }
        private ALVector2D _spawnsAt;

        public ALVector2D SpawningPosition
        {
            get { return _spawnsAt; }
            set
            {
                _spawnsAt = value;
                RaisePropertyChanged("SpawningPosition");
            }
        }

        private IList<object> _connSlots;

        public IList<object> ConnectionSlots
        {
            get { return _connSlots; }
            set
            {
                _connSlots = value;
                RaisePropertyChanged("ConnectionSlots");
            }
        }

        #endregion

        #region Public Methods

        public override CoreModel GetModel()
        {
            return new CoreModel
                {
                    StartPosition = this.SpawningPosition + new ALVector2D(_rnd.Next(-100, 100) * 0.001, _rnd.Next(-100, 100) * 0.01, _rnd.Next(-100, 100) * 0.01),
                    Size = this.Size,
                    Mass = 100,
                    ConnectionSlots = new []
                        {
                            new ConnectionSlotModel
                                {
                                    MaxMass = 15,
                                    MaxSize = 15,
                                    RelativePosition = new ALVector2D{Angular = Math.PI, X = -this.Size - 30, Y = -this.Size}
                                },
                            new ConnectionSlotModel
                                {
                                    MaxMass = 15,
                                    MaxSize = 15,
                                    RelativePosition = new ALVector2D{Angular = Math.PI, X = 0, Y = this.Size + 30}
                                }, 
                            new ConnectionSlotModel
                                {
                                    MaxMass = 15,
                                    MaxSize = 15,
                                    RelativePosition = new ALVector2D{Angular = Math.PI, X = this.Size + 30, Y = -this.Size}
                                }
                        }
                };
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
