namespace Business.Common.System.States
{
    public interface IDynamicLoader
    {
        dynamic Load();

        bool TryLoad(out dynamic value);

        void Save(dynamic value);

        bool TrySave(dynamic value);
    }
}