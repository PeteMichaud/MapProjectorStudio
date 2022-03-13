using VectSharp;
using VectSharp.SVG;

namespace MapProjectorLib
{
    public class SvgImageSaver
    {
        readonly string FileExt;
        readonly string FileNameWithoutExt;
        readonly string FileName;

        public SvgImageSaver(string fileName)
        {
            var etxInx = fileName.LastIndexOf('.');
            FileNameWithoutExt = fileName.Substring(0, etxInx) + "_Widgets";
            FileExt = "svg";
            FileName = $"{FileNameWithoutExt}.{FileExt}";
        }

        public SvgImageSaver(string fileName, int seriesNumber)
            : this(fileName)
        {
            FileName = string.Format(
                "{0}{1,4:0000}.{2}",
                FileNameWithoutExt,
                seriesNumber,
                FileExt
            );
        }

        public void Save(Page image)
        {
            image.SaveAsSVG(FileName);
        }
    }
}
