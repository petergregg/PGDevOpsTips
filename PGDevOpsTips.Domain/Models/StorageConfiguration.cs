namespace PGDevOpsTips.Domain.Models
{
    /// <summary>
    /// Used to strongly type the "StorageConfiguration" appsettings section
    /// </summary>
    public class StorageConfiguration
    {
        /// <summary>
        /// The name of the blob container in Azure Storage where HTML blobs for Contents is stored.
        /// </summary>
        public string ContentBlobsContainer { get; set; }

        /// <summary>
        /// The connection string for Azure Storage.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of a table in Azure Storage where Content entity are stored.
        /// </summary>
        public string ContentsTable { get; set; }

        /// <summary>
        /// The name of a table in Azure Storage where Shortcut entities are stored.
        /// </summary>
        public string ShortcutsTable { get; set; }

        /// <summary>
        /// The name of the blob container in Azure Storage where wallpaper images are stored.
        /// </summary>
        public string WallpaperBlobsContainer { get; set; }
    }
}
