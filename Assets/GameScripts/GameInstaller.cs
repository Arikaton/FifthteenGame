using GameScripts.Game;
using GameScripts.Pool;
using GameScripts.UI;
using UnityEngine;
using Zenject;

namespace GameScripts
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SimpleObjectPool _cellViewsPool;
        [SerializeField] private RectTransform _fieldContainer;
        [SerializeField] private GameController _gameController;
        [SerializeField] private UIManager _uiManager;
        public override void InstallBindings()
        {
            Container.Bind<RectTransform>().FromInstance(_fieldContainer);
            Container.Bind<SimpleObjectPool>().FromInstance(_cellViewsPool);
            Container.Bind<CellViewFactory>().FromNew().AsSingle();
            Container.Bind<GameController>().FromInstance(_gameController);
            Container.Bind<FieldViewModelContainer>().FromNew().AsSingle();
            Container.Bind<UIManager>().FromInstance(_uiManager);
        }
    }
}