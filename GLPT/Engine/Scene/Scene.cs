using System.Collections.Generic;
using osuTK;

namespace GLPT
{
    public class Scene
    {
        private List<IShape> _shapes = new List<IShape>();

        public void addShape(IShape shape)
        {
            _shapes.Add(shape);
        }

        public List<Sphere> GetSpheres()
        {
            var spheres = new List<Sphere>();
            foreach(var shape in _shapes)
            {
                if (shape is Sphere)
                {
                    spheres.Add((Sphere) shape);
                }
            }
            return spheres;
        }
        
    }
}