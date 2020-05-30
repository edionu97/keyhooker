using System;
using System.Collections.Generic;

namespace DataStreams.Core.Service.Misspeling
{
    public interface IMisspellingService
    {
        Tuple<bool, IList<string>> IsMisspelled(string word, bool getSuggestions = false);
    }
}
