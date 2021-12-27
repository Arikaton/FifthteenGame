using UnityEngine;
using Zenject;

namespace GameScripts.Game
{
    public class GameLoader : MonoBehaviour
    {
        private GameController _gameController;

        [Inject]
        public void Construct(GameController gameController)
        {
            _gameController = gameController;
        }
        
        public void LoadGame(int sizeIndex)
        {
            _gameController.StartGame(GetFieldSizeFromIndex(sizeIndex));
        }

        public void LoadSavedGame()
        {
            _gameController.ContinueGame();
        }

        private Vector2Int GetFieldSizeFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return Vector2Int.one * 3;
                case 1:
                    return Vector2Int.one * 4;
                case 2:
                    return Vector2Int.one * 5;
                default:
                    Debug.LogWarning("Index must be in range 0-2. Return default value");
                    return Vector2Int.one * 3;
            }
        }
    }
}