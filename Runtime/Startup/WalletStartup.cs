using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using WalletLib.Controller;
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
        private static WalletController _controllerInstance;
        private static Queue<Action<WalletController>> _onControllerCreated = new Queue<Action<WalletController>>();

        private void Awake()
        {
            IWalletRepository repository = Config.Create();

            Wallet
                .Create(repository)
                .ContinueWith((wallet) =>
                {
                    _controllerInstance = new WalletController(wallet, new UnityLogger());
                    Debug.Log("Created controller");
                    if (_onControllerCreated.Count > 0)
                    {
                        Debug.Log("Start callback queue");
                        while (_onControllerCreated.Count != 0)
                        {
                            Debug.Log("Call callback");
                            _onControllerCreated.Dequeue().Invoke(_controllerInstance);
                        }
                        _onControllerCreated = null;
                    }

                    _controllerInstance
                        .OnWalletStateChange
                        .Where(state => state.IsError())
                        .Subscribe(state => Debug.LogError(state.exceptionMessage))
                        .AddTo(this);

                })
                .Forget(exception => Debug.LogError(exception));
        }

        /// <summary>
        /// Subscribe to<see cref="WalletController"/> initialization.
        /// If controller already created, invoke callback immediately.
        /// </summary>
        /// <param name="onControllerCreated">Callback</param>
        public static void RequestController(Action<WalletController> onControllerCreated)
        {
            Debug.Log("Controller requested");
            if (_controllerInstance == null)
            {
                Debug.Log("Controller is not created, add callback to queue");
                _onControllerCreated.Enqueue(onControllerCreated);
            }
            else
            {
                Debug.Log("Controller created, invoke callback");
                onControllerCreated.Invoke(_controllerInstance);
            }
        }


    }

}