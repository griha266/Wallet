using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WalletLib.Startup;

namespace WalletLib.View
{
    /// <summary>
    /// Example of Currency control class
    /// <para>
    /// Can Add, Subtract or crear selected currency in wallet
    /// </para>
    /// </summary>
    public class CurrencyControl : MonoBehaviour
    {
        public Button AddButton;
        public Button SubstractButton;
        public Button ClearButton;
        public string CurrencyId;
        /// <summary>
        /// How much add or subtract on button click
        /// </summary>
        public int DifferencePerClick;

        private void Awake()
        {
            WalletStartup.RequestWallet(wallet =>
            {
                var addStream = AddButton
                    .OnClickAsObservable()
                    .Select((_) => 1);

                var substractStream = SubstractButton
                    .OnClickAsObservable()
                    .Select((_) => -1);

                Observable
                    .Merge(addStream, substractStream)
                    .Subscribe((i) => wallet.AddCash(CurrencyId, DifferencePerClick * i))
                    .AddTo(this);

                ClearButton
                    .OnClickAsObservable()
                    .Subscribe((_) => wallet.ClearCurrency(CurrencyId))
                    .AddTo(this);
            });
        }

    }

}