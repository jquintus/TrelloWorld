namespace TrelloWorld.Server.Config
{
    using Ninject;
    using Services;
    using TrelloNet;

    public class TrelloModule
    {
        public static ITrelloController GetRoot()
        {
            var k = CreateKernel();
            return k.Get<ITrelloController>();
        }

        private static StandardKernel CreateKernel()
        {
            var k = new StandardKernel();

            // Settings
            k.Bind<ISettingsLoader<Settings>>().To<SettingsLoader>();
            k.Bind<Settings>().ToMethod( c=>  k.Get<ISettingsLoader<Settings>>().Load()).InSingletonScope();

            // Trello
            k.Bind<ITrello>().ToMethod(c => new Trello(k.Get<Settings>()
                                                        .Key)).InSingletonScope();
            k.Bind<IAsyncTrello>().ToMethod(c => k.Get<ITrello>().Async).InSingletonScope();

            // Services
            k.Bind<ITrelloWorldService>().To<TrelloWorldService>().InSingletonScope();
            k.Bind<IMarkdownService>().To<MarkdownService>().InSingletonScope();

            // Root
            k.Bind<ITrelloController>().To<TrelloControllerService>().InSingletonScope();
            return k;
        }
    }
}