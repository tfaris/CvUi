using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OpenCvSharp;
using System.ComponentModel;
using System.Globalization;

namespace CvUi.Core
{
    class CvFunction : ICvFunction
    {
        Func<object[], Mat> _func;

        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<ICvFunctionOption> Options
        {
            get;
            private set;
        }

        public CvFunction(MethodInfo methodInfo, IEnumerable<ICvFunctionOption> filterOptions)
        {
            _func = (parameters) =>
            {
                return FilterUsingReflection(methodInfo, parameters);
            };
            Name = methodInfo.Name;
            Options = filterOptions;
        }

        public CvFunction(Func<object[], Mat> filterFunction, string name, IEnumerable<ICvFunctionOption> filterOptions)
        {
            _func = filterFunction;
            Name = name;
            Options = filterOptions;
        }

        public Mat Filter(Mat input)
        {
            return _func.Invoke(new object[] { input });
        }

        public Mat Filter(object[] options)
        {
            return _func.Invoke(options);
        }

        private Mat FilterUsingReflection(MethodInfo methodInfo, object[] parameters)
        {
            var optList = Options.ToList();
            object[] allParams = new object[optList.Count()];
            
            int maybeOutputParameter = -1;
            if (parameters != null)
            {
                for (int i = 0; i < optList.Count; i++)
                {
                    var opt = optList[i];
                    if (i < parameters.Length)
                    {
                        if (opt.CreatesType.IsAssignableFrom(typeof(OutputArray)))
                        {
                            maybeOutputParameter = i;

                            if (parameters[i] == null)
                            {
                                // Convenience if output matrix was not specified. It's just easier
                                // to create it here.
                                parameters[i] = new Mat();
                            }
                        }
                        if (parameters[i] != null)
                        {
                            allParams[i] = TypeHelpers.Convert(parameters[i], opt.CreatesType);
                        }
                    }
                    else if (opt.IsOptional)
                    {
                        allParams[i] = Type.Missing;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Option {0} is required.", opt.OptionName));
                    }
                }
            }

            object result = methodInfo.Invoke(null, allParams);

            for (int i=0; i < allParams.Length; i++)
            {
                // Given parameter list length is long enough, parameter was passed as null, 
                // but now has a value after calling the function. Must be an "out" parameter.
                if (i < parameters.Length && parameters[i] == null && allParams[i] != null)
                {
                    parameters[i] = allParams[i];
                }
            }

            Mat matResult = null;
            try
            {
                matResult = (Mat) TypeHelpers.Convert(result, typeof(Mat));
            }
            catch (InvalidOperationException)
            {
                if (maybeOutputParameter >= 0)
                {
                    try
                    {
                        matResult = (Mat)TypeHelpers.Convert(parameters[maybeOutputParameter], typeof(Mat));
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
            }
            return matResult;
        }

        public override string ToString()
        {
            return string.Format("FilterFunction(Name={0}, Options={1})", Name, string.Join(",", Options.Select(o => o.OptionName)));
        }
    }
}
