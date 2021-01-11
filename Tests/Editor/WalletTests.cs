using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WalletLib.Core;
using UniRx;
using System.Collections.Generic;
using WalletLib.Logger.Unity;

namespace Tests
{

    public class WalletTests
    {
        private float _delayedSeconds = 2;
        private UnityLogger _logger = new UnityLogger();

        [UnityTest]
        public IEnumerator wallet_shoud_created()
        {
            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds), _logger)
                .ToCoroutine(
                    resultHandler: wallet => Assert.NotNull(wallet),
                    exceptionHandler: exception => Debug.LogError(exception)
                );
        }

        [UnityTest]
        public IEnumerator wallet_shoud_load_user_cash()
        {

            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds), _logger)
                .ContinueWith(wallet => wallet.CurrentState)
                .ToCoroutine(
                    resultHandler: walletState =>
                    {
                        Assert.IsTrue(walletState.IsValid());
                        Assert.NotZero(walletState.GetUserCash("currency_1"));
                    },
                    exceptionHandler: exception => Debug.LogError(exception)
                );
        }


        [UnityTest]
        public IEnumerator wallet_shoud_increase_cash()
        {
            var currencyId = "currency_1";
            int initialCash = -1;
            Wallet _wallet = null;

            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds), _logger)
                .ContinueWith(wallet =>
                {
                    Assert.IsTrue(wallet.CurrentState.IsValid());
                    initialCash = wallet.CurrentState.GetUserCash(currencyId);
                    Assert.Positive(initialCash);
                    _wallet = wallet;
                    return wallet;
                })
                .ToCoroutine(
                    resultHandler: wallet => wallet.AddCash(currencyId, 20),
                    exceptionHandler: exception => Debug.LogError(exception)
                );

            yield return TestsAsyncUtils
                .Delay(_delayedSeconds + 2)
                .ToCoroutine();

            Assert.AreEqual(initialCash + 20, _wallet.CurrentState.GetUserCash(currencyId));

        }


    }
}
