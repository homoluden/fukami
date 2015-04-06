using CustomBodies;
using CustomBodies.Models;
using Fukami.ViewModels.Commands;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using WorldControllers;

namespace Fukami.ViewModels
{
    public class FukamiDemoViewModel : BaseViewModel
    {
        #region Fields

        private readonly Random _rnd = new Random(Environment.TickCount);

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
            if (Will.Instance.IsRunning)
            {
                Will.Instance.Stop();
            }
            else
            {
                Will.Instance.Run();
            }
        }

        private bool RunPauseCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion // RunPauseCommand    
        
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

            var floorPosition = new Vector2(101, 300);
            //var angle = MathHelper.ToRadians(15.0f);

            var floorBody = WillHelper.CreateStaticBody();
            var floorModel = WillHelper.CreateModelForBody<BaseModelBody>(floorBody);

            floorBody.Position = floorPosition;

            floorBody.AttachRectangleFixture(50, 1024, float.PositiveInfinity, new Vector2(-512, -25));

            Representation.Instance.RegisterCompositeModel(floorModel.ModelId, new List<BaseModelBody>{floorModel});

            Will.Instance.Run();
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
                                StartingTransform = new ALVector2D(MathHelper.PiOver2, 700 + _rnd.Next(-100, 100) * 0.1, 600 + _rnd.Next(-100, 100) * 0.1),
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
