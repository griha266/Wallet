# Wallet

Library for managing user wallet in Unity.

## Git dependencies

Library use next git dependencies:

* UniRx - `"com.neuecc.unirx":"https://github.com/neuecc/UniRx.git?path=/Assets/Plugins/UniRx/Scripts"`

* UniTask - `"com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"`

For resolving install [git package resolver](https://github.com/sandolkakos/unity-package-manager-utilities) or
just copy this to your project `manifest.json` file. 

## How to use

1. Create new scene

2. Add `WalletStartup` component on game object

3. Create repository config for Wallet by open CreateAssetMenu: `Wallet/<Type of repository>`

4. Add config ref to `WalletStartup`

5. Create game objects with `CurrencyControl` or `CurrencyView` components

6. For more examples, go `Packages/Wallet/Examples/`
