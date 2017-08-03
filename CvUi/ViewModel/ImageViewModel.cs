using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CvUi.ViewModel
{
    public class ImageViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        ImageSource _source;
        System.IO.Stream _imageStream;

        public ImageSource ImageSource
        {
            get
            {
                if (_source == null)
                {
                    _source = BitmapDecoder.Create(_imageStream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
                }
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Stream ImageStream
        {
            get
            {
                return _imageStream;
            }
        }

        public ImageViewModel(System.IO.Stream imageStream)
        {
            // TODO: Stream needs to be disposed somewhere 
            _imageStream = imageStream;
        }
    }
}
