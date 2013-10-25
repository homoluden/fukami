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
    public class FukamiDemoViewModel : BaseViewModel
    {
        #region Properties
        public IList<BaseGeneViewModel> FukamiGenes { get; private set; }
        #endregion


        #region RunPauseCommand

        ICommand _runPauseCommand;
        public ICommand RunPauseCommand
        {
            get {
                return _runPauseCommand ??
                       (_runPauseCommand = new RelayCommand(RunPauseCommandExecute, RunPauseCommandCanExecute));
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
            get {
                return _addChainCommand ??
                       (_addChainCommand = new RelayCommand(AddChainCommandExecute, AddChainCommandCanExecute));
            }
        }

        private void AddChainCommandExecute(object parameter)
        {
            Will.Instance.Purge();
            Will.Instance.RunPauseWilling(false);

            var startPoint = new Vector2D(300, 800);
            double angle = MathHelper.ToRadians(15.0f);
            double boxlength = 50;
            double spacing = 2;
            double anchorLength = 30;
            double anchorGap = (boxlength / 2) + spacing + (anchorLength / 2);

            var chainId = Guid.NewGuid();

            var chain = WillHelper.BuildChain(startPoint, boxlength, 3, 1200, spacing, 600, chainId);
            
            var point2 = new Vector2D(chain[chain.Count - 1].State.Position.Linear.X + anchorGap, startPoint.Y);
            var end2 = WillHelper.AddCircle(anchorLength / 2, 6, double.PositiveInfinity, new ALVector2D(0, point2), chainId);
            end2.IgnoresGravity = true;

            var joint2 = new HingeJoint(chain[chain.Count - 1], end2, point2, new Lifespan()) {DistanceTolerance = 20};
            var joint21 = new AngleJoint(chain[chain.Count - 1], end2, new Lifespan()) { Angle = angle };

            var point1 = new Vector2D(chain[0].State.Position.Linear.X - anchorGap, startPoint.Y);
            var end1 = WillHelper.AddCircle(anchorLength / 2, 6, double.PositiveInfinity, new ALVector2D(0, point1), chainId);
            
            chain.Add(end1);
            chain.Add(end2);

            end1.IgnoresGravity = true;
            var joint1 = new HingeJoint(chain[0], end1, point1, new Lifespan()) {DistanceTolerance = 20};
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

        #endregion // AddChainCommand    

        #region AddCoreCommand

        ICommand _addCoreCommand;
        public ICommand AddCoreCommand
        {
            get {
                return _addCoreCommand ??
                       (_addCoreCommand = new RelayCommand(AddCoreCommandExecute, AddCoreCommandCanExecute));
            }
        }

        private void AddCoreCommandExecute(object parameter)
        {
            Will.Instance.Purge();
            Will.Instance.RunPauseWilling(false);

            var startPoint = new Vector2D(100, 300);
            double angle = MathHelper.ToRadians(15.0f);
            const double boxlength = 50;
            const double spacing = 2;
            const double anchorLength = 30;
            const double anchorGap = (boxlength / 2) + spacing + (anchorLength / 2);

            var modelId = Guid.NewGuid();

            var floor = WillHelper.CreateRectangle(10, 1024, 5000, new ALVector2D(0, startPoint.X + 512, startPoint.Y)).AsModelBody(modelId);
            floor.IgnoresGravity = true;

            Will.Instance.AddBody(floor);

            Representation.Instance.RegisterModel(modelId, new List<BaseModelBody>{floor});

            Will.Instance.RunPauseWilling(true);
        }

        private bool AddCoreCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // AddCoreCommand


        #region Private Methods
        
        private IList<BaseGeneViewModel> GenerateRandomGenes()
        {
            return new List<BaseGeneViewModel>();
        }

        #endregion


        #region Ctors

        public FukamiDemoViewModel()
        {
            FukamiGenes = GenerateRandomGenes();
        }

        #endregion
    }
}
