using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyCollector.Interfaces;

namespace FamilyCollector.Controller
{
    class SearchStack : SearchBase
    {
        #region Constructors
        public SearchStack(IFileManage d) : base(d)
        {

        }
        #endregion

        public override void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            throw new NotImplementedException();
        }
    }
}
