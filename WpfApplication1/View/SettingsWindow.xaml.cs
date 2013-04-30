using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

using FamilyCollector.Model;

namespace FamilyCollector.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private System.Windows.Forms.FolderBrowserDialog tgtBrowserDlg;
        private System.Windows.Forms.FolderBrowserDialog srcBrowserDlg;
        string _oldTarget;
        string _oldRoot;

        public SettingsWindow()
        {
            _oldTarget = Settings.TargetPath;
            _oldRoot = Settings.RootPath;
            
            InitializeComponent();

            this.txtTarget.Text = Settings.TargetPath;
            this.txtRoot.Text = Settings.RootPath;
        }

        private void btnTgtBrowse_Click(object sender, RoutedEventArgs e)
        {
            this.tgtBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.tgtBrowserDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            System.Windows.Forms.DialogResult result = this.tgtBrowserDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings.TargetPath = this.tgtBrowserDlg.SelectedPath;
                this.txtTarget.Text = Settings.TargetPath;
            }
        }

        private void btnSrcBrowse_Click(object sender, RoutedEventArgs e)
        {
            this.srcBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.srcBrowserDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            System.Windows.Forms.DialogResult result = this.srcBrowserDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings.RootPath = this.srcBrowserDlg.SelectedPath;
                this.txtRoot.Text = Settings.RootPath;
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.TargetPath = _oldTarget;
            Settings.RootPath = _oldRoot;
            this.Close();
        }
    }
}
