using System;
using WalletLib.Core;
using WalletLib.Logger;

namespace WalletLib.Controller
{
    /// <summary>
    /// Controller class for interaction with wallet
    /// </summary>
    public class WalletController
    {
        private readonly Wallet _wallet;
        private readonly ILogger _logger;
        /// <summary>
        /// Current state of wallet.
        /// <see cref="WalletLib.Core.WalletState"/>
        /// </summary>
        public WalletState CurrentState => _wallet.CurrentState;
        /// <summary>
        /// Stream of wallet state changes,
        /// <see cref="WalletLib.Core.WalletState"/>
        /// </summary>
        public IObservable<WalletState> OnWalletStateChange => _wallet.OnStateChange;
        
        public WalletController(Wallet wallet, ILogger logger)
        {
            _wallet = wallet;
            _logger = logger;
        }

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
            _wallet.UpdateWallet(currencyId, newValue);
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

            _wallet.UpdateWallet(currencyId, 0);
        }
    }

}