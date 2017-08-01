using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using CvUi.Core;
using System.Collections.Generic;

namespace CvUi.Tests
{
    [TestClass]
    public class Core
    {

        [TestMethod]
        public void TestFunctionWithMatExprReturnType()
        {
            var img = new Mat("Images/Lenna.png");
            Assert.AreEqual(typeof(Mat), GetFunctionWithName("Abs").Filter(img).GetType(),
                "Function with MatExpr return type did not cast correctly to Mat.");
        }

        [TestMethod]
        public void TestFunctionWithMissingOptionalParameters()
        {
            var img = new Mat("Images/Lenna.png");
            var func = GetFunctionWithName("Add");
            Assert.AreNotEqual(0, func.Options.Where(opt => opt.IsOptional).Count(), "Function that was expected to have optional parameters did not have any.");

            Mat output = func.Filter(new object[] { img, img, new Mat() });
            Assert.IsNotNull(output, "Function with optional parameters did not return anything.");
        }

        [TestMethod]
        public void TestFunctionWithOutParameter()
        {
            var img = new Mat("Images/Lenna.png");
            // The second parameter of the FindContours method is an out parameter.
            var func = GetFunctionWithName("FindContours");
            Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
            var binary = img.Threshold(50, 255, ThresholdTypes.Binary);
            object[] parameters = new object[] {
                binary, null, null, RetrievalModes.CComp, ContourApproximationModes.ApproxNone };
            func.Filter(parameters);
            Assert.IsNotNull(parameters[1]);
        }

        ICvFunction GetFunctionWithName(string name)
        {
            return GetFunctionsWithName(name).First();
        }

        IEnumerable<ICvFunction> GetFunctionsWithName(string name)
        {
            return new CvFunctionFactory().FindCvFunctions().Where(f => f.Name == name);
        }
    }
}
