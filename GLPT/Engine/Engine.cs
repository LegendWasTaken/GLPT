using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Engine
    {
        private Window _window;
        private Scene _scene;

        public Engine(int width, int height, Scene scene)
        {
            _window = new Window("C# Path Tracer", width, height);
            _scene = scene;
        }
        
        public void Initialize()
        {
            // do start stuff lol
            // Start the window instance
            _window.Run();
        }
        
    }
}