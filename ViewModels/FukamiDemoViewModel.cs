using AdvanceMath;
using CustomBodies;
using Fukami.ViewModels.Commands;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WorldControllers;

namespace Fukami.ViewModels
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

            var startPoint = new Vector2D(101, 300);
            double angle = MathHelper.ToRadians(15.0f);
            const double boxlength = 50;
            const double spacing = 2;
            const double anchorLength = 30;
            const double anchorGap = (boxlength / 2) + spacing + (anchorLength / 2);

            var modelId = Guid.NewGuid();

            var floor = WillHelper.CreateRectangle(10, 1024, double.PositiveInfinity, new ALVector2D(0, startPoint.X + 512, startPoint.Y)).AsModelBody(modelId);
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

        #region ApplyGeneCommand

        ICommand _applyGeneCommand;
        public ICommand ApplyGeneCommand
        {
            get
            {
                return _applyGeneCommand ??
                       (_applyGeneCommand = new RelayCommand(ApplyGeneCommandExecute, ApplyGeneCommandCanExecute));
            }
        }

        private void ApplyGeneCommandExecute(object parameter)
        {
            var gene = parameter as BaseGeneViewModel;
            if (gene == null)
            {
                throw new ArgumentException("Parameter is not a gene!");
            }
            var category = gene.Category;
            var geneApplicationId = Guid.NewGuid();
            switch (category)
            {
                case "Core":
                    //Add Core object to scene
                    var core = (CoreGeneViewModel)gene;
                    var model = core.GetModel();
                    var bodies = WillHelper.BuildCoreBody(model, geneApplicationId);
                    Will.Instance.AddModelBodies(bodies);
                    break;
                case "Node":
                    //Add Node object to scene
                    break;
                case "Bone":
                    //Add Bone object to scene
                    break;
                default:
                    //Nothing here yet
                    break;
            }
        }

        private bool ApplyGeneCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // ApplyGeneCommand


        #region Private Methods
        
        private IList<BaseGeneViewModel> GenerateGenes()
        {
            return new List<BaseGeneViewModel>
                {
                    new CoreGeneViewModel{Id = 1, Category = "Core", Description = "Gene of Core with three connector slots.", ParentViewModel = this,
                                            Size = 15, SpawningPosition = new ALVector2D(Math.PI/2+0.1f, 500, 600)},
                    new NodeGeneViewModel{Id = 2, Category = "Node", Description = "Node gene with Size: 15", Size = 15, ParentViewModel = this},
                    new NodeGeneViewModel{Id = 3, Category = "Node", Description = "Node gene with Size: 10", Size = 10, ParentViewModel = this},
                    new NodeGeneViewModel{Id = 4, Category = "Node", Description = "Node gene with Size: 20", Size = 20, ParentViewModel = this},
                    new BoneGeneViewModel{Id = 5, Category = "Bone", Description = "Bone gene with Size: {50 x 2}", Length = 50, Thickness = 2, ParentViewModel = this},
                    new BoneGeneViewModel{Id = 6, Category = "Bone", Description = "Bone gene with Size: {30 x 4}", Length = 30, Thickness = 4, ParentViewModel = this},
                    new BoneGeneViewModel{Id = 7, Category = "Bone", Description = "Bone gene with Size: {40 x 1}", Length = 40, Thickness = 1, ParentViewModel = this}
                };
        }

        #endregion


        #region Ctors

        public FukamiDemoViewModel()
        {
            FukamiGenes = GenerateGenes();
        }

        #endregion
    }
}
