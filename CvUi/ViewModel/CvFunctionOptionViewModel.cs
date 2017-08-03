using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CvUi.ViewModel
{
    public class CvFunctionOptionViewModel : ViewModelBase
    {
        object _value;
        object[] _optionChoices;

        public Core.ICvFunctionOption Option { get; private set; }

        public string OptionName { get { return Option.OptionName; } }

        public object[] OptionChoices { get => _optionChoices; set => Set(ref _optionChoices, value); }

        public object Value
        {
            get => _value; set
            {
                if (OptionType == typeof(System.Double) || OptionType == typeof(System.Int32))
                {
                    // TODO: More laziness. There should be validation here.
                    value = Convert.ChangeType(value, OptionType);
                }
                Set(ref _value, value);
            }
        }

        public Type OptionType { get { return Option.CreatesType; } }

        public ICommand PickMatrixCommand { get; private set; }

        public CvFunctionOptionViewModel(Core.ICvFunctionOption option)
        {
            Option = option;
            PickMatrixCommand = new RelayCommand(PickMatrix);
            Setup();
        }

        private void Setup()
        {
            if (Option.CreatesType.IsEnum)
            {
                OptionChoices = Enum.GetValues(Option.CreatesType).Cast<object>().ToArray();
            }
        }

        void PickMatrix()
        {
            var msg = new Messages.PickImageMessage(Option.OptionName);
            MessengerInstance.Send(msg);
            if (msg.SelectedImage != null)
            {
                var stream = msg.SelectedImage.ImageStream;
                var mat = OpenCvSharp.Mat.FromStream(stream, OpenCvSharp.ImreadModes.AnyColor);
                Value = mat;
            }
        }
    }
}
