namespace Arrba.Repositories.Enums
{
    public enum RoomSorter
    {
        /// <summary>
        /// Показывать все объявления
        /// </summary>
        All,

        /// <summary>
        /// Архивные объявления
        /// </summary>
        InArchive,

        /// <summary>
        /// На модерации
        /// </summary>
        OnModiration,
        Active,
    }

    public enum TypeField
    {
        SelectAll = 0,
        SelectCount = 1,
        SelectIds = 2
    }
}
