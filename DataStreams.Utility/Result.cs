using System;
using System.Collections.Generic;

namespace DataStreams.Utility
{
    [Serializable]
    public class Result
    {
        public string Word { get; set; }

        public int Count { get; set; }

        public IList<string> Suggestions { get; set; }

        public void Deconstruct(out int count, out IList<string> suggestions)
        {
            count = Count;
            suggestions = Suggestions;
        }

        public static Result operator ++(Result result)
        {
            ++result.Count;
            return result;
        }
    }
}
