using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace CvUi.Services
{
    public interface IImageProvider
    {
        Stream OpenImage(string filepath);
        ImageSource Convert(Mat input);
    }

    class ImageProvider : IImageProvider
    {
        public ImageSource Convert(Mat input)
        {
            var stream = input.ToMemoryStream();
            return BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
        }

        public System.IO.Stream OpenImage(string imagePath)
        {
            return File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
