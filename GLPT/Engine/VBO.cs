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
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            GL.EnableVertexArrayAttrib(vao, 0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }
    }
}