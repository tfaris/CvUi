using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.ViewModel
{
    public class CvFunctionViewModel
    {
        public Core.ICvFunction Function { get; private set; }

        public string Name
        {
            get { return Function.Name; }
        }

        public string ParametersText
        {
            get
            {
                return string.Join(", ", Function.Options.Select(o => o.OptionName));
            }
        }

        public CvFunctionViewModel(Core.ICvFunction cvFunction)
        {
            Function = cvFunction;
        }
    }
}
