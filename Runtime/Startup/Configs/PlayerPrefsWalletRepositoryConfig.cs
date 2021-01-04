using System.Collections.Generic;
using UnityEngine;
using WalletLib.Core;
using WalletLib.Repository;

namespace WalletLib.Startup
{
    /// <summary>
    /// Config for PlayerPrefs repository
    /// <para><see cref="PlayerPrefsWalletRepository"/></para>
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerPrefsWallet", menuName = "Wallet/PlayerPrefsRepo", order = 1)]
    public class PlayerPrefsWalletRepositoryConfig : AbstractWalletRepositoryConfig
    {
        /// <summary>
        /// Wallet key for Unity PlayerPrefs
        /// </summary>
        public string WalletKey = "WalletKey";
        /// <summary>
        /// List of available currencies
        /// </summary>
        public List<string> Currencies = new List<string>();
        /// <summary>
        /// Shoud clean PlayerPrefs string when creating repository
        /// </summary>
        public bool CreateClean = false;

        public override IWalletRepository Create() => PlayerPrefsWalletRepository.Create(
            currencyList: Currencies.ToArray(),
            WalletKey,
            CreateClean
        );

    }

}