using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Core
{
    public class CvFunctionFactory
    {
        public IEnumerable<ICvFunction> FindCvFunctions()
        {
            var klass = typeof(OpenCvSharp.Cv2);
            var pubStatMethods = klass.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            List<ICvFunction> functions = new List<ICvFunction>();

            var imgProcArgIndicatorTypes = new Type[] {
                typeof(OpenCvSharp.Mat),
                typeof(OpenCvSharp.InputArray),
                typeof(OpenCvSharp.OutputArray),
                typeof(OpenCvSharp.InputOutputArray)
            };

            List<System.Reflection.MethodInfo> imageProcessingMethods = new List<System.Reflection.MethodInfo>();
            List<Type> allParameterTypes = new List<Type>();
            foreach (var methodInfo in pubStatMethods)
            {
                var methodImgProcParams = methodInfo.GetParameters()
                    .Select(pi => pi.ParameterType).Union(new Type[] { methodInfo.ReturnType })
                    .Intersect(imgProcArgIndicatorTypes);
                if (methodImgProcParams.Count() > 0)
                {
                    imageProcessingMethods.Add(methodInfo);

                    List<ICvFunctionOption> options = new List<ICvFunctionOption>();
                    foreach(var pi in methodInfo.GetParameters())
                    {
                        Type paramType = pi.ParameterType;
                        while (paramType.IsGenericType)
                        {
                            paramType = paramType.GetGenericArguments()[0];
                        }
                        if (!allParameterTypes.Contains(paramType))
                        {
                            allParameterTypes.Add(paramType);
                        }
                        options.Add(new CvFunctionOption(paramType, pi.Name, pi.IsOptional));
                    }
                    functions.Add(new CvFunction(methodInfo, options));
                }
            }

            return functions;
        }

        public IEnumerable<ICvFunction> GetCustomCvFunctions()
        {
            return new ICvFunction[]
            {
                new CvFunction(GrayScale, "Grayscale [CvUi]", new ICvFunctionOption[] { new CvFunctionOption(typeof(Mat), "src", false) })
            };
        }

        Mat GrayScale(object[] args)
        {
            var output = new Mat();
            Cv2.CvtColor(args[0] as Mat, output, ColorConversionCodes.BGR2GRAY);
            return output;
        }
    }
}
