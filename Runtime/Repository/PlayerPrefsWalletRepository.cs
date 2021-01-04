using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using WalletLib.Core;
using System.Linq;

namespace WalletLib.Repository
{
    /// <summary>
    /// PlayerPrefs repository class for wallet
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use Unity<see cref="PlayerPrefs"/> for store and saving wallet data as json string
    /// </para>
    /// <para>
    /// Can be created only from<see cref="PlayerPrefsWalletRepository.Create(string[], string, bool)"/>>
    /// </para>
    /// </remarks>
    class PlayerPrefsWalletRepository : IWalletRepository
    {
        /// <summary>
        /// Create <see cref="PlayerPrefsWalletRepository"/> instance
        /// and create PlayerPrefs string if not existed
        /// </summary>
        /// <param name="currencyList">List of all available currencies</param>
        /// <param name="walletKey">Key in PlayerPrefs for storing json</param>
        /// <param name="createClean">Clear previous wallet data if existed before initialize repository</param>
        /// <returns><see cref="PlayerPrefsWalletRepository"/></returns>
        public static PlayerPrefsWalletRepository Create(string[] currencyList, string walletKey, bool createClean = false)
        {
            if (PlayerPrefs.HasKey(walletKey))
            {
                var userCash = JsonConvert.DeserializeObject<Dictionary<string, int>>(PlayerPrefs.GetString(walletKey));

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
                    PlayerPrefs.SetString(walletKey, JsonConvert.SerializeObject(userCash));
                }
            }
            else
            {
                var userCash = currencyList.ToDictionary(
                    keySelector: entry => entry,
                    elementSelector: _ => 0
                );

                PlayerPrefs.SetString(walletKey, JsonConvert.SerializeObject(userCash));
            }

            return new PlayerPrefsWalletRepository(walletKey);
        }


        private readonly string _walletKey;
        private PlayerPrefsWalletRepository(string walletKey)
        {
            _walletKey = walletKey;
        }


        public UniTask<WalletRepositoryResponse> LoadWallet()
        {
            if (!PlayerPrefs.HasKey(_walletKey))
            {
                return WalletRepositoryResponse
                    .Invalid(new NullReferenceException("Cannot read wallet from PlayerPrefs"))
                    .ToUniTask();
            }

            var stringState = PlayerPrefs.GetString(_walletKey);
            var userCash = JsonConvert.DeserializeObject<Dictionary<string, int>>(stringState);

            if (userCash == null)
            {
                return WalletRepositoryResponse.Invalid(new NullReferenceException("Cannot read wallet from PlayerPrefs")).ToUniTask();
            }

            return WalletRepositoryResponse.Valid(userCash).ToUniTask();

        }

        public UniTask<WalletRepositoryResponse> SetWallet(Dictionary<string, int> userCash)
        {
            PlayerPrefs.SetString(_walletKey, JsonConvert.SerializeObject(userCash));
            return WalletRepositoryResponse.Valid(userCash).ToUniTask();
        }

        public UniTask<WalletRepositoryResponse> UpdateWallet(string currencyId, int newValue)
        {
            if (!PlayerPrefs.HasKey(_walletKey))
            {
                return WalletRepositoryResponse
                    .Invalid(new NullReferenceException("Cannot read wallet from PlayerPrefs"))
                    .ToUniTask();
            }

            var stringState = PlayerPrefs.GetString(_walletKey);
            var cachedState = JsonConvert.DeserializeObject<Dictionary<string, int>>(stringState);

            if (cachedState == null)
            {
                return WalletRepositoryResponse
                    .Invalid(new NullReferenceException("Cannot read wallet from PlayerPrefs"))
                    .ToUniTask();

            }

            if (!cachedState.ContainsKey(currencyId))
            {
                return WalletRepositoryResponse
                    .Invalid(new ArgumentOutOfRangeException($"Cannot find currency with id {currencyId}"))
                    .ToUniTask();
            }

            cachedState[currencyId] = newValue;
            PlayerPrefs.SetString(_walletKey, JsonConvert.SerializeObject(cachedState));

            return WalletRepositoryResponse.Valid(cachedState).ToUniTask();

        }

    }

}