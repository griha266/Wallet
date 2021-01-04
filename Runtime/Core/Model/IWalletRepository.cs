using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace WalletLib.Core
{
    /// <summary>
    /// Base interface for wallet repositories.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Methods in inherited classes shoudn't throw exceptions, 
    /// instead they need to return invalid state with exception message.
    /// <see cref="WalletLib.Core.WalletRepositoryResponse"/> 
    /// </para>
    /// </remarks>
    public interface IWalletRepository
    {   
        /// <summary>
        /// Load last wallet date
        /// </summary>
        /// <returns>Repository response: <see cref="WalletLib.Core.WalletRepositoryResponse" /></returns>
        UniTask<WalletRepositoryResponse> LoadWallet();
        /// <summary>
        /// Change selected currency in wallet to new value
        /// </summary>
        /// <param name="currencyId">ID of selected currency</param>
        /// <param name="newValue">New value for selected currency</param>
        /// <returns>Repository response: <see cref="WalletLib.Core.WalletRepositoryResponse" /></returns>
        UniTask<WalletRepositoryResponse> UpdateWallet(string currencyId, int newValue);
        /// <summary>
        /// Set new wallet data
        /// </summary>
        /// <param name="userCash">New wallet data</param>
        /// <returns>Repository response: <see cref="WalletLib.Core.WalletRepositoryResponse" /></returns>
        UniTask<WalletRepositoryResponse> SetWallet(Dictionary<string, int> userCash);
    }

}