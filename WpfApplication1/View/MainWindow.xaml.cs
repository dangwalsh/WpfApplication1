using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;

using FamilyCollector.Controller;
using FamilyCollector.Model;


namespace FamilyCollector.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timerSrch;
        //static bool _exitFlag;
        SearchManager _manager;
        ProgressBar _progress;
        Label _curPath;
        TimeSpan _seconds;
        string _time = "00:00:00";
        BackgroundWorker backgroundWorker;

        #region Public properties

        public ProgressBar Progress // needed for updates via SearchManager
        {
            get { return _progress; }
            set { _progress = value; }
        }
        public Label CurPath // needed for updates via SearchManager
        {
            get { return _curPath; }
            set { _curPath = value; }
        }

        public delegate void NextFileDelegate();
        #endregion



        public MainWindow()
        {
            InitializeComponent();

            _progress = this.progCur;
            _curPath = this.lblCur;
            _manager = new SearchManager(this);
            _timerSrch = new DispatcherTimer();

            this.btnCancel.Visibility = Visibility.Hidden;
            this.btnReset.Visibility = Visibility.Hidden;
            this.progCur.Visibility = Visibility.Hidden;
            this.progCur.Minimum = 1;
            this.progCur.Maximum = 100;
            this.lblTgt.Content = Settings.TargetPath;
            this.lblTime.Content = _time;

            this._timerSrch.Interval = new TimeSpan(0, 0, 1);
            //this._timerSrch.Tick += new EventHandler(counterOne_Tick);
            //this._timerSrch.Tick += timerSrch_Tick;
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.IO.DirectoryInfo root = (System.IO.DirectoryInfo)e.Argument;
            _manager.IterateProjects(root, backgroundWorker);
            
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }           
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnCancel.Visibility = Visibility.Hidden;
            this.progCur.Visibility = Visibility.Hidden;
            this.btnSearch.Visibility = Visibility.Visible;
            this.btnReset.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate()
                {
                    this.gridMoved.ItemsSource = Results.MovedFiles;
                    this.gridMoved.ItemsSource = Results.MovedFiles;
                });
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progCur.Value = e.ProgressPercentage;
            Dispatcher.BeginInvoke(new Action(() => { this.lblCur.Content = Settings.SourcePath; }));

        }
       
        public void timerSrch_Tick()
        {
            _seconds += _timerSrch.Interval;
            _time = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                        (_seconds.Hours),
                                        (_seconds.Minutes),
                                        (_seconds.Seconds));
            this.lblTime.Content = _time;
            this._timerSrch.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, 
                new NextFileDelegate(this.timerSrch_Tick));
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.RootPath == null || Settings.TargetPath == null)
            {
                MessageBox.Show("Please choose locations for both the Root and Target of the search", "Error");
            }
            else
            {
                System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(Settings.RootPath);
                if (root.Exists)
                {
                    this.progCur.Value = 1;
                    this.btnSearch.Visibility = Visibility.Hidden;
                    this.progCur.Visibility = Visibility.Visible;
                    this.btnCancel.Visibility = Visibility.Visible;

                    backgroundWorker.RunWorkerAsync(root);                    
                }
                else
                {
                    string error = "Root path does not exist";
                    Results.LogError(error);
                    throw new Exception(error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {            
            backgroundWorker.CancelAsync();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _seconds = TimeSpan.Zero;
            _time = "00:00:00";
            this.lblTime.Content = _time;
            this.btnReset.Visibility = Visibility.Hidden;
            this.progCur.Visibility = Visibility.Hidden;
            this.lblCur.Content = Settings.RootPath;
            Results.MovedFiles.Clear();
            Results.SkippedFiles.Clear();
            Results.Errors.Clear();
            ErrorStat.Count = 0;
            MoveStat.Count = 0;
            SkipStat.Count = 0; 
        }

        //private void timerSrch_Tick(Object obj, EventArgs events)
        //{
        //    _seconds += this._timerSrch.Interval;
        //    _time = string.Format("{0:D2}:{1:D2}:{2:D2}",
        //                                _seconds.Hours,
        //                                _seconds.Minutes,
        //                                _seconds.Seconds);
        //    this.lblTime.Content = _time;
        //}

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: commit data to persistent store
            this.Close();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            SettingsWindow setWindow = new SettingsWindow();
            setWindow.ShowDialog();
            this.lblTgt.Content = Settings.TargetPath;
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}
