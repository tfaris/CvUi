using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Messages
{
    public class PickImageMessage
    {
        public ViewModel.ImageViewModel SelectedImage { get; set; }
        public string OptionName { get; private set; }

        public PickImageMessage() { }

        public PickImageMessage(string optionName)
        {
            OptionName = optionName;
        }
    }
}
