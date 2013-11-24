using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fukami.Helpers
{
    public class Wrap<T>
    {
        public string ValueSource { get; set; }

        private bool _isSet;
        public bool IsSet
        {
            get { return _isSet; }
        }

        private T _value;

        public T Value
        {
            get { return _value; }
            set 
            { 
                _value = value;
                _isSet = true;
            }
        }
        
    }
}
