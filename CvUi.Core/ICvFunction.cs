using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Core
{
    public interface ICvFunction
    {
        string Name { get; }
        IEnumerable<ICvFunctionOption> Options { get; }
        Mat Filter(Mat input);
        Mat Filter(object[] parameters);
    }
}
