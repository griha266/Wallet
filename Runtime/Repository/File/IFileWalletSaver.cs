using System.Collections.Generic;

namespace WalletLib.Repository.File
{
    /// <summary>
    /// Base interface for saving and loading from file wallet data
    /// </summary>
    interface IFileWalletSaver
    {
        /// <summary>
        /// Load file from selected path and deserialized it to wallet data
        /// </summary>
        /// <param name="filePath">Selected file path</param>
        /// <returns>Deserialized wallet data</returns>
        Dictionary<string, int> LoadFromFile(string filePath);
        /// <summary>
        /// Serialize and save wallet data to selected file path
        /// </summary>
        /// <param name="filePath">Selected file path</param>
        /// <param name="userCash">Wallet data to save</param>
        void SaveToFile(string filePath, Dictionary<string, int> userCash);
    }

}