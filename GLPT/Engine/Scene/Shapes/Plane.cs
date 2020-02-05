using osuTK;

namespace GLPT
{
    public class Plane : IShape
    {
        public Vector3 Origin;
        public Vector3 Normal;

        public Plane(Vector3 origin, Vector3 normal)
        {
            Origin = origin;
            Normal = normal;
        }
    }
}