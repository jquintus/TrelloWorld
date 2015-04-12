namespace TrelloWorld.Server.Services
{
    public interface ISettingsLoader<T>
    {
        T Load();
    }
}