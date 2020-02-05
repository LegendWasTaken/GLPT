using osuTK;
using osuTK.Input;

namespace GLPT
{
    public class Camera
    {
        private const float LOOK_SPEED = 1.1F;
        private const float MOVE_SPEED = 2.3F;

        private Vector3 _translation = Vector3.Zero;
        
        private float _pitch;
        private float _yaw;

        public Matrix4 GetViewMatrix() => Matrix4.Mult(Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(_pitch, _yaw, 0)), Matrix4.CreateTranslation(_translation));
        
        public void Update(float deltaTime)
        {
            // Looking around, so rotation
            var lookDelta = GetLookVector() * deltaTime * LOOK_SPEED;
            _pitch += lookDelta.Z;
            _yaw += lookDelta.Y;
            
            // Translation
            var td = GetTranslationVector() * deltaTime * MOVE_SPEED;
            _translation += (new Vector4(td.X, td.Y, td.Z, 1) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(_pitch, _yaw, 0))).Xyz;
        }

        private Vector3 GetTranslationVector()
        {
            var translate = Vector3.Zero;
            
            if(IsKeyDown(Key.W))
                translate += new Vector3(0f, 0f, 1f);
            if(IsKeyDown(Key.S))
                translate += new Vector3(0f, 0f, -1f);
            if(IsKeyDown(Key.A))
                translate += new Vector3(-1f, 0f, 0f);
            if(IsKeyDown(Key.D))
                translate += new Vector3(1f, 0f, 0f);
            if(IsKeyDown(Key.Space))
                translate += new Vector3(0f, 1f, 0f);
            if(IsKeyDown(Key.ShiftLeft))
                translate += new Vector3(0f, -1f, 0f);
            return translate;
        }
        
        private Vector3 GetLookVector()
        {
            var look = Vector3.Zero;

            if (IsKeyDown(Key.Up))
                look += new Vector3(0f, 0f, -1f);
            if (IsKeyDown(Key.Down))
                look += new Vector3(0f, 0f, 1f);
            if (IsKeyDown(Key.Right))
                look += new Vector3(0, 1f, 0);
            if(IsKeyDown(Key.Left))
                look += new Vector3(0, -1f, 0);
            return look;
        }

        public bool IsKeyDown(Key key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}