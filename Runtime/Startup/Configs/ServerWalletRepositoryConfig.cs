using UnityEngine;
using WalletLib.Core;
using WalletLib.Repository;

namespace WalletLib.Startup
{
    /// <summary>
    /// Config for server repository
    /// <para><see cref="ServerWalletRepository"/></para>
    /// </summary>
    [CreateAssetMenu(fileName = "ServerWallet", menuName = "Wallet/ServerRepo", order = 1)]
    public class ServerWalletRepositoryConfig : AbstractWalletRepositoryConfig
    {
        /// <summary>
        /// Server host for request
        /// </summary>
        public string Host = "http://localhost:5000";
        /// <summary>
        /// Id of user wallet
        /// </summary>
        public string WalletId = "WalletId";

        public override IWalletRepository Create() => new ServerWalletRepository(Host, WalletId);

    }

}