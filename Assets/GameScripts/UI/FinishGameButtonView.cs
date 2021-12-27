using System;
using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    [RequireComponent(typeof(Button))]
    public class FinishGameButtonView : MonoBehaviour
    {
        private Button _button;
        private GameController _gameController;
        private UIManager _uiManager;

        [Inject]
        public void Construct(GameController gameController, UIManager uiManager)
        {
            _uiManager = uiManager;
            _gameController = gameController;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.OnClickAsObservable().Subscribe(OnExitGame).AddTo(this);
        }

        private void OnExitGame(Unit unit)
        {
            _gameController.FinishGame(true);
            _uiManager.GoToMenu();
        }
    }
}