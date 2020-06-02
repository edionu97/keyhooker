using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataStreams.Utility;

namespace DataStreams.Core.Service.Words.Impl
{
    public partial class WordService
    {
        private static bool IsCombinationKey(char key)
        {
            var values = Enum.GetValues(typeof(Keys));
            return values
                .Cast<object>()
                .All(value => (char)((Keys)value) != key);
        }

        private static bool IsWordSeparator(char key)
        {
            return "`~!@#$%^&*()-+=][{}';:/?.>,<\\|\"\n".Contains(key + "") || char.IsWhiteSpace(key);
        }

        private void Reduce(IDictionary<string, Result> dictionary)
        {
            if (dictionary.Count <= _maxWords)
            {
                return;
            }

            //keep max _maxWords element into dictionary
            dictionary.Remove(
                dictionary
                    .Values
                    .OrderBy(x => x.CreatedAt)
                    .First()
                    .Word);
        }

    }
}
