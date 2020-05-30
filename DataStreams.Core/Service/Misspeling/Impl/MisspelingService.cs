using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataStreams.Core.Resources;
using NHunspell;

namespace DataStreams.Core.Service.Misspeling.Impl
{
    public class MisspelingService : IMisspellingService
    {
        private readonly Hunspell _grammarEngine;
        private readonly TextInfo _textInfo;

        public MisspelingService(IResourceManager manager, string language)
        {
            //get the language information
            var lang = manager
                .GetLanguages()
                .ToList()
                .FirstOrDefault(x => x.Language.Equals(language));

            //if the language cannot be found, than throw an error
            if (lang == null)
            {
                throw new Exception($"Language {language} not found");
            }

            //create the engine
            _grammarEngine = new Hunspell(lang.FileAff, lang.FileDict);
            _textInfo = new CultureInfo(lang.Language, false).TextInfo;
        }

        /// <summary>
        /// This method is used in order to check if a word is misspelled
        /// </summary>
        /// <param name="word">the word we want to check</param>
        /// <param name="getSuggestions">if true, then a list of suggestions will be returned, alongside the boolean value</param>
        /// <returns>
        ///     a pair of elements
        ///     where item1 represents a boolean (checking if the word is misspelled or not)
        ///     and   item2 represents a list of suggestions, that may be similar with the word you've entered
        /// </returns>
        public Tuple<bool, IList<string>> IsMisspelled(string word, bool getSuggestions = false)
        {
            var result = IsMisspelled(word);
            if (result)
            {
                return new Tuple<bool, IList<string>>(false, new List<string>());
            }

            return !getSuggestions
                ? new Tuple<bool, IList<string>>(true, new List<string>())
                : new Tuple<bool, IList<string>>(true, _grammarEngine.Suggest(word));
        }

        private bool IsMisspelled(string word)
        {
            return _grammarEngine.Spell(word)
                   || _grammarEngine.Spell(_textInfo.ToLower(word))
                   || _grammarEngine.Spell(_textInfo.ToUpper(word))
                   || _grammarEngine.Spell(_textInfo.ToTitleCase(word));
        }
    }
}
