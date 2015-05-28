namespace Business.Common.System.States
{
    public interface IDynamicStateLoader
    {
        string FileName { get; }

        dynamic LoadState();

        bool TryLoadState(out dynamic state);

        void SaveState(dynamic state);

        bool TrySaveState(dynamic state);
    }
}