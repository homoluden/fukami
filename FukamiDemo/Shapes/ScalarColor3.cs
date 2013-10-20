
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif

namespace Shapes
{
    public struct ScalarColor3
    {
        public const int Count = 3;
        public const int Size = sizeof(Scalar) * Count;
        public Scalar Red;
        public Scalar Green;
        public Scalar Blue;
        public ScalarColor3(Scalar Red, Scalar Green, Scalar Blue)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
        }
    }
}
