using Runtime.IAP;
using Runtime.Services.ApplicationState;
using Runtime.Services.Audio;
using Runtime.Services.Network;
using Runtime.Services.UserData;
using Runtime.UI;
using Core.Compressor;
using Core.Factory;
using Core.Services.Audio;
using Core.Services.ScreenOrientation;
using Core;
using Runtime.Application.UserAccountSystem;
using UnityEngine;
using Zenject;

namespace Runtime.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AccountStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSelectionStateController>().AsSingle();
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<SettingsStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopStateController>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
            Container.Bind<UserAccountService>().AsSingle();
            Container.Bind<PyramidController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ImageProcessingService>().AsSingle();
            Container.Bind<AvatarSelectionService>().AsSingle();
        }
    }
}