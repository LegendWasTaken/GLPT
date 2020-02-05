using System;
using System.Collections.Generic;
using System.Drawing;
using osuTK;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Engine : GameWindow
    {
        private float _time;
        private Scene _scene;
        private Shader _shader;
        private Vector3 _cameraLocation;
        private Camera _camera;
        
        public Engine(int width, int height, Scene scene)
        {
            Title = "GL Path Tracer";
            Width = width;
            Height = height;
            _scene = scene;
            _shader = new Shader();
            _cameraLocation = new Vector3(0, 0, 0);
            _camera = new Camera();
        }
        
        public void Initialize()
        {
            // Start the window
            Run();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            // Start up
            DebugMessageHandler.Initialize(); // Starting the debug message handler 

            // Setup viewport stuff
            GL.Viewport(0, 0, Width, Height);
            
            // Create the full screen triangle
            new VBO(
                new Vector3(-5f, -1f, 0f),
                new Vector3(1f, -1f, 0f),
                new Vector3(1f, 5f, 0)
            );

            var spheres = _scene.GetSpheres();
            var planes = _scene.GetPlanes();
            
            var placeholders = new List<Placeholder>();
            placeholders.Add(new Placeholder("%SCENE_SPHERE_COUNT%", spheres.Count > 0 ? spheres.Count.ToString() : "1"));
            placeholders.Add(new Placeholder("%SCENE_CHECK_SPHERE%", (spheres.Count != 0).ToString().ToLower()));
            placeholders.Add(new Placeholder("%SCENE_PLANE_COUNT%", planes.Count > 0 ? planes.Count.ToString() : "1"));
            placeholders.Add(new Placeholder("%SCENE_CHECK_PLANE%", (planes.Count != 0).ToString().ToLower()));
            
            // Initialize shaders
            _shader.Create(placeholders);

            /*================================*/// Scene setup
            _shader.SetUniform("spheres", spheres);
            _shader.SetUniform("planes", planes);
            /*================================*/
            
            
            _shader.SetUniform("res", new Vector2(Width, Height));
            _shader.SetUniform("color", new Vector3(.27F));

            base.OnLoad(e); // dont fucking touch this
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _camera.Update((float)e.Time);
            
            _shader.SetUniform("viewMatrix", _camera.GetViewMatrix());
            
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            /*=====================*/
            _shader.useShader();

            _time += (float) e.Time;
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            /*=====================*/
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        public override void Exit()
        {
            _shader.Release();
            base.Exit();
        }
    }
}