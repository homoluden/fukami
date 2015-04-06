using AdvanceMath;
using CustomBodies;
using Fukami.ViewModels.Commands;
using Physics2DDotNet;
using Physics2DDotNet.Joints;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WorldControllers;
using System.Linq;
using CustomBodies.Models;
using Interfaces;

namespace Fukami.ViewModels
{
    public class FukamiDemoViewModel : BaseViewModel
    {
        #region Fields

        private Random _rnd = new Random(Environment.TickCount);

        #endregion

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

            var modelId = Guid.NewGuid();

            var floor = WillHelper.CreateRectangle(50, 1024, double.PositiveInfinity, new ALVector2D(0, startPoint.X + 512, startPoint.Y)).AsModelBody(modelId);
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
                    AddCore(geneApplicationId, gene);
                    break;
                case "Node":
                    //Add Bone Slot Body
                    AddSlot(gene);
                    break;
                case "Bone":
                    //Add Bone object to scene
                    AddBone(gene);
                    break;
                case "InterCon":
                    //Add Interconnection object to scene
                    AddInterconnection(gene);
                    break;
                default:
                    // Nothing there yet
                    break;
            }
        }

        private bool ApplyGeneCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // ApplyGeneCommand


        #region Private Methods

        private void AddCore(Guid geneApplicationId, BaseGeneViewModel gene)
        {
            var core = (CoreGeneViewModel)gene;
            var model = core.GetModelDuplicate();
            model.Id = geneApplicationId;
            
            var coreBody = WillHelper.CreateCoreBody(model, geneApplicationId);

            var nodes = WillHelper.BuildNodeSlots(coreBody, geneApplicationId);
            coreBody.Children = nodes;
            var corePos = coreBody.State.Position;

            var joints = new List<Joint>();
            foreach (var node in nodes)
            {
                var hinge = new HingeJoint(coreBody, node, (2* node.State.Position.Linear + 8 * corePos.Linear) * 0.1f, new Lifespan())
                {
                    DistanceTolerance = 10,
                    Softness = 100.0
                };
                var angle = new AngleJoint(coreBody, node, new Lifespan()) { Softness = 0.0001, BiasFactor = 0.2f };

                joints.Add(hinge);
                joints.Add(angle);
            }


            Will.Instance.AddModelBodies(new List<BaseModelBody>{ coreBody }.Concat(nodes).ToList());
            Will.Instance.AddJoints(joints);
        }

        private void AddBone(BaseGeneViewModel gene)
        {
            Will.Instance.RunPauseWilling(false);

            var boneGene = (BoneGeneViewModel)gene;

            var boneModel = boneGene.GetModelDuplicate();

            var boneBody = WillHelper.AddBoneBody(boneModel);

            Will.Instance.RunPauseWilling(true);
        }

        private void AddSlot(BaseGeneViewModel gene)
        {
            Will.Instance.RunPauseWilling(false);

            var slotGene = (NodeGeneViewModel)gene;

            var boneBody = Will.Instance.Bodies.RandomOrDefault<BoneBody>(b => b.Model.ChildSlots.Any(s => s.IsOccupied == false));

            if (boneBody == null)
            {
                Will.Instance.RunPauseWilling(true);
                return;
            }

            var parPos = boneBody.State.Position;

            var randSlot = boneBody.Model.ChildSlots.Where(s => s.IsOccupied == false).RandomOrDefault();

            var slot = slotGene.GetModelDuplicate();
            slot.Direction = randSlot.Direction;
            slot.DistanceFromCenter = randSlot.DistanceFromCenter;
            slot.Orientation = randSlot.Orientation;

            randSlot.IsOccupied = true;

            var slotBody = WillHelper.CreateConnectionSlotBody(slot, boneBody.ModelId);
            

            var slotXAngle = slot.Direction + parPos.Angular;
            var slotCenter = Vector2D.Rotate(slotXAngle, new Vector2D(slot.DistanceFromCenter, 0.0f));
            var slotPos = new ALVector2D(slot.Orientation + slotXAngle, slotCenter + parPos.Linear);

            slotBody.State.Position = slotPos;
            slotBody.ApplyPosition();

            slotBody.Parent = boneBody;

            boneBody.Children.Add(slotBody);

            var joints = new List<Joint>();

            var nodePos = slotBody.State.Position;

            var hinge = new HingeJoint(boneBody, slotBody, (slot.Size * nodePos.Linear + boneBody.Model.Length * parPos.Linear) * (1/(slot.Size + boneBody.Model.Length)), new Lifespan())
            {
                DistanceTolerance = 50,
                Softness = 10.1
            };
            var angle = new AngleJoint(boneBody, slotBody, new Lifespan()) { Softness = 0.00001 };

            joints.Add(hinge);
            joints.Add(angle);

            Will.Instance.AddBody(slotBody);
            Will.Instance.AddJoints(joints);

            Will.Instance.RunPauseWilling(true);
        }


        private void AddInterconnection(BaseGeneViewModel gene)
        {
            var interConnGene = (InterconnectionViewModel)gene;
            var model = interConnGene.GetModelDuplicate();

            var body = WillHelper.TryAddInterconnectionBody(model, 5);
        }


        private IList<BaseGeneViewModel> GenerateGenes()
        {
            var coreSize = 30;
            var result = new List<BaseGeneViewModel>
                {
                    new CoreGeneViewModel
                    {
                        Id = 1, 
                        Category = "Core", 
                        Description = "Gene of Core with three connector slots.", 
                        ParentViewModel = this,
                        Model = new CoreModel
                            {
                                StartPosition = new ALVector2D(MathHelper.PiOver2, 700 + _rnd.Next(-100, 100) * 0.1, 600 + _rnd.Next(-100, 100) * 0.1),
                                Size = coreSize,
                                Density = 1,
                                ConnectionSlots = new []
                                    {
                                        new ConnectionSlotModel
                                            {
                                                IsOccupied = false,
                                                Size = 25,
                                                MaxChildMass = 15,
                                                MaxChildSize = 100,
                                                DistanceFromCenter = coreSize + 10.0f,
                                                Direction = MathHelper.PiOver2 + 0.3f,
                                                Orientation = -0.8f
                                            },
                                        new ConnectionSlotModel
                                            {
                                                IsOccupied = false,
                                                Size = 25,
                                                MaxChildMass = 15,
                                                MaxChildSize = 100,
                                                DistanceFromCenter = coreSize + 10.0f,
                                                Direction = 0.0f,
                                                Orientation = 0.0f
                                            }, 
                                        new ConnectionSlotModel
                                            {
                                                IsOccupied = false,
                                                Size = 25,
                                                MaxChildMass = 15,
                                                MaxChildSize = 100,
                                                DistanceFromCenter = coreSize + 10.0f,
                                                Direction = -MathHelper.PiOver2 - 0.3f,
                                                Orientation = 0.8f
                                            }
                                    }
                            }
                    },
                    new NodeGeneViewModel
                    {
                        Id = 2, 
                        Category = "Node", 
                        Description = "Node gene with Size: 6", 
                        ParentViewModel = this,
                        Model = new ConnectionSlotModel{
                            Size = 6,
                            MaxChildMass = 15,
                            MaxChildSize = 100,
                        }
                    },
                    new NodeGeneViewModel
                    {
                        Id = 3, 
                        Category = "Node", 
                        Description = "Node gene with Size: 7", 
                        ParentViewModel = this,
                        Model = new ConnectionSlotModel{
                            Size = 7,
                            MaxChildMass = 15,
                            MaxChildSize = 100,
                        }
                    },
                    new NodeGeneViewModel
                    {
                        Id = 4, 
                        Category = "Node", 
                        Description = "Node gene with Size: 10", 
                        ParentViewModel = this,
                        Model = new ConnectionSlotModel{
                            Size = 10,
                            MaxChildMass = 15,
                            MaxChildSize = 100,
                        }
                    },
                    new BoneGeneViewModel
                    {
                        Id = 5, 
                        Category = "Bone", 
                        Description = "Bone gene with Size: {75 x 2}",
                        ParentViewModel = this,
                        Model = new BoneModel{
                            Length = 75,
                            Thickness = 2,
                            ChildSlots = new List<IConnectionSlot>{
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 40.0f,
                                        Direction = 0.4,
                                        Orientation = 1.15
                                    },
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 10,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 45.0f,
                                        Direction = 0.0f,
                                        Orientation = 0.0f
                                    }, 
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 40.0f,
                                        Direction = -0.4,
                                        Orientation = -1.15
                                    }
                            }
                        }
                    },
                    new BoneGeneViewModel
                    {
                        Id = 6, 
                        Category = "Bone", 
                        Description = "Bone gene with Size: {60 x 4}", 
                        ParentViewModel = this,
                        Model = new BoneModel{
                            Length = 60,
                            Thickness = 4,
                            ChildSlots = new List<IConnectionSlot>{
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 30.0f,
                                        Direction = 0.4,
                                        Orientation = 1.15
                                    },
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 10,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 35.0f,
                                        Direction = 0.0f,
                                        Orientation = 0.0f
                                    }, 
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 30.0f,
                                        Direction = -0.4,
                                        Orientation = -1.15
                                    }
                            }
                        }
                    },
                    new BoneGeneViewModel
                    {
                        Id = 7, 
                        Category = "Bone",
                        Description = "Bone gene with Size: {40 x 1}",
                        ParentViewModel = this,
                        Model = new BoneModel{
                            Length = 40,
                            Thickness = 1,
                            ChildSlots = new List<IConnectionSlot>{
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 20.0f,
                                        Direction = 0.4,
                                        Orientation = 1.15
                                    },
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 10,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 25.0f,
                                        Direction = 0.0f,
                                        Orientation = 0.0f
                                    }, 
                                new ConnectionSlotModel
                                    {
                                        IsOccupied = false,
                                        Size = 15,
                                        MaxChildMass = 15,
                                        MaxChildSize = 100,
                                        DistanceFromCenter = 20.0f,
                                        Direction = -0.4,
                                        Orientation = -1.15
                                    }
                            }
                        }
                    },
                    new InterconnectionViewModel
                    {
                        Id = 8,
                        Category = "InterCon",
                        Description = "",
                        ParentViewModel = this,
                        Model = new InterconnectionModel
                        {
                            Length = 100,
                            MaxMissAlign = MathHelper.ToRadians(30),
                            MaxDistance = 300
                        }
                    }
                };
            return result;
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
