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
    class SearchManager : IFileManage, INotifyPropertyChanged
    {
        private SearchBase _search;
        private string _currentPath;
        private View.MainWindow _delegate;

        #region Public properties
        public string CurrentPath
        {
            get { return _currentPath; }
            set
            {
                _currentPath = value;
                OnPropertyChanged("CurrentPath");
            }
        }
        public View.MainWindow Delegate
        {
            get { return _delegate; }
            set { _delegate = value; }
        }
        #endregion

        #region Constructors
        public SearchManager(View.MainWindow window)
        {
            _search = new SearchRecurse(this); // NOTE: change this type to instantiate a different search algorithm
            _delegate = window;
            
        }

        #endregion

        #region IFileManage interface methods
        public void MoveFile(System.IO.FileInfo file)
        {
            System.IO.FileInfo newfile = null;
            try
            {
                string dest = Model.Settings.TargetPath + @"\" + file.Name;
                newfile = file.CopyTo(dest);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Model.Results.LogError(e.Message);
            }

            if (newfile != null)
            {
                Model.Results.UpMoved(file.FullName, newfile.FullName);
                DeleteFile(file);
            }
            else
            {
                Model.Results.LogError("Copy failed for unkown reasons.");
                SkipFile(file);
            }
        }

        public void DeleteFile(System.IO.FileInfo file)
        {
            try
            {
                file.Delete();
            }
            catch (UnauthorizedAccessException e)
            {
                Model.Results.LogError(e.Message);
            }
        }

        public void SkipFile(System.IO.FileInfo file)
        {
            Model.Results.UpSkipped(file.FullName);
        }
        #endregion

        #region INotifyPropertyChanged methods
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Main program loop
        public void IterateProjects(System.IO.DirectoryInfo root, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            int i = 0;
            int max = 0;
            System.IO.DirectoryInfo[] subDirs = null;
            try
            {
                subDirs = root.GetDirectories();
                max = subDirs.Count() / 100;
                //this.Delegate.Progress.Maximum = subDirs.Count();
            }
            catch (Exception e)
            {
                Model.Results.LogError(e.Message);
            }
            
            // this will iterate through each job folder
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                ++i;
                if ((i % max == 0) && (null != backgroundWorker))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        // Return without doing any more work.
                        return;
                    }
                    if (backgroundWorker.WorkerReportsProgress)
                    {
                        backgroundWorker.ReportProgress(i / max);
                    }
                }
                //Application.DoEvents();
                //this.Delegate.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                //        new View.MainWindow.NextFileDelegate(this.Delegate.timerSrch_Tick));
                //if (View.MainWindow.ExitFlag) return;
                //this.Delegate.Progress.PerformStep();
                //this.Delegate.CurPath.Content = dirInfo.FullName;
                Model.Settings.SourcePath = dirInfo.FullName;
                string familyPath = dirInfo.FullName + @"\3_Process Families"; // TODO: set this name through the form
                System.IO.DirectoryInfo familyDir = new System.IO.DirectoryInfo(familyPath);

                if (familyDir.Exists)
                {
                    try
                    {
                        _search.WalkDirectoryTree(familyDir);
                    }
                    catch (Exception e)
                    {
                        Model.Results.LogError(e.Message);
                    }
                }
                
            }
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(100);
            }
        }
        #endregion
    }
}
