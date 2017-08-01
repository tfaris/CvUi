using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Core
{
    class CvFunctionOption : ICvFunctionOption
    {
        public Type CreatesType { get; private set; }
        public string OptionName { get; private set; }
        public bool IsOptional { get; private set; }

        public CvFunctionOption(Type createsType, string optionName, bool isOptional)
        {
            CreatesType = createsType;
            OptionName = optionName;
            IsOptional = isOptional;
        }

        public virtual object CreateOption()
        {
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is CvFunctionOption)
            {
                return Equals((CvFunctionOption)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(CvFunctionOption fo)
        {
            return fo.CreatesType.Equals(CreatesType) && fo.OptionName == OptionName;
        }

        public override int GetHashCode()
        {
            return CreatesType.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("FilterOption({0}, {1})", CreatesType.Name, OptionName);
        }
    }
}
