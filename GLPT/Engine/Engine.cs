using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Engine
    {
        private Window _window;
        private Shader _shader;
        
        public Engine(int width, int height)
        {
            _window = new Window("C# Path Tracer", width, height);
        }
        
        public void Initialize()
        {
            // do start stuff lol
            _window.Run();
            _shader = new Shader();
            
        }
        
    }
}