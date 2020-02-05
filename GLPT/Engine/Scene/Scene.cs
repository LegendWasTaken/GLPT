using System.Collections.Generic;

namespace GLPT
{
    public class Scene
    {
        private List<Shape> _shapes = new List<Shape>();

        public void addShape(Shape shape)
        {
            _shapes.Add(shape);
        }
        
        
    }
}