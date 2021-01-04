using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletLib.Repository.File
{
    /// <summary>
    /// Wallet file saver, that use<see cref="Newtonsoft.Json.JsonConvert"/> 
    /// for serializing and deserializing wallet data
    /// </summary>
    class JsonFileWalletSaver : IFileWalletSaver
    {
        public Dictionary<string, int> LoadFromFile(string filePath) =>
            JsonConvert.DeserializeObject<Dictionary<string, int>>(System.IO.File.ReadAllText(filePath));

        public void SaveToFile(string filePath, Dictionary<string, int> userCash) =>
            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(userCash));
    }

}