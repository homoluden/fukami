using AdvanceMath;
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
            double boxlength = 50;
            double spacing = 4;
            double anchorLength = 30;
            double anchorGap = (boxlength / 2) + spacing + (anchorLength / 2);

            var chain = WillHelper.BuildChain(new Vector2D(0, 0), boxlength, 10, 200, spacing, 600);
            
            var point2 = new Vector2D(chain[chain.Count - 1].State.Position.Linear.X + anchorGap, 500);
            var end2 = WillHelper.AddCircle(anchorLength / 2, 14, double.PositiveInfinity, new ALVector2D(0, point2));
            end2.IgnoresGravity = true;
            var joint2 = new HingeJoint(chain[chain.Count - 1], end2, point2, new Lifespan());
            joint2.DistanceTolerance = 10;

        }

        private bool AddChainCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // RunPauseCommand    


    }
}
