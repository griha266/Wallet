using System;
using System.Collections.Generic;

namespace WalletLib.Core
{
    /// <summary>
    /// Type of<see cref="WalletState"/>
    /// <para>
    /// Can be:
    /// </para>
    /// <list>
    /// <item>
    /// <term>Loading</term>
    /// <description>Wallet is waiting for repository response</description>
    /// </item>
    /// <item>
    /// <term>Error</term>
    /// <description>Repository responded to wallet with error</description>
    /// </item>
    /// <item>
    /// <term>Valid</term>
    /// <description>Repository responded to wallet succsesful</description>
    /// </item>
    /// </list>
    /// </summary>
    public enum WalletStateType
    {
        /// <summary>
        /// Repository responded to wallet with error
        /// </summary>
        Error,
        /// <summary>
        /// Wallet is waiting for repository response
        /// </summary>
        Loading,
        /// <summary>
        /// Repository responded to wallet succsesful
        /// </summary>
        Valid
    }

    /// <summary>
    /// State of wallet
    /// <para>
    /// Can be as next types:
    /// </para>
    /// <list>
    /// <item>
    /// <term>Loading</term>
    /// <description>Wallet is waiting for repository response</description>
    /// </item>
    /// <item>
    /// <term>Error</term>
    /// <description>Repository responded to wallet with error</description>
    /// </item>
    /// <item>
    /// <term>Valid</term>
    /// <description>Repository responded to wallet succsesful</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>Can contains exception message or wallet data</para>
    /// <para>
    /// For creating state use next methods:
    /// <see cref="WalletState.Valid(Dictionary{string, int})"/>,
    /// <see cref="WalletState.Invalid(Exception)"/>,
    /// <see cref="WalletState.Loading"/>
    /// </para>
    /// </remarks>
    public struct WalletState
    {
        /// <summary>
        /// State type<see cref="WalletStateType"/>
        /// </summary>
        public readonly WalletStateType stateType;
        /// <summary>
        /// Exception message for Invalid state
        /// <para>
        /// Can be null if state is<see cref="WalletStateType.Loading"/> or <see cref="WalletStateType.Valid"/>
        /// </para>
        /// </summary>
        public readonly string exceptionMessage;
        /// <summary>
        /// Wallet data
        /// <para>
        /// Can be null if state is<see cref="WalletStateType.Loading"/> or <see cref="WalletStateType.Error"/>
        /// </para>
        /// </summary>
        private readonly Dictionary<string, int> _userCash;

        private WalletState(WalletStateType stateType, Exception exception, Dictionary<string, int> userCash)
        {
            this.stateType = stateType;
            this.exceptionMessage = exception?.Message;
            this._userCash = userCash;
        }

        private WalletState(WalletStateType stateType, string exceptionMessage, Dictionary<string, int> userCash)
        {
            this.stateType = stateType;
            this.exceptionMessage = exceptionMessage;
            this._userCash = userCash;
        }

        /// <summary>
        /// Create valid state with selected wallet data
        /// </summary>
        /// <param name="userCash">Wallet data</param>
        /// <returns><see cref="WalletState"/></returns>
        public static WalletState Valid(Dictionary<string, int> userCash) => new WalletState(
            stateType: WalletStateType.Valid,
            exception: null,
            userCash
        );

        /// <summary>
        /// Create invalid state with message from exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns><see cref="WalletState"/></returns>
        public static WalletState Invalid(Exception exception) => new WalletState(
            stateType: WalletStateType.Error,
            exception,
            userCash: null
        );

        /// <summary>
        /// Create loading state
        /// </summary>
        /// <returns><see cref="WalletState"/></returns>
        public static WalletState Loading => new WalletState(
            stateType: WalletStateType.Loading,
            exceptionMessage: null,
            userCash: null
        );

        /// <summary>
        /// Check is State is valid
        /// </summary>
        public bool IsValid() => stateType == WalletStateType.Valid;

        /// <summary>
        /// Check is state contains error
        /// </summary>
        public bool IsError() => stateType == WalletStateType.Error;

        /// <summary>
        /// Check is state is loading
        /// </summary>
        public bool IsLoading() => stateType == WalletStateType.Loading;
        
        /// <summary>
        /// Try get currency value from Valid state
        /// </summary>
        /// <param name="currencyId">ID of selected currency</param>
        /// <returns>Value of selected currency</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Thrown if wallet data don't have selected currency
        /// </exception>
        /// <exception cref="System.NullReferenceException">
        /// Thrown if state is not valid
        /// </exception>
        public int GetUserCash(string currencyId) => _userCash[currencyId];

        /// <summary>
        /// Check if wallet data contains selected currency
        /// </summary>
        /// <param name="currencyId">ID of selected currency</param>
        /// <exception cref="System.NullReferenceException">
        /// Thrown if state is not valid
        /// </exception>
        public bool ContainsCurrency(string currencyId) => _userCash.ContainsKey(currencyId);
    }
}