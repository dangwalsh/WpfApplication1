using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCollector.Model
{
    class MoveStat
    {
        private static int _count;
        private int _number;
        private string _source;
        private string _target;

        public static int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public int Number
        {
            get { return _number; }
        }
        public string Source
        {
            get { return _source; }
        }
        public string Target
        {
            get { return _target; }
        }

        public MoveStat(string s, string t)
        {
            ++_count;
            _number = _count;
            _source = s;
            _target = t;
        }
    }
}
