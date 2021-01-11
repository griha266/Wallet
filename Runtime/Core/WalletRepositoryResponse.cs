using System;
using System.Collections.Generic;

namespace WalletLib.Core
{
    /// <summary>
    /// Repository response body.
    /// <para>If response sucessful, contains user cash</para>
    /// <para>If repository fail to load or update wallet, response contains error message</para>
    /// </summary>
    public struct WalletRepositoryResponse
    {
        private readonly Exception _exception;
        private readonly Dictionary<string, int> _userCash;

        private WalletRepositoryResponse(Exception exception, Dictionary<string, int> userCash)
        {
            this._exception = exception;
            this._userCash = userCash;
        }

        /// <summary>
        /// Create valid response with selected wallet data
        /// </summary>
        /// <param name="userCash">Wallet data</param>
        /// <returns>Valid variant of <see cref="WalletRepositoryResponse"/></returns>
        public static WalletRepositoryResponse Valid(Dictionary<string, int> userCash) => new WalletRepositoryResponse(exception: null, userCash);

        /// <summary>
        /// Create invalid response with message from exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Invalid variant of <see cref="WalletRepositoryResponse"/></returns>
        public static WalletRepositoryResponse Invalid(Exception exception) => new WalletRepositoryResponse(exception, userCash: null);

        /// <summary>
        /// Is response is valid
        /// </summary>
        public bool IsValid => _exception == null;

        /// <summary>
        /// Try get exception from response.
        /// <para>Can be null</para>
        /// </summary>
        public Exception Exception => _exception;
        
        /// <summary>
        /// Try get wallet data from response
        /// <para>Can be null</para>
        /// </summary>
        public Dictionary<string, int> UserCash => _userCash;

    }

}