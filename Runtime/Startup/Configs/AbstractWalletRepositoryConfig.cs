using UnityEngine;
using WalletLib.Core;

namespace WalletLib.Startup
{
    /// <summary>
    /// Base abstract class for<see cref="IWalletRepository"/> config.
    /// Used for creating repositories
    /// </summary>
    abstract class AbstractWalletRepositoryConfig : ScriptableObject
    {
        /// <summary>
        /// Create repository with current config
        /// </summary>
        /// <returns><see cref="IWalletRepository"/></returns>
        public abstract IWalletRepository Create();
    }
}