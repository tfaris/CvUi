using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CvUi.Messages;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.CommandWpf;

namespace CvUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<Messages.PickImageMessage>(this, HandlePickImageMessage);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<Messages.WarningMessage>(this, HandleWarningMessage);
        }

        private void HandleWarningMessage(WarningMessage obj)
        {
            MessageBox.Show(obj.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void HandlePickImageMessage(PickImageMessage obj)
        {
            var mainVm = SimpleIoc.Default.GetInstance<ViewModel.MainViewModel>();
            var pickerVm = new ViewModel.ImagePickerViewModel(mainVm);
            var view = new View.ImagePickerView() { DataContext = pickerVm };
            Window win = new Window()
            {
                Title = string.Format("Pick image for {0}", obj.OptionName),
                Content = view,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 500,
                Height = 500
            };
            pickerVm.OkCommand = new RelayCommand(() =>
            {
                win.DialogResult = true;
                win.Close();
            });
            pickerVm.CancelCommand = new RelayCommand(() =>
            {
                win.DialogResult = false;
                win.Close();
            });
            var result = win.ShowDialog();
            if (result.HasValue && result.Value)
            {
                obj.SelectedImage = pickerVm.SelectedImage;
            }
        }
    }
}
