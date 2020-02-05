using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Engine
    {
        private Window _window;
        public Engine(int width, int height)
        {
            _window = new Window("C# Path Tracer", width, height);
        }
        
        public void Initialize()
        {
            // do start stuff lol
            _window.Run();
        }
        
    }
}