using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCollector.Model
{
    static class Settings
    {
        private static string _rootPath = @"J:";
        private static string _targetPath = @"L:\4_Revit\Family Inbox";
        private static string _sourcePath;
        private static decimal _maxSearchTm;
        private static decimal _maxCopyTm;

        #region Public properties for windows form
        public static string RootPath
        {
            get { return _rootPath; }
            set { _rootPath = value; }
        }
        public static string TargetPath
        {
            get { return _targetPath; }
            set { _targetPath = value; }
        }
        public static string SourcePath
        {
            get { return _sourcePath; }
            set { _sourcePath = value; }
        }
        public static decimal MaxSearchTm
        {
            get { return _maxSearchTm; }
            set { _maxSearchTm = value; }
        }
        public static decimal MaxCopyTm
        {
            get { return _maxCopyTm; }
            set { _maxCopyTm = value; }
        }
        #endregion
    }
}
