using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Helpers
{
    public struct MultipleSelectorStruct
    {
        public MultipleSelectorStruct(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
        //public bool Selectable { get; set; } //for future development: is to allow item(in dropdown list) to be selected or not
    }
}
