using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataStreams.Core.Service.Misspeling;
using DataStreams.Utility;
using Newtonsoft.Json;
using WebSocketSharp;

namespace DataStreams.Core.Service.Words.Impl
{
    public class WordService : IWordService
    {
        private readonly object _lockObject = new object();
        private readonly IDictionary<string, Result> _misspelledWords = new ConcurrentDictionary<string, Result>();

        private readonly WebSocket _socket;
        private readonly StringBuilder _streamWindow;
        private readonly IMisspellingService _misspellingService;

        public WordService(IMisspellingService misspellingService, WebSocket webSocket)
        {
            _socket = webSocket;
            _misspellingService = misspellingService;
            _streamWindow = new StringBuilder();
        }

        public void ProcessKey(char key)
        {
            lock (_lockObject)
            {
                // if we the backspace key it will be used in order to delete letters from te actual build word
                if (key.Equals((char)Keys.Back))
                {
                    if (_streamWindow.Length <= 0)
                    {
                        return;
                    }

                    _streamWindow.Remove(_streamWindow.Length - 1, 1);
                    return;
                }

                if (IsCombinationKey(key))
                {
                    return;
                }

                //if we have a word build, then we reset the window
                if (IsWordSeparator(key) && _streamWindow.Length > 0)
                {
                    ProcessWord(_streamWindow.ToString().ToLower().Trim());
                    _streamWindow.Clear();
                    return;
                }

                _streamWindow.Append(key);
            }

        }

        private void ProcessWord(string word)
        {
            //check if the word is misspelled
            var (misspelled, suggestions) = _misspellingService.IsMisspelled(word, true);
            if (!misspelled)
            {
                return;
            }

            if (!_misspelledWords.ContainsKey(word))
            {
                _misspelledWords.Add(word, new Result
                {
                    Count = 0,
                    Suggestions = suggestions
                });
            }

            //increment the number of apparitions in dictionary
            _misspelledWords[word]++;

            //send the data through websocket
            _socket?.SendAsync(
                JsonConvert.SerializeObject(_misspelledWords, Formatting.Indented), null);
        }

        private static bool IsCombinationKey(char key)
        {
            var values = Enum.GetValues(typeof(Keys));

            foreach (var value in values)
            {
                if ((char) ((Keys) value) == key)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsWordSeparator(char key)
        {
            return "`~!@#$%^&*()-+=][{}';:/?.>,<\\|\"\n".Contains(key + "") || char.IsWhiteSpace(key);
        }
    }
}
