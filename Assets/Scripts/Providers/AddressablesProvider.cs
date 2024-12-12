using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Providers
{
    public class AddressablesProvider
    {
        public async UniTask<T> LoadAsync<T>(string assetName)
        {
            T asset = await Addressables.LoadAssetAsync<T>(assetName);

            if (asset == null)
                throw new Exception($"Failed to load asset: {assetName}");

            return asset;
        }
    }
}