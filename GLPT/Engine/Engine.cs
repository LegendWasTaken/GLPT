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
        
        public Engine(int width, int height, Scene scene)
        {
            Title = "GL Path Tracer";
            Width = width;
            Height = height;
            _scene = scene;
            _shader = new Shader();
            _cameraLocation = new Vector3(0, 0, 0);
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
            
            var placeholders = new List<Placeholder>();
            placeholders.Add(new Placeholder("%SCENE_SPHERE_COUNT%", spheres.Count.ToString()));
            
            // Initialize shaders
            _shader.Create(placeholders);
            
            
            /*================================*/// Scene setup
            _shader.SetUniform("spheres", spheres);
            /*================================*/
            
            /*================================*/// Camera Setup
            var theta = (float) (50 * Math.PI / 180);
            var halfHeight = (float) Math.Tan(theta / 2);
            var halfWidth = halfHeight * Width / Height;
            var w = Vector3.Normalize(_cameraLocation - new Vector3(0, 0, 1));
            var u = Vector3.Normalize(Vector3.Cross(new Vector3(0, 1, 0), w));
            var v = Vector3.Cross(w, u);
            var lookat = _cameraLocation - w;
            
            _shader.SetUniform("horizontal", 2 * halfWidth * u);
            _shader.SetUniform("vertical", 2 * halfHeight * v);
            _shader.SetUniform("lookat", lookat);
            _shader.SetUniform("camera_location", _cameraLocation);
            
            _shader.SetUniform("viewMatrix", Matrix4.Identity);
            /*================================*/
            
            
            _shader.SetUniform("res", new Vector2(Width, Height));
            _shader.SetUniform("color", new Vector3(.27F));

            base.OnLoad(e); // dont fucking touch this
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
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