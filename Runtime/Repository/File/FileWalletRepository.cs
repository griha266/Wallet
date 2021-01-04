using System;
using System.IO;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using WalletLib.Core;
using System.Linq;
using UnityEngine;

namespace WalletLib.Repository.File
{
    /// <summary>
    /// File repository class for wallet
    /// </summary>
    /// <remarks>
    /// <para>
    /// Can be created only from<see cref="FileWalletRepository.Create(string[], string, bool, bool)"/>>
    /// </para>
    /// <para>
    /// Use<see cref="JsonFileWalletSaver"/> or<see cref="BinaryFileWalletSaver"/> for work with file data
    /// </para>
    /// </remarks>
    public class FileWalletRepository : IWalletRepository
    {
        /// <summary>
        /// Create<see cref="FileWalletRepository"/> instance and
        /// create file if it is not existed yet
        /// </summary>
        /// <param name="currencyList">List of all available currencies</param>
        /// <param name="fileName">Name of the file without extension</param>
        /// <param name="useJson">Use json format for serialization or use binary</param>
        /// <param name="createClean">Clear previous wallet data if existed before initialize repository</param>
        /// <returns><see cref="FileWalletRepository"/></returns>
        public static FileWalletRepository Create(string[] currencyList, string fileName, bool useJson = true, bool createClean = false)
        {
            var filePath = $"{Application.persistentDataPath}/{fileName}" + (useJson ? ".json" : ".dat");
            IFileWalletSaver fileSaver;
            
            if (useJson) fileSaver = new JsonFileWalletSaver();
            else fileSaver = new BinaryFileWalletSaver();
            

            if (System.IO.File.Exists(filePath))
            {
                var userCash = fileSaver.LoadFromFile(filePath);
                var needUpdate = false;

                if (createClean)
                {
                    userCash = userCash.ToDictionary(
                        keySelector: entry => entry.Key,
                        elementSelector: _ => 0
                    );

                    needUpdate = true;
                }

                foreach (var key in currencyList)
                {
                    if (!userCash.ContainsKey(key))
                    {
                        userCash[key] = 0;
                        needUpdate = true;
                    }
                }
                if (needUpdate)
                {
                    fileSaver.SaveToFile(filePath, userCash);
                }
            }
            else
            {
                var userCash = currencyList.ToDictionary(
                    keySelector: currencyId => currencyId,
                    elementSelector: _ => 0
                );
                fileSaver.SaveToFile(filePath, userCash);
            }

            return new FileWalletRepository(filePath, fileSaver);
        }

        private readonly string _filePath;
        private readonly IFileWalletSaver _fileSaver;
        private FileWalletRepository(string filePath, IFileWalletSaver fileSaver)
        {
            _filePath = filePath;
            _fileSaver = fileSaver;
        }

        public UniTask<WalletRepositoryResponse> LoadWallet() => UniTask.Create(() =>
       {
           if (!System.IO.File.Exists(_filePath))
           {
               return WalletRepositoryResponse
                   .Invalid(new FileNotFoundException($"Cannot find file {_filePath}"))
                   .ToUniTask();
           }

           try
           {
               var cachedWallet = _fileSaver.LoadFromFile(_filePath);

               return WalletRepositoryResponse.Valid(cachedWallet).ToUniTask();
           }
           catch (Exception exception)
           {
               return WalletRepositoryResponse.Invalid(exception).ToUniTask();
           }
       });


        public UniTask<WalletRepositoryResponse> SetWallet(Dictionary<string, int> userCash) => UniTask.Create(() =>
       {
           try
           {
               _fileSaver.SaveToFile(_filePath, userCash);

               return WalletRepositoryResponse.Valid(userCash).ToUniTask();
           }
           catch (Exception exception)
           {
               return WalletRepositoryResponse.Invalid(exception).ToUniTask();
           }
       });

        public UniTask<WalletRepositoryResponse> UpdateWallet(string currencyId, int newValue) => UniTask.Create(() =>
       {
           try
           {
               if (!System.IO.File.Exists(_filePath))
               {
                   return WalletRepositoryResponse
                        .Invalid(new FileNotFoundException($"Cannot find file {_filePath}"))
                        .ToUniTask();
               }

               var cachedWallet = _fileSaver.LoadFromFile(_filePath);
               cachedWallet[currencyId] = newValue;
               _fileSaver.SaveToFile(_filePath, cachedWallet);

               return WalletRepositoryResponse.Valid(cachedWallet).ToUniTask();
           }
           catch (Exception exception)
           {
               return WalletRepositoryResponse.Invalid(exception).ToUniTask();
           }
       });
    }

}