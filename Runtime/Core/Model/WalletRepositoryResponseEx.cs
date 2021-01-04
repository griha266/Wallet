using Cysharp.Threading.Tasks;

namespace WalletLib.Core
{
    /// <summary>
    /// Extensions for <see cref="WalletLib.Core.WalletRepositoryResponse"/>
    /// </summary>
    public static class WalletRepositoryResponseEx
    {
        /// <summary>
        /// Convert repository response to<see cref="UniTask.FromResult{WalletRepositoryResponse}(WalletRepositoryResponse)"/>
        /// </summary>
        /// <param name="response">Repository response</param>
        /// <returns><see cref="UniTask{WalletRepositoryResponse}"/></returns>
        public static UniTask<WalletRepositoryResponse> ToUniTask(this WalletRepositoryResponse response) => UniTask.FromResult(response);

        /// <summary>
        /// Convert repository response to<see cref="WalletLib.Core.WalletState"/>
        /// </summary>
        /// <param name="response">Repository response</param>
        /// <returns><see cref="WalletLib.Core.WalletState"/></returns>
        public static WalletState ToWalletState(this WalletRepositoryResponse response) => response.IsValid
                    ? WalletState.Valid(response.UserCash)
                    : WalletState.Invalid(response.Exception);

    }

}