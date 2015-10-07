namespace Hardly {
    public interface IDataList {
        bool Save(bool lazySave = false);

        bool HasChangedDb {
            get;
        }

        bool Load();
    }
}