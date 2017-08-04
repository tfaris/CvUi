using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CvUi.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        int _selectedImageIndex = -1;
        string _cvFuncFilter;
        IEnumerable<CvFunctionViewModel> _funcs;
        IEnumerable<CvFunctionOptionViewModel> _currentFunctionOptions;
        CvFunctionViewModel _selectedFunction;

        public ObservableCollection<ImageViewModel> Images { get; private set; }

        public int SelectedImageIndex
        {
            get
            {
                return _selectedImageIndex;
            }
            set
            {
                Set(ref _selectedImageIndex, value);
            }
        }

        public IEnumerable<CvFunctionViewModel> CvFunctions
        {
            get { return _funcs; }
            private set
            {
                if (_funcs != value)
                {
                    _funcs = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CvFunctionViewModel SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                if (_selectedFunction != value)
                {
                    _selectedFunction = value;
                    RaisePropertyChanged();
                    UpdateCurrentFunctionOptions();
                }
            }
        }

        public IEnumerable<CvFunctionOptionViewModel> CurrentFunctionOptions
        {
            get
            {
                return _currentFunctionOptions;
            }
            set
            {
                if (_currentFunctionOptions != value)
                {
                    _currentFunctionOptions = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CvFunctionFilter
        {
            get { return _cvFuncFilter; }
            set
            {
                if (_cvFuncFilter != value)
                {
                    _cvFuncFilter = value;
                    UpdateFunctions(CvFunctionFilter);
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand OpenCommand { get; private set; }
        public ICommand RunCvFunctionCommand { get; private set; }
        public ICommand CloseImageCommand { get; private set; }

        public MainViewModel()
        {
            Images = new ObservableCollection<ImageViewModel>();
            OpenCommand = new RelayCommand(Open);
            RunCvFunctionCommand = new RelayCommand(RunCvFunction);
            CloseImageCommand = new RelayCommand<ImageViewModel>(CloseImage);
            UpdateFunctions();
        }

        private void CloseImage(ImageViewModel obj)
        {
            Images.Remove(obj);
            obj.Cleanup();
        }

        void UpdateFunctions(string filter = null)
        {
            var factory = new Core.CvFunctionFactory();
            var cvFuncs = factory.FindCvFunctions().Concat(factory.GetCustomCvFunctions());
            if (!string.IsNullOrEmpty(filter))
            {
                cvFuncs = cvFuncs.Where(f => f.Name.ToLower().Contains(filter.ToLower()));
            }
            cvFuncs = cvFuncs
                .OrderBy(f => f.Name)
                .ThenBy(f => f.Options.Count());

            CvFunctions = cvFuncs.Select(f => new CvFunctionViewModel(f));
        }

        void UpdateCurrentFunctionOptions()
        {
            CurrentFunctionOptions = _selectedFunction?.Function.Options
                .Select(opt => new CvFunctionOptionViewModel(opt))
                .ToList();
        }

        void Open()
        {
            var imageProvider = SimpleIoc.Default.GetInstance<Services.IImageProvider>();
            string[] files = SimpleIoc.Default.GetInstance<Services.IFileDialogService>().OpenFile();
            if (files != null)
            {
                foreach (var f in files)
                {
                    var imgStream = imageProvider.OpenImage(f);
                    var vm = new ImageViewModel(imgStream)
                    {
                        Name = System.IO.Path.GetFileName(f)
                    };
                    Images.Add(vm);
                    SelectedImageIndex = Images.Count - 1;
                }
            }
        }

        void RunCvFunction()
        {
            Mat img = null;

            try
            {
                img = SelectedFunction.Function.Filter(CurrentFunctionOptions.Select(opt => opt.Value).ToArray());
            }
            catch (Exception exc)
            {
                var msg = exc.Message;
                if (exc.InnerException != null)
                {
                    msg = exc.InnerException.Message;
                }
                MessengerInstance.Send(new Messages.WarningMessage() { Message = msg });
            }

            if (img != null)
            {
                var vm = new ImageViewModel(img.ToMemoryStream())
                {
                    Name = SelectedFunction.Name
                };
                Images.Add(vm);
                img.Dispose();
                SelectedImageIndex = Images.Count - 1;
            }
        }
    }
}
