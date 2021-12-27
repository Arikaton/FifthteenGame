using Lean.Gui;
using UnityEngine;

namespace GameScripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private LeanWindow _mainMenuWindow;

        private void Start()
        {
            _mainMenuWindow.TurnOn();
        }

        public void GoToMenu()
        {
            _mainMenuWindow.TurnOn();
        }
    }
}