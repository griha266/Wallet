using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using WalletLib.Core;
using WalletLib.Logger.Unity;

namespace WalletLib.Startup
{
    /// <summary>
    /// Base class for initialize Wallet in Unity scene
    /// </summary>
    class WalletStartup : MonoBehaviour
    {
        /// <summary>
        /// Config for repository<see cref="AbstractWalletRepositoryConfig"/>
        /// </summary>
        public AbstractWalletRepositoryConfig Config;
        private static Wallet _walletInstance;
        private static Queue<Action<Wallet>> _onWalletCreated = new Queue<Action<Wallet>>();

        private void Awake()
        {
            IWalletRepository repository = Config.Create();

            Wallet
                .Create(repository, new UnityLogger())
                .ContinueWith((wallet) =>
                {
                    if (_onWalletCreated.Count > 0)
                    {
                        while (_onWalletCreated.Count != 0)
                        {
                            _onWalletCreated.Dequeue().Invoke(_walletInstance);
                        }
                        _onWalletCreated = null;
                    }

                    _walletInstance
                        .OnStateChange
                        .Where(state => state.IsError())
                        .Subscribe(state => Debug.LogError(state.exceptionMessage))
                        .AddTo(this);

                })
                .Forget(exception => Debug.LogError(exception));
        }

        /// <summary>
        /// Subscribe to<see cref="Wallet"/> initialization.
        /// If controller already created, invoke callback immediately.
        /// </summary>
        /// <param name="onWalletCreated">Callback</param>
        public static void RequestWallet(Action<Wallet> onWalletCreated)
        {
            if (_walletInstance == null)
            {
                _onWalletCreated.Enqueue(onWalletCreated);
            }
            else
            {
                onWalletCreated.Invoke(_walletInstance);
            }
        }


    }

}