using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyCollector.Interfaces;

namespace FamilyCollector.Controller
{
    abstract class SearchBase
    {  
        private IFileManage _delegate;

        #region Delegate property to allow Manager and Search classes to communicate
        public IFileManage Delegate
        {
            get { return _delegate; }
        }
        #endregion

        #region Constructors
        public SearchBase(IFileManage d)
        {
            _delegate = d;
        }
        #endregion

        #region Abstract methods to force override
        public abstract void WalkDirectoryTree(System.IO.DirectoryInfo root);
        #endregion
    }
}
