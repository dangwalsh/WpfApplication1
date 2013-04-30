using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FamilyCollector.Model
{
    static class Results
    {
        private static BindingList<MoveStat> _moved = new BindingList<MoveStat>();
        private static BindingList<SkipStat> _skipped = new BindingList<SkipStat>();
        private static BindingList<ErrorStat> _errors = new BindingList<ErrorStat>();
       
        #region Public list properties for use by windows form datagridviews
        public static BindingList<MoveStat> MovedFiles
        {
            get { return Results._moved; }
            set { _moved = value; }
        }
        public static BindingList<SkipStat> SkippedFiles
        {
            get { return Results._skipped; }
            set { _skipped = value; }
        }
        public static BindingList<ErrorStat> Errors
        {
            get { return Results._errors; }
            set { _errors = value; }
        }
        #endregion

        #region Methods
        public static void UpMoved(string source, string target)
        {
            MoveStat ms = new MoveStat(source, target);
            Results._moved.Add(ms);
        }

        public static void UpSkipped(string source)
        {
            SkipStat ss = new SkipStat(source);
            Results._skipped.Add(ss);
        }

        public static void LogError(string e)
        {
            ErrorStat es = new ErrorStat(e);
            Results._errors.Add(es);
        }
        #endregion

    }
}
