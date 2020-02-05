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

        public void SetUniform(string name, params Vector4[] vecs)
        {
            GL.UseProgram(_progHandle);
            var location = _uniforms.GetValueOrDefault(name, -1);
            if (location != -1)
            { 
                GL.Uniform4(location, vecs.Length, ref vecs[0].X);
            }
        }

        public void SetUniform(string name, List<Plane> planes)
        {
            for (var i = 0; i < planes.Count; i++)
            {
                SetUniform($"{name}[{i}].origin", planes[i].Origin); 
                SetUniform($"{name}[{i}].normal", planes[i].Normal);
            }
        }
        
        public void SetUniform(string name, List<Sphere> spheres)
        {
            for (var i = 0; i < spheres.Count; i++)
            {
                SetUniform($"{name}[{i}].origin", spheres[i].Origin); 
                SetUniform($"{name}[{i}].radius", spheres[i].Radius);
            }
        }
        
        public void SetUniform(string name, Matrix4 matrix)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.UniformMatrix4(location, true, ref matrix);
        }
        
        public void SetUniform(string name, Vector4 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform4(location, vec);
        }

        public void SetUniform(string name, Vector3 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform3(location, vec);
        }
        
        public void SetUniform(string name, Vector2 vec)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform2(location, vec);
        }

        public void SetUniform(string name, float num)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform1(location, num);
        }
        
        public void SetUniform(string name, int num)
        {
            GL.UseProgram(_progHandle);
            int location = _uniforms.GetValueOrDefault(name, -1);
            if(location != -1) GL.Uniform1(location, num);
        }
        
        public void useShader()
        {
            GL.UseProgram(_progHandle);
        }
        
        public void Create(List<Placeholder> placeholders)
        {
            // Create the vertex shader
            _vertHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertHandle, ReadShader("vert.glsl", placeholders));
            GL.CompileShader(_vertHandle);
            GL.GetShader(_vertHandle, ShaderParameter.CompileStatus, out var status);
            if (status != 1) throw new Exception(GL.GetShaderInfoLog(_vertHandle));
            
            // Create the fragment shader
            _fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragHandle, ReadShader("frag.glsl", placeholders));
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
                Console.WriteLine($"UNIFORM: {name}, LOCATION: {location}");
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

        private string ReadShader(string fileName, List<Placeholder> placeholders)
        {
            return ReadShader(fileName, placeholders, new List<string>());
        }
        
        private string ReadShader(string fileName, List<Placeholder> placeholders, List<string> includePaths)
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
            for (var i = 0; i < match.Count; i++) 
            {
                file = file.Replace(match[i].Groups[0].Value, ReadShader(match[i].Groups[1].Value, placeholders, includePaths));
            }

            for (var i = 0; i < placeholders.Count; i++)
            {
                var current = placeholders[i];
                file = file.Replace(current.Key, current.Value);
            }
            includePaths.Remove(path);
            return file;
        }
    }
}