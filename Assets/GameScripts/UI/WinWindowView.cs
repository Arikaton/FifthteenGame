using GameScripts.Game;
using Lean.Gui;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    [RequireComponent(typeof(LeanWindow))]
    public class WinWindowView : MonoBehaviour
    {
        [SerializeField] private LeanWindow _leanWindow;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Button _goToMenuButton;
        
        private CompositeDisposable _disposable;
        private GameController _gameController;
        private UIManager _uiManager;

        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, GameController gameController, UIManager uiManager)
        {
            _uiManager = uiManager;
            _gameController = gameController;
            fieldViewModelContainer.FieldViewModel.Subscribe(OnFieldViewModelChanged).AddTo(this);
            _goToMenuButton.OnClickAsObservable().Subscribe(OnGoToMenu).AddTo(this);
            _restartGameButton.OnClickAsObservable().Subscribe(OnRestartGame).AddTo(this);
        }

        private void OnRestartGame(Unit unit)
        {
            _gameController.RestartGame();
            _leanWindow.TurnOff();
        }

        private void OnGoToMenu(Unit unit)
        {
            _gameController.FinishGame();
            _uiManager.GoToMenu();
        }

        private void OnFieldViewModelChanged(FieldViewModel fieldViewModel)
        {
            if (fieldViewModel == null)
            {
                _disposable?.Dispose();
            }
            else
            {
                _disposable = new CompositeDisposable();
                fieldViewModel.IsCompleted.Where(completed => completed).Subscribe(OnCompleteGame).AddTo(_disposable);
            }
        }

        private void OnCompleteGame(bool isCompleted)
        {
            _leanWindow.TurnOn();
        }
    }
}