using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using WalletLib.Core;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WalletLib.Repository
{
    /// <summary>
    /// Server repository class for wallet
    /// <para>
    /// Server must provide next methods
    /// </para>
    /// <list>
    /// <item>
    /// <para><c>GET: {host}/:walletId</c> - Recive wallet data</para>
    /// <para>Response Body:</para>
    /// <example>
    /// <code>
    /// {
    ///     "currencyId(string)": currencyValue(int),
    ///     ...       
    /// }
    /// </code>
    /// </example>
    /// </item>
    /// <item>
    /// <para><c>POST: {host}/:walletId</c> - Set new wallet data</para>
    /// <para>Request Body:</para>
    /// <example>
    /// <code>
    /// {
    ///     "currencyId(string)": currencyValue(int),
    ///     ...       
    /// }
    /// </code>
    /// </example>
    /// <para>Response Body:</para>
    /// <example>
    /// <code>
    /// {
    ///     "currencyId(string)": currencyValue(int),
    ///     ...       
    /// }
    /// </code>
    /// </example>
    /// </item>
    /// <item>
    /// <para><c>PUT: {host}/:walletId</c> - Update selected currency value</para>
    /// <para>Request Body:</para>
    /// <example>
    /// <code>
    /// {
    ///     "currencyId": string,
    ///     "value": int       
    /// }
    /// </code>
    /// </example>
    /// <para>Response Body:</para>
    /// <example>
    /// <code>
    /// {
    ///     "currencyId(string)": currencyValue(int),
    ///     ...       
    /// }
    /// </code>
    /// </example>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use <see cref="System.Net.Http.HttpClient"/> for http requests to api
    /// </para>
    /// </remarks>
    public class ServerWalletRepository : IWalletRepository
    {
        private readonly string _walletUri;
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// Constructor for<see cref="ServerWalletRepository"/>
        /// </summary>
        /// <param name="host">Api address for making requests</param>
        /// <param name="walletId">Wallet ID for user</param>
        public ServerWalletRepository(string host, string walletId)
        {
            _walletUri = $"{host}/{walletId}";
        }

        public async UniTask<WalletRepositoryResponse> LoadWallet()
        {
            try
            {
                var response = await client.GetAsync(_walletUri);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return WalletRepositoryResponse.Invalid(new Exception($"Server responded with code {response.StatusCode}"));
                }

                var responseString = await response.Content.ReadAsStringAsync();
                return WalletRepositoryResponse.Valid(JsonConvert.DeserializeObject<Dictionary<string, int>>(responseString));
            }
            catch (Exception exception)
            {
                return WalletRepositoryResponse.Invalid(exception);
            }
        }

        public async UniTask<WalletRepositoryResponse> SetWallet(Dictionary<string, int> userCash)
        {
            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(userCash), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_walletUri, body);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return WalletRepositoryResponse.Invalid(new Exception($"Server responded with code {response.StatusCode}"));
                }

                return WalletRepositoryResponse.Valid(userCash);
            }
            catch (Exception exception)
            {
                return WalletRepositoryResponse.Invalid(exception);
            }
        }

        public async UniTask<WalletRepositoryResponse> UpdateWallet(string currencyId, int newValue)
        {
            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(new { currencyId, value = newValue }), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(_walletUri, body);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return WalletRepositoryResponse.Invalid(new Exception($"Server responded with code {response.StatusCode}"));
                }

                var responseString = await response.Content.ReadAsStringAsync();
                return WalletRepositoryResponse.Valid(JsonConvert.DeserializeObject<Dictionary<string, int>>(responseString));
            }
            catch (Exception exception)
            {
                return WalletRepositoryResponse.Invalid(exception);
            }
        }



    }

}