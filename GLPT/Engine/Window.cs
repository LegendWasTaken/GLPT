using System;
using System.Drawing;
using osuTK;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Window : GameWindow
    {
        private Shader _shader;
        public Window(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            _shader = new Shader();
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
            
            // Initialize shaders
            _shader.Create();
            
            _shader.setUniform("color", new Vector3(0, 255, 255));
            _shader.setUniform("camera_location", new Vector3(0, 0, 0));
            
            _shader.setUniform("viewMatrix", Matrix4.Identity);
            // _shader.setUniform("res", new Vector2(Width, Height));

            base.OnLoad(e); // dont fucking touch this
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Game loop
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            /*=====================*/
            _shader.useShader();
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