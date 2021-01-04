using System.Collections.Generic;
using UnityEngine;
using WalletLib.Core;
using WalletLib.Repository.File;

namespace WalletLib.Startup
{
    /// <summary>
    /// Config for file repository
    /// <para><see cref="FileWalletRepository"/></para>
    /// </summary>
    [CreateAssetMenu(fileName = "FileWallet", menuName = "Wallet/FileRepo", order = 1)]
    class FileWalletRepositoryConfig : AbstractWalletRepositoryConfig
    {
        /// <summary>
        /// List of available currencies
        /// </summary>
        public List<string> Currencies;
        /// <summary>
        /// File name for wallet
        /// </summary>
        public string FileName = "WalletFile";
        /// <summary>
        /// Use <see cref="JsonFileWalletSaver"/> or <see cref="BinaryFileWalletSaver"/>
        /// </summary>
        public bool Json = true;
        /// <summary>
        /// Shoud clean file when repository was created
        /// </summary>
        public bool CreateClean = false;

        public override IWalletRepository Create() => FileWalletRepository.Create(
            currencyList: Currencies.ToArray(),
            FileName,
            Json,
            CreateClean
        );

    }

}