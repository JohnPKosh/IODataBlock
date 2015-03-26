namespace Business.Common.System.States
{
    public interface IStateLoader
    {
        T LoadState<T>();
        bool TryLoadState<T>(out T state);

        void SaveState<T>(T state);
        bool TrySaveState<T>(T state);
    }
}
