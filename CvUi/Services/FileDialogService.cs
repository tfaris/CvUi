using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvUi.Services
{
    public interface IFileDialogService
    {
        string[] OpenFile();
    }

    class FileDialogService : IFileDialogService
    {
        public string[] OpenFile()
        {
            var dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dlg.FileNames;
            }
            else
            {
                return null;
            }
        }
    }
}
