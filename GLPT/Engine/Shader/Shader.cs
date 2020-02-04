using System;
using System.Collections.Generic;
using System.IO;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Shader
    {
        private Dictionary<string, int> _uniforms;
        private int _vertHandle;
        private int _fragHandle;
        private int _progHandle;
        
        public void Create(string vert, string frag)
        {
            // Create the vertex shader
            _vertHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertHandle, ReadSource("vert.glsl"));
            GL.CompileShader(_vertHandle);
            GL.GetShader(_vertHandle, ShaderParameter.CompileStatus, out int status);
            if (status != 1) throw new Exception(GL.GetShaderInfoLog(_vertHandle));
            
            // Create the fragment shader
            _fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragHandle, ReadSource("frag.glsl"));
            GL.CompileShader(_fragHandle);
            GL.GetShader(_fragHandle, ShaderParameter.CompileStatus, out status);
            if(status != 1) throw new Exception(GL.GetShaderInfoLog(_vertHandle));
            
            // Create the program
            _progHandle = GL.CreateProgram();
            
            //Linking shaders to the program
            GL.AttachShader(_progHandle, _vertHandle);
            GL.AttachShader(_progHandle, _fragHandle);
            
            // Link the program
            GL.LinkProgram(_progHandle);
            GL.GetProgram(_progHandle, GetProgramParameterName.LinkStatus, out status);
            if(status != 1) throw new Exception(GL.GetProgramInfoLog(_progHandle));
            
            // Validating the program
            GL.ValidateProgram(_progHandle);
            GL.GetProgram(_progHandle, GetProgramParameterName.ValidateStatus, out status);
            if(status != 1) throw new Exception(GL.GetProgramInfoLog(_progHandle));

            // pre-caching the uniform variable locations before actually running the program
            // to allow for faster lookup, so we don't have to ask open-gl every time we want
            // to send some information to a uniform variable
            GL.GetProgram(_progHandle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(_progHandle, i, 256, out _, out int size, out ActiveUniformType type, out string name);
                int location = GL.GetUniformLocation(_progHandle, name);
                _uniforms.Add(name, location);
            }
            
        }

        private string ReadSource(string fileName)
        {
            // Todo Add inline includes
            
            return File.ReadAllText(fileName);
        }
    }
}