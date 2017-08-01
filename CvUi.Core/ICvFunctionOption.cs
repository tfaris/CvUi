using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Core
{
    public interface ICvFunctionOption
    {
        Type CreatesType { get; }
        string OptionName { get; }
        bool IsOptional { get; }
    }
}
