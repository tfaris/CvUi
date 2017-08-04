using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CvUi.ViewModel
{
    public class ImagePickerViewModel : ViewModelBase
    {
        IEnumerable<ImageViewModel> _thumbs;
        ImageViewModel _selected;

        public IEnumerable<ImageViewModel> ImageThumbs
        {
            get
            {
                return _thumbs;
            }
            set
            {
                Set(ref _thumbs, value);
            }
        }

        public ImageViewModel SelectedImage
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ImagePickerViewModel(MainViewModel mainView)
        {
            _thumbs = mainView.Images;
            SelectedImage = mainView.Images[mainView.SelectedImageIndex];
        }
    }
}
