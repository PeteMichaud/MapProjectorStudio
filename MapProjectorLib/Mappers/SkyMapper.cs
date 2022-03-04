using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Mappers
{
    class SkyMapper : ImageMapper
    {
        readonly Transform _transform;
        readonly TransformParams _tParams;
        readonly Rgb24 _skyColor;
        readonly Rgb24 _bgColor;
        
        public SkyMapper(Transform transform, TransformParams tParams, Rgb24 sky, Rgb24 bg)
        {
            _transform = transform;
            _tParams = tParams;
            _skyColor = sky;
            _bgColor = bg;
        }

        public override void InitY(double y)
        {
            _transform.SetY(y);
        }

        public override Rgb24 Map(double x, double y)
        {
            double x0 = 0.0, y0 = 0.0, z0 = 0.0;
            double phi = 0.0, lambda = 0.0;
            if (_transform.Project(_tParams, x, y, ref x0, ref y0, ref z0, ref phi, ref lambda))
            {
                return _bgColor;
            }
            else
            {
                return _skyColor;
            }
        }

        public override double Scale(int width, int height)
        {
            return _transform.BasicScale(width, height) / _tParams.scale;
        }
    }
}
