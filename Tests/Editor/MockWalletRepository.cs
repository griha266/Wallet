using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using WalletLib.Core;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Mock for wallet repository
    /// </summary>
    class MockWalletRepository : IWalletRepository
    {
        private readonly float _delayedSeconds;
        /// <summary>
        /// Mock repository constructor
        /// </summary>
        /// <param name="delayedSeconds">Fake delay for async operation</param>
        public MockWalletRepository(float delayedSeconds)
        {
            _delayedSeconds = delayedSeconds;
        }
        private Dictionary<string, int> _userCash = new Dictionary<string, int>
        {
            {"currency_1", 20},
            {"currency_2", 4000},
            {"currency_3", 0}
        };



        public UniTask<WalletRepositoryResponse> LoadWallet()
        {
            Debug.Log("Load Wallet");
            return TestsAsyncUtils.Delay(_delayedSeconds).ContinueWith(() =>
            {
                Debug.Log("Load Wallet End");
                return WalletRepositoryResponse.Valid(_userCash);
            });
        }

        public UniTask<WalletRepositoryResponse> SetWallet(Dictionary<string, int> userCash)
        {
            Debug.Log("Set Wallet");
            return TestsAsyncUtils.Delay(_delayedSeconds).ContinueWith(() =>
            {
                Debug.Log("Set Wallet End");
                _userCash = userCash;
                return WalletRepositoryResponse.Valid(_userCash);
            });
        }

        public UniTask<WalletRepositoryResponse> UpdateWallet(string currencyId, int newValue)
        {
            Debug.Log("Update wallet");
            return TestsAsyncUtils.Delay(_delayedSeconds).ContinueWith(() =>
            {
                if (_userCash.ContainsKey(currencyId))
                {
                    _userCash[currencyId] = newValue;
                    Debug.Log("Update wallet End");
                    return WalletRepositoryResponse.Valid(_userCash);
                }
                else
                {
                    Debug.Log("Update wallet End");
                    return WalletRepositoryResponse.Invalid(new IndexOutOfRangeException($"Cannot find currency with id {currencyId}"));
                }

            });
        }
    }
}
