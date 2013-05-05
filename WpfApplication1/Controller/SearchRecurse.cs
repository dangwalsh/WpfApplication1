using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;

using FamilyCollector.Interfaces;

namespace FamilyCollector.Controller
{
    class SearchRecurse : SearchBase
    {
        #region Constructors
        public SearchRecurse(IFileManage d) : base(d)
        {

        }
        #endregion

        #region Search class method
        // this is the program...
        // the entry point should be no higher than the folder holding families
        // otherwise you may experience stack overflow
        public override void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            //Application.DoEvents();
            //if (View.MainWindow.ExitFlag) return;
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // first process all of the files directly in this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Model.Results.LogError(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Model.Results.LogError(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    //Application.DoEvents();
                    //if (View.MainWindow.ExitFlag) return;
                    if(fi.Extension == ".rfa")
                        this.Delegate.MoveFile(fi);
                }

                // now get all of the subdirectories under this directory
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    //Application.DoEvents();
                    //if (View.MainWindow.ExitFlag) return;
                    // recursive call to self
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
        #endregion
    }
}
