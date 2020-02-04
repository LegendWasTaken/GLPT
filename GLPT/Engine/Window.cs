using System;
using System.Drawing;
using System.Runtime.InteropServices;
using osuTK;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Window : GameWindow
    {
        public Window(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            // Start up
            DebugMessageHandler.Initialize(); // Starting the debug message handler 

            new VBO(
                new Vector3(-20f, -1f, 0f),
                new Vector3(2f, -20f, 0f),
                new Vector3(2f, 2f, 0)
                );
            base.OnLoad(e); // dont fucking touch this
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Game loop
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // do stuff here
            
            
            // dont do stuff here
            SwapBuffers();
            base.OnRenderFrame(e);
        }
        

    }
}