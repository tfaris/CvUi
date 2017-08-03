using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CvUi.View
{
    class FunctionOptionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var optionVm = item as ViewModel.CvFunctionOptionViewModel;
            FrameworkElement element = container as FrameworkElement;
            Type optionType = optionVm.OptionType;
            string dataTemplateName = string.Format("{0}Option", optionType.Name);
            DataTemplate template = null;
            try
            {
                template = element.FindResource(dataTemplateName) as DataTemplate;
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
            }
            if (template == null)
            {
                if (optionType == typeof(Mat) || optionType == typeof(InputArray) || optionType == typeof(InputOutputArray))
                {
                    template = element.FindResource("ImageOption") as DataTemplate;
                }
                else if (optionType.IsEnum)
                {
                    template = element.FindResource("EnumOption") as DataTemplate;
                }
                else
                {
                    template = element.FindResource("UnknownOptionType") as DataTemplate;
                }
            }
            return template;
        }
    }
}
