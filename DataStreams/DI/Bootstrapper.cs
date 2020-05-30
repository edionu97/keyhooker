using System.Configuration;
using Autofac;
using DataStreams.Core.Resources.Impl;
using DataStreams.Core.Service.Misspeling;
using DataStreams.Core.Service.Misspeling.Impl;
using DataStreams.Core.Service.Words;
using DataStreams.Core.Service.Words.Impl;
using DataStreams.Hooker.Hooker;
using DataStreams.Hooker.Hooker.Impl;
using WebSocketSharp;

namespace DataStreams.Hooker.DI
{
    public class Bootstrapper
    {
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            //register the websocket
            builder
                .RegisterInstance(new WebSocket(ConfigurationManager.AppSettings["serverAddress"]))
                .AsSelf()
                .SingleInstance()
                .OnActivated(x => x.Instance.Connect());

            //register the resource manager
            builder
                .RegisterType<ResourceManager>()
                .AsSelf()
                .SingleInstance();

            //register the misspelling service
            builder
                .Register(ctx =>
                    {
                        var manager = ctx.Resolve<ResourceManager>();
                        return new MisspelingService(manager, ConfigurationManager.AppSettings["language"]);
                    })
                .As<IMisspellingService>()
                .SingleInstance();

            //register word service
            builder
                .RegisterType<WordService>()
                .As<IWordService>()
                .SingleInstance();

            //register the keyboard hooker
            builder
                .RegisterType<KeyboardHooker>()
                .As<IHooker<char>>()
                .SingleInstance();

            return builder.Build();
        }
    }
}
