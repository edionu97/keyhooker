using System.Configuration;
using System.Globalization;
using DataStreams.Core.Resources.Impl;
using DataStreams.Core.Service.Misspeling.Impl;
using DataStreams.Core.Service.Words.Impl;
using DataStreams.Hooker.Hooker.Impl;
using WebSocketSharp;

namespace DataStreams.Hooker
{
    public class MainHooker
    {
        public static void Main(string[] args)
        {
            var ws = new WebSocket(ConfigurationManager.AppSettings["serverAddress"]);
            ws.Connect();

            var manager = new ResourceManager();
            var service = new MisspelingService(manager, ConfigurationManager.AppSettings["language"]);
            var wordService = new WordService(service, ws);

            new KeyboardHooker().Start(wordService.ProcessKey);
        }
    }
}