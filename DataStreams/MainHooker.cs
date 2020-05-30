using System;
using Autofac;
using DataStreams.Core.Service.Words;
using DataStreams.Hooker.DI;
using DataStreams.Hooker.Hooker;

namespace DataStreams.Hooker
{
    public class MainHooker
    {
        public static void Main(string[] args)
        {
            var container = Bootstrapper.Bootstrap();

            Console.WriteLine("Keyboard monitoring started...\nClose this window for stopping the process");
            container
                .Resolve<IHooker<char>>()
                .Start(container.Resolve<IWordService>().ProcessKey);
        }
    }
}