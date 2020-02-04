using osuTK;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class VBO
    {
        private int _handle;

        public VBO(params Vector3[] vertices)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);
            
        }
    }
}