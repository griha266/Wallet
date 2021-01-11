using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using WalletLib.Logger;

namespace WalletLib.Core
{

    /// <summary>
    /// Base class for Wallet.
    /// <para>
    /// Can be in three states:
    /// </para>
    /// <list>
    /// <item>
    /// <term>Loading</term>
    /// <description>Waiting for repository response</description>
    /// </item>
    /// <item>
    /// <term>Error</term>
    /// <description>Repository responded with error</description>
    /// </item>
    /// <item>
    /// <term>Valid</term>
    /// <description>Repository responded succsesful</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// For creating wallet use method <see cref="Wallet.Create(IWalletRepository)"/>
    /// </para>
    /// </remarks>
    public class Wallet
    {
        /// <summary>
        /// Creating Wallet and load initial data from repository
        /// </summary>
        /// <param name="repository">Wallet repository</param>
        /// <returns><see cref="Wallet"/></returns>
        public static async UniTask<Wallet> Create(IWalletRepository repository, ILogger logger)
        {
            var wallet = new Wallet(repository, logger);
            await wallet.InitWallet();
            return wallet;
        }
        private readonly IWalletRepository _repository;
        private readonly ILogger _logger;
        private readonly BehaviorSubject<WalletState> _userCash;

        private Wallet(IWalletRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
            _userCash = new BehaviorSubject<WalletState>(WalletState.Loading);
        }

        private async UniTask InitWallet()
        {
            var newState = (await _repository.LoadWallet()).ToWalletState();
            _userCash.OnNext(newState);
        }

        /// <summary>
        /// Update value of selected currency.
        /// </summary>
        /// <remarks>
        /// <para>New value cannot be negative</para>
        /// </remarks>
        /// <param name="currencyId">ID of selected currency</param>
        /// <param name="newValue">
        /// New value for selected currency.
        /// Cannot be negative
        /// </param>
        private void UpdateWallet(string currencyId, int newValue)
        {
            if (newValue < 0)
            {
                return;
            }


            _userCash.OnNext(WalletState.Loading);
            UniTask.Create(async () =>
            {
                var newState = (await _repository.UpdateWallet(currencyId, newValue)).ToWalletState();
                _userCash.OnNext(newState);
            }).Forget(exception => _userCash.OnNext(WalletState.Invalid(exception)));

        }

        /// <summary>
        /// Set new wallet data
        /// </summary>
        /// <param name="userCash">New wallet data</param>
        private void SetWallet(Dictionary<string, int> userCash)
        {
            _userCash.OnNext(WalletState.Loading);
            UniTask.Create(async () =>
            {
                var newState = (await _repository.SetWallet(userCash)).ToWalletState();
                _userCash.OnNext(newState);
            }).Forget(exception => _userCash.OnNext(WalletState.Invalid(exception)));

        }

        /// <summary>
        /// Check if wallet is ready to preform operation
        /// </summary>
        /// <returns>Is wallet ready to preform operation</returns>
        private bool WalletIsValid()
        {
            if (CurrentState.IsLoading())
            {
                _logger.LogWarning("Wallet is loading right now");
                return false;
            }

            if (CurrentState.IsError())
            {
                _logger.LogError(CurrentState.exceptionMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Add cash of selected currency.
        /// <para>Can subtract cash if value is negative</para>
        /// </summary>
        /// <param name="currencyId">ID of changing currency</param>
        /// <param name="cashToAdd">Value to add</param>
        public void AddCash(string currencyId, int cashToAdd)
        {
            if (!WalletIsValid())
            {
                return;
            }

            if (!CurrentState.ContainsCurrency(currencyId))
            {
                _logger.LogError($"Wallet don't contains currency with id {currencyId}");
                return;
            }

            var newValue = CurrentState.GetUserCash(currencyId) + cashToAdd;
            UpdateWallet(currencyId, newValue);
        }


        /// <summary>
        /// Set selected currency cash to 0
        /// </summary>
        /// <param name="currencyId">ID of selected currency</param>
        public void ClearCurrency(string currencyId)
        {
            if (!WalletIsValid())
            {
                return;
            }

            UpdateWallet(currencyId, 0);
        }

        /// <summary>
        /// Returns current state of wallet.
        /// <see cref="WalletLib.Core.WalletState"/>
        /// </summary>
        public WalletState CurrentState => _userCash.Value;

        /// <summary>
        /// Stream of new wallet states
        /// <see cref="WalletLib.Core.WalletState"/>
        /// </summary>
        public IObservable<WalletState> OnStateChange => _userCash;

    }
}