using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public interface IGetFileService
    {
        public bool OpenFileDialog(out string[] paths);
    }
}
