using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WalletLib.Startup;

namespace WalletLib.View
{
    /// <summary>
    /// Example of Currency view class for wallet
    /// <para>
    /// Subscribe to wallet state and draw selected currency from valid state
    /// </para>
    /// </summary>
    public class CurrencyView : MonoBehaviour
    {
        /// <summary>
        /// Text for current currency value
        /// </summary>
        public Text CurrencyNumber;
        /// <summary>
        /// Selected currency
        /// </summary>
        public string CurrencyId;
        public GameObject LoadingObj;
        public GameObject ErrorObj;
        public GameObject CurrencyObj;

        private void Awake()
        {
            LoadingObj.SetActive(true);
            ErrorObj.SetActive(false);
            CurrencyObj.SetActive(false);

            WalletStartup.RequestWallet(wallet =>
            {
                wallet
                    .OnStateChange
                    .Select(state => state.IsValid())
                    .DistinctUntilChanged()
                    .Subscribe(isValid => CurrencyObj.SetActive(isValid))
                    .AddTo(this);

                wallet
                    .OnStateChange
                    .Select(state => state.IsLoading())
                    .DistinctUntilChanged()
                    .Subscribe(isLoading => LoadingObj.SetActive(isLoading))
                    .AddTo(this);

                wallet
                    .OnStateChange
                    .Select(state => state.IsError())
                    .DistinctUntilChanged()
                    .Subscribe(isError => ErrorObj.SetActive(isError))
                    .AddTo(this);

                wallet
                    .OnStateChange
                    .Where((state) => state.IsValid() && state.ContainsCurrency(CurrencyId))
                    .Select((state) => state.GetUserCash(CurrencyId))
                    .DistinctUntilChanged()
                    .Subscribe((cash) => CurrencyNumber.text = cash.ToString())
                    .AddTo(this);
            });
        }

    }

}