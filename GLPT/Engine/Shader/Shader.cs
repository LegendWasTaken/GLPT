using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using osuTK;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class Shader
    {
        private Dictionary<string, int> _uniforms;
        private int _vertHandle;
        private int _fragHandle;
        private int _progHandle;

        public void setUniform(string name, Matrix4 matrix)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.UniformMatrix4(location, true, ref matrix);
        }
        
        public void setUniform(string name, Vector4 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform4(location, vec);
        }

        public void setUniform(string name, Vector3 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform3(location, vec);
        }
        
        public void setUniform(string name, Vector2 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform2(location, vec);
        }
        
        
        public void setUniform(string name, int num)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform1(location, num);
        }
        
        public void useShader()
        {
            GL.UseProgram(_progHandle);
        }
        
        public void Create()
        {
            // Create the vertex shader
            _vertHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertHandle, ReadShader("vert.glsl"));
            GL.CompileShader(_vertHandle);
            GL.GetShader(_vertHandle, ShaderParameter.CompileStatus, out int status);
            if (status != 1) throw new Exception(GL.GetShaderInfoLog(_vertHandle));
            
            // Create the fragment shader
            _fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragHandle, ReadShader("frag.glsl"));
            GL.CompileShader(_fragHandle);
            GL.GetShader(_fragHandle, ShaderParameter.CompileStatus, out status);
            if(status != 1) throw new Exception(GL.GetShaderInfoLog(_fragHandle));
            
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
            _uniforms = new Dictionary<string, int>(uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(_progHandle, i, 256, out _, out int size, out ActiveUniformType type, out string name);
                int location = GL.GetUniformLocation(_progHandle, name);
                _uniforms.Add(name, location);
            }
        }

        public void Release()
        {
            GL.DetachShader(_progHandle, _vertHandle);
            GL.DetachShader(_progHandle, _fragHandle);
            GL.DeleteShader(_vertHandle);
            GL.DeleteShader(_fragHandle);
            GL.DeleteProgram(_progHandle);
        }

        private string ReadShader(string fileName)
        {
            return ReadShader(fileName, new List<string>());
        }
        
        private string ReadShader(string fileName, List<string> includePaths)
        {
            var path = Path.Combine("Assets", "Shaders", fileName);
            if (includePaths.Contains(path))
            {
                throw new Exception($"SHADER INCLUDE ERROR: Recursive include @ file[{fileName}]");
            }
            includePaths.Add(path);
            var file = File.ReadAllText(path);
            var pattern = new Regex("#include\\s*\"(.+)\"");
            var match = pattern.Matches(file);
            for (int i = 0; i < match.Count; i++) 
            {
                file = file.Replace(match[i].Groups[0].Value, ReadShader(match[i].Groups[1].Value, includePaths));
            }
            includePaths.Remove(path);
            return file;
        }
    }
}