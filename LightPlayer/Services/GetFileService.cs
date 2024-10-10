using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LightPlayer.Services
{
    public class GetFileService : IGetFileService
    {
        public bool OpenFileDialog(out string[] paths)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = ".mp3|*.mp3|.flac|*.flac",
                Multiselect = true,
                Title = "Select"
            };
            if (ofd.ShowDialog() == true)
            {
                paths = ofd.FileNames;
                return true;
            }
            else
            {
                paths = Array.Empty<string>();
                return false;
            }
        }
    }
}
