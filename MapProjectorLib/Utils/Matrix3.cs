using System;

namespace MapProjectorLib
{
    public class Matrix3
    {
        readonly double[] _data = new double[9];
        public bool isIdentity;

        public Matrix3()
        {
            Identity();
        }

        public Matrix3(RotationAxis rAxis, double theta)
        {
            switch (rAxis)
            {
                case RotationAxis.X:
                    RotX(theta);
                    break;

                case RotationAxis.Y:
                    RotY(theta);
                    break;

                case RotationAxis.Z:
                    RotZ(theta);
                    break;
            }
        }

        public Matrix3(Matrix3 toCopy)
        {
            Copy(toCopy);
        }

        double this[int i]
        {
            get => _data[i];
            set => _data[i] = value;
        }

        double this[int i, int j]
        {
            get => _data[i + j + j + j];
            set => _data[i + j + j + j] = value;
        }

        public void Copy(Matrix3 toCopy)
        {
            for (var i = 0; i < 9; i++) this[i] = toCopy[i];
            isIdentity = toCopy.isIdentity;
        }

        public void Identity()
        {
            for (var i = 0; i < 9; i++) this[i] = 0.0d;
            for (var i = 0; i < 9; i += 4) this[i] = 1.0d;
            isIdentity = true;
        }

        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            var result = new Matrix3();
            result.Mult(m1, m2);
            return result;
        }

        void Mult(Matrix3 m1, Matrix3 m2)
        {
            if (m1.isIdentity)
            {
                if (m2.isIdentity)
                    Identity();
                else
                    Copy(m2);
            } else if (m2.isIdentity)
            {
                Copy(m1);
            } 
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        var x = 0.0;
                        x += m1[i, 0] * m2[0, j];
                        x += m1[i, 1] * m2[1, j];
                        x += m1[i, 2] * m2[2, j];
                        this[i, j] = x;
                    }
                }
                isIdentity = false; // We could check the indices at this point
            }
        }

        // Set m to be a rotation of theta about the x-axis
        public void RotX(double theta)
        {
            if (theta == 0.0)
            {
                Identity();
            } 
            else
            {
                this[0] = 1.0;
                this[1] = 0.0;
                this[2] = 0.0;
                this[3] = 0.0;
                this[4] = Math.Cos(theta);
                this[5] = -Math.Sin(theta);
                this[6] = 0.0;
                this[7] = Math.Sin(theta);
                this[8] = Math.Cos(theta);
                isIdentity = false;
            }
        }

        // Set m to be a rotation of theta about the y-axis
        public void RotY(double theta)
        {
            if (theta == 0.0)
            {
                Identity();
            } 
            else
            {
                this[0] = Math.Cos(theta);
                this[1] = 0.0;
                this[2] = -Math.Sin(theta);
                this[3] = 0.0;
                this[4] = 1.0;
                this[5] = 0.0;
                this[6] = Math.Sin(theta);
                this[7] = 0.0;
                this[8] = Math.Cos(theta);
                isIdentity = false;
            }
        }

        // Set m to be a rotation of theta about the z-axis
        public void RotZ(double theta)
        {
            if (theta == 0.0)
            {
                Identity();
            } 
            else
            {
                this[0] = Math.Cos(theta);
                this[1] = -Math.Sin(theta);
                this[2] = 0.0;
                this[3] = Math.Sin(theta);
                this[4] = Math.Cos(theta);
                this[5] = 0.0;
                this[6] = 0.0;
                this[7] = 0.0;
                this[8] = 1.0;
                isIdentity = false;
            }
        }

        public (double x, double y, double z) Apply(double x, double y, double z)
        {
            if (isIdentity) return (x, y, z);
            
            return (
                x: this[0] * x + this[1] * y + this[2] * z,
                y: this[3] * x + this[4] * y + this[5] * z,
                z: this[6] * x + this[7] * y + this[8] * z
            );
        }

        public (double phi, double lambda) ApplyLatLong(double phi, double lambda)
        {
            if (isIdentity) return (phi, lambda);

            var x = Math.Cos(lambda) * Math.Cos(phi);
            var y = Math.Sin(lambda) * Math.Cos(phi);
            var z = Math.Sin(phi);
            
            (x, y, z) = Apply(x, y, z);

            return (
                phi: Math.Asin(z),
                lambda: Math.Atan2(y, x)
            );
        }
    }
}