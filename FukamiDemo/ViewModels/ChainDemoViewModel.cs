using AdvanceMath;
using CustomBodies;
using FukamiDemo.Commands;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldControllers;

namespace FukamiDemo.ViewModels
{
    public class ChainDemoViewModel : BaseViewModel
    {

        #region RunPauseCommand

        ICommand _runPauseCommand;
        public ICommand RunPauseCommand
        {
            get
            {
                if (_runPauseCommand == null)
                {
                    _runPauseCommand = new RelayCommand(RunPauseCommandExecute, RunPauseCommandCanExecute);
                }
                return _runPauseCommand;
            }
        }

        private void RunPauseCommandExecute(object parameter)
        {
            Will.Instance.RunPauseWilling();
        }

        private bool RunPauseCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // RunPauseCommand    


        #region AddChainCommand

        ICommand _addChainCommand;
        public ICommand AddChainCommand
        {
            get
            {
                if (_addChainCommand == null)
                {
                    _addChainCommand = new RelayCommand(AddChainCommandExecute, AddChainCommandCanExecute);
                }
                return _addChainCommand;
            }
        }

        private void AddChainCommandExecute(object parameter)
        {
            Will.Instance.Purge();
            Will.Instance.RunPauseWilling(false);

            double angle = MathHelper.ToRadians(-15.0f);
            double boxlength = 50;
            double spacing = 2;
            double anchorLength = 30;
            double anchorGap = (boxlength / 2) + spacing + (anchorLength / 2);

            var chainId = Guid.NewGuid();

            var chain = WillHelper.BuildChain(new Vector2D(150, 150), boxlength, 3, 5000, spacing, 600, chainId);
            
            var point2 = new Vector2D(chain[chain.Count - 1].State.Position.Linear.X + anchorGap, 150);
            var end2 = WillHelper.AddCircle(anchorLength / 2, 6, double.PositiveInfinity, new ALVector2D(0, point2), chainId);
            end2.IgnoresGravity = true;
            chain.Add(end2);

            var joint2 = new HingeJoint(chain[chain.Count - 1], end2, point2, new Lifespan()) {DistanceTolerance = 50};
            var joint21 = new AngleJoint(chain[chain.Count - 1], end2, new Lifespan()) { Angle = angle };

            var point1 = new Vector2D(chain[0].State.Position.Linear.X - anchorGap, 150);
            var end1 = WillHelper.AddCircle(anchorLength / 2, 6, double.PositiveInfinity, new ALVector2D(0, point1), chainId);
            chain.Add(end1);

            end1.IgnoresGravity = true;
            var joint1 = new HingeJoint(chain[0], end1, point1, new Lifespan()) {DistanceTolerance = 50};
            var joint11 = new AngleJoint(end1, chain[0], new Lifespan()) { Angle = angle };

            Will.Instance.AddJoint(joint1);Will.Instance.AddJoint(joint11);
            Will.Instance.AddJoint(joint2);Will.Instance.AddJoint(joint21);

            Representation.Instance.RegisterModel(chainId, chain);

            Will.Instance.RunPauseWilling(true);
        }

        private bool AddChainCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // RunPauseCommand    


    }
}
