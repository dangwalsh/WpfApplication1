using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCollector.Model
{
    class DirStat
    {
        private System.IO.DirectoryInfo _dir;

        public System.IO.DirectoryInfo Dir
        {
            get { return _dir; }
        }

        public DirStat(System.IO.DirectoryInfo d)
        {
            _dir = d;
        }
    }
}
