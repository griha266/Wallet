using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WalletLib.Core;
using UniRx;
using System.Collections.Generic;
using WalletLib.Controller;
using WalletLib.Logger.Unity;

namespace Tests
{

    public class WalletTests
    {
        private float _delayedSeconds = 2;

        [UnityTest]
        public IEnumerator wallet_shoud_created()
        {
            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds))
                .ToCoroutine(
                    resultHandler: wallet => Assert.NotNull(wallet),
                    exceptionHandler: exception => Debug.LogError(exception)
                );
        }

        [UnityTest]
        public IEnumerator wallet_shoud_load_user_cash()
        {

            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds))
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
        public IEnumerator wallet_shoud_update_user_cash()
        {
            string currencyId = "currency_1";
            Wallet _wallet = null;
            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds))
                .ContinueWith(wallet =>
                {
                    _wallet = wallet;
                    wallet.UpdateWallet(
                        currencyId,
                        newValue: 2
                    );
                    return wallet.CurrentState;
                })
                .ToCoroutine(
                    resultHandler: walletState =>
                    {
                        Assert.IsTrue(walletState.IsLoading());
                    },
                    exceptionHandler: exception => Debug.LogError(exception)
                );

            yield return TestsAsyncUtils.Delay(_delayedSeconds + 2).ToCoroutine();

            Assert.IsTrue(_wallet.CurrentState.IsValid());
            Assert.AreEqual(_wallet.CurrentState.GetUserCash(currencyId), 2);
        }

        [UnityTest]
        public IEnumerator wallet_shoud_set_user_cash()
        {
            var newUserCash = new Dictionary<string, int> {
                {"another_currency_1", 5},
                {"another_currency_2", 400}
            };
            Wallet _wallet = null;
            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds))
                .ContinueWith(wallet =>
                {
                    _wallet = wallet;
                    wallet.SetWallet(newUserCash);
                    return wallet.CurrentState;
                })
                .ToCoroutine(
                    resultHandler: walletState =>
                    {
                        Assert.IsTrue(walletState.IsLoading());
                    },
                    exceptionHandler: exception => Debug.LogError(exception)
                );

            yield return TestsAsyncUtils.Delay(_delayedSeconds + 2).ToCoroutine();

            Assert.IsTrue(_wallet.CurrentState.IsValid());
            Assert.AreEqual(_wallet.CurrentState.GetUserCash("another_currency_1"), 5);
            Assert.AreEqual(_wallet.CurrentState.GetUserCash("another_currency_2"), 400);
        }

        [UnityTest]
        public IEnumerator wallet_controller_shoud_increase_cash()
        {
            var currencyId = "currency_1";
            int initialCash = -1;
            Wallet _wallet = null;

            yield return Wallet
                .Create(new MockWalletRepository(_delayedSeconds))
                .ContinueWith(wallet =>
                {
                    Assert.IsTrue(wallet.CurrentState.IsValid());
                    initialCash = wallet.CurrentState.GetUserCash(currencyId);
                    Assert.Positive(initialCash);
                    _wallet = wallet;
                    return new WalletController(wallet, new UnityLogger());
                })
                .ToCoroutine(
                    resultHandler: controller => controller.AddCash(currencyId, 20),
                    exceptionHandler: exception => Debug.LogError(exception)
                );

            yield return TestsAsyncUtils
                .Delay(_delayedSeconds + 2)
                .ToCoroutine();

            Assert.AreEqual(initialCash + 20, _wallet.CurrentState.GetUserCash(currencyId));

        }


    }
}
