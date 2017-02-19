using Hellion.Core.Helpers;
using System;

namespace Hellion.Core.Structures
{
    /// <summary>
    /// Represents 3D coordinates in the world.
    /// </summary>
    public class Vector3
    {
        private float x;
        private float y;
        private float z;

        /// <summary>
        /// Gets or sets the X position in the world.
        /// </summary>
        public float X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Gets or sets the Y position in the world.
        /// </summary>
        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// Gets or sets the Z position in the world.
        /// </summary>
        public float Z
        {
            get { return this.z; }
            set { this.z = value; }
        }

        /// <summary>
        /// Gets the vector length.
        /// </summary>
        public float Length
        {
            get { return (float)(Math.Sqrt(this.GetLengthSq())); }
        }

        /// <summary>
        /// Creates a new Vector3 initialized to 0.
        /// </summary>
        public Vector3()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates a new Vector3 with specific values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new Vector3 with specific string values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(string x, string y, string z)
        {
            float.TryParse(x, out this.x);
            float.TryParse(y, out this.y);
            float.TryParse(z, out this.z);
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance2D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(this.X - otherPosition.X, 2) + Math.Pow(this.Z - otherPosition.Z, 2));
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance3D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(this.X - otherPosition.X, 2) + Math.Pow(this.Y - otherPosition.Y, 2) + Math.Pow(this.Z - otherPosition.Z, 2));
        }

        /// <summary>
        /// Check if the other position is in the circleRadius of this position.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <param name="circleRadius"></param>
        /// <returns></returns>
        public bool IsInCircle(Vector3 otherPosition, float circleRadius)
        {
            float xDistance = this.X - otherPosition.X;
            float zDistance = this.Z - otherPosition.Z;

            return (xDistance * xDistance + zDistance * zDistance) <= circleRadius * circleRadius;
        }

        public Vector3 Normalize()
        {
            float distance = this.Length;

            return new Vector3(this.x / distance, this.y / distance, this.z / distance);
        }

        public float GetLengthSq()
        {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }

        /// <summary>
        /// Clones this Vector3 instance.
        /// </summary>
        /// <returns></returns>
        public Vector3 Clone()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }

        public void Reset()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public bool IsZero()
        {
            return this.x == 0 && this.y == 0 && this.z == 0;
        }

        /// <summary>
        /// Returns a vector3 under string format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Vector3: {0}:{1}:{2}", this.x, this.y, this.z);
        }

        /// <summary>
        /// Add two Vecto3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Substract two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Multiplies two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        /// Multiplies a Vector3 and a float value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Divides two Vector3.
        /// </summary>
        /// <remarks>
        /// Be carefull with the <see cref="DivideByZeroException"/>.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        /// <summary>
        /// Compares two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return Math.Ceiling(a.X - b.X) < 0.01 && Math.Ceiling(a.Y - b.Y) < 0.01 && Math.Ceiling(a.Z - b.Z) < 0.01;
        }

        /// <summary>
        /// Compares two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Get the angle between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AngleBetween(Vector3 a, Vector3 b)
        {
            float deltaX = a.x - b.x;
            float deltaZ = a.z - b.z;

            float angle = (float)((Math.Atan2(deltaZ, deltaX) * 180 / Math.PI) + 90D);

            if (angle < 0)
                angle += 360;

            if (a.X >= b.X)
                angle += 180;
            else
                angle -= 180;

            if (angle >= 360)
                angle -= 360;

            return angle;
        }
    }
}
