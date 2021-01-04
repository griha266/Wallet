using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace WalletLib.Repository.File
{
    /// <summary>
    /// Wallet file saver, that use<see cref="BinaryFormatter"/> 
    /// for serializing and deserializing wallet data
    /// </summary>
    class BinaryFileWalletSaver : IFileWalletSaver
    {
        public Dictionary<string, int> LoadFromFile(string filePath)
        {
            var result = new Dictionary<string, int>();
            var fileStream = new FileStream(filePath, FileMode.Open);
            try
            {
                var formatter = new BinaryFormatter();

                result = (Dictionary<string, int>)formatter.Deserialize(fileStream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileStream.Close();
            }

            return result;
        }

        public void SaveToFile(string filePath, Dictionary<string, int> userCash)
        {
            var fileStream = new FileStream(filePath, FileMode.Create);

            try
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, userCash);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileStream.Close();
            }
        }
    }

}