using System.Collections.Generic;
using GameScripts.Pool;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameScripts.Game
{
    public class GameController : MonoBehaviour
    {
        private FieldViewModel _fieldViewModel;
        private FieldModel _fieldModel;

        private CellViewFactory _cellViewFactory;
        private FieldViewModelContainer _fieldViewModelContainer;
        private SimpleObjectPool _simpleObjectPool;
        private Vector2Int _fieldSize;

        [Inject]
        public void Construct(CellViewFactory cellViewFactory,
            FieldViewModelContainer fieldViewModelContainer,
            SimpleObjectPool simpleObjectPool)
        {
            _simpleObjectPool = simpleObjectPool;
            _fieldViewModelContainer = fieldViewModelContainer;
            _cellViewFactory = cellViewFactory;
        }

        public void StartGame(Vector2Int fieldSize)
        {
            _fieldSize = fieldSize;
            _fieldModel = new FieldModel(new Vector2Int(fieldSize.x, fieldSize.y));
            StartGameInternal(fieldSize, true);
        }

        public void ContinueGame()
        {
            _fieldModel = FieldModel.LoadFromLocalDb();
            _fieldSize = new Vector2Int(
                _fieldModel.FieldMatrix.GetLength(0),
                _fieldModel.FieldMatrix.GetLength(1));
            StartGameInternal(_fieldSize, false);
        }

        public void RestartGame()
        {
            FinishGame();
            StartGame(_fieldSize);
        }

        private void StartGameInternal(Vector2Int fieldSize, bool needReshuffle)
        {
            _fieldViewModel = new FieldViewModel(_fieldModel);
            _fieldViewModelContainer.FieldViewModel.Value = _fieldViewModel;
            for (var y = 0; y < fieldSize.y; y++)
            {
                for (var x = 0; x < fieldSize.x; x++)
                {
                    if (_fieldModel.FieldMatrix[x, y] == 0)
                        continue;

                    var cellViewModel = new CellViewModel(_fieldViewModel,
                        new Vector2Int(x, y),
                        _fieldModel.FieldMatrix[x, y]);
                    _cellViewFactory.Create(cellViewModel, fieldSize);
                }
            }

            if (needReshuffle)
                StartCoroutine(_fieldViewModel.ReshuffleCoroutine());
        }

        public void FinishGame(bool save = false)
        {
            StopAllCoroutines();
            _fieldViewModel = null;
            if (!save)
                FieldModel.ClearSavedInstance();
            else
                _fieldModel.SaveToLocalDb();
            _simpleObjectPool.DespawnAll();
            _fieldViewModelContainer.FieldViewModel.Value = null;
        }

        private void OnApplicationQuit()
        {
            _fieldModel?.SaveToLocalDb();
        }
    }
}