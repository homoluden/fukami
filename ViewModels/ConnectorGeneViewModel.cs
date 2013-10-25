using Physics2DDotNet.Joints;

namespace Fukami.ViewModels
{
    public class ConnectorGeneViewModel : BaseGeneViewModel
    {
        private HingeJoint _parHinge;

        public HingeJoint ParentHingeJoint
        {
            get { return _parHinge; }
            set
            {
                _parHinge = value;
                RaisePropertyChanged("ParentHingeJoint");
            }
        }

        private AngleJoint _parAngle;

        public AngleJoint ParentAngleJoint
        {
            get { return _parAngle; }
            set
            {
                _parAngle = value;
                RaisePropertyChanged("ParentAngleJoint");
            }
        }

        private HingeJoint _childHinge;

        public HingeJoint ChildHingeJoint
        {
            get { return _childHinge; }
            set
            {
                _childHinge = value;
                RaisePropertyChanged("ChildHingeJoint");
            }
        }

        private AngleJoint _childAngle;

        public AngleJoint ChildAngleJoint
        {
            get { return _childAngle; }
            set
            {
                _childAngle = value;
                RaisePropertyChanged("ChildAngleJoint");
            }
        }
        
    }
}
