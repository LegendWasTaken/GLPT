using osuTK;

namespace GLPT
{
    public class Sphere : IShape
    {
        public Vector3 Origin;
        public float Radius;

        public Sphere(Vector3 origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }
    }
}