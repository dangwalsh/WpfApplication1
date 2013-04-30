using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCollector.Model
{
    class ErrorStat
    {
        private static int _count;
        private int _number;
        private string _error;

        public static int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public int Number
        {
            get { return _number; }
        }
        public string Error
        {
            get { return _error; }
        }

        public ErrorStat(string e)
        {
            ++_count;
            _number = _count; 
            _error = e;
        }
    }
}
