using System;
using System.Collections;
using System.Collections.Generic;
using GameScripts.Utils;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScripts.Game
{
    public class FieldViewModel
    {
        public event Action<Vector2Int, Vector2Int> CellMoved;
        public IReadOnlyReactiveProperty<bool> IsCompleted;
        private FieldModel _fieldModel;
        private Vector2Int _freeCellIndex;
        private Vector2Int _fieldSize;
        private IReactiveProperty<bool> _isCompleted;
        private bool _isReshuffling = false;

        public FieldViewModel(FieldModel fieldModel)
        {
            _fieldModel = fieldModel;
            _fieldSize = new Vector2Int(
                fieldModel.FieldMatrix.GetLength(0), 
                fieldModel.FieldMatrix.GetLength(1));
            _freeCellIndex = FindFreeCellIndex();
            _isCompleted = new ReactiveProperty<bool>(false);
            IsCompleted = _isCompleted;
        }

        public IEnumerator ReshuffleCoroutine()
        {
            _isReshuffling = true;
            var horizontal = true;
            for (var i = 0; i < Random.Range(10, 15); i++)
            {
                var availableCells = GetAvailableForMoveCells(horizontal);
                horizontal = !horizontal;
                MoveCells(availableCells[Random.Range(0, availableCells.Count)]);
                yield return new WaitForSeconds(0.4f);
            }

            _isReshuffling = false;
        }

        public void MoveCellsRequest(Vector2Int index)
        {
            if (_isReshuffling)
                return;
            MoveCells(index);
        }

        public void MoveCells(Vector2Int index)
        {
            if (!MoveAvailable(index))
                return;
            var difference = _freeCellIndex - index;
            var differenceLength = Mathf.Abs(difference.x) + Mathf.Abs(difference.y);
            var direction = new Vector2Int(difference.x.Sign(), difference.y.Sign());
            for (var i = differenceLength; i > 0; i--)
            {
                var nextIndex = index + direction * i;
                var prevIndex = index + direction * (i - 1);
                MoveCell(prevIndex, nextIndex);
            }
            
            _fieldModel.FieldMatrix[index.x, index.y] = 0;
            _freeCellIndex = index;
            _isCompleted.Value = CheckIsCompleted();
        }

        private List<Vector2Int> GetAvailableForMoveCells(bool horizontal)
        {
            var availableCells = new List<Vector2Int>(horizontal ? _fieldSize.x - 1 : _fieldSize.y - 1);
            for (var i = 0; i < _fieldSize.x; i++)
            {
                var index = horizontal 
                    ? new Vector2Int(i, _freeCellIndex.y)
                    : new Vector2Int(_freeCellIndex.x, i);
                if (index == _freeCellIndex)
                    continue;
                availableCells.Add(index);
            }
            return availableCells;
        }

        private bool CheckIsCompleted()
        {
            var counter = 1;
            for (var y = 0; y < _fieldSize.y; y++)
            {
                for (var x = 0; x < _fieldSize.x; x++)
                {
                    if (x == _fieldSize.x - 1 && y == _fieldSize.y - 1)
                        return true;
                    if (_fieldModel.FieldMatrix[x, y] != counter++)
                        return false;
                }
            }

            return false;
        }
        
        private void MoveCell(Vector2Int prevIndex, Vector2Int nextIndex)
        {
            _fieldModel.FieldMatrix[nextIndex.x, nextIndex.y] =
                _fieldModel.FieldMatrix[prevIndex.x, prevIndex.y];
            CellMoved?.Invoke(prevIndex, nextIndex);
        }

        private bool MoveAvailable(Vector2Int index)
        {
            return index != _freeCellIndex && index.x == _freeCellIndex.x || index.y == _freeCellIndex.y;
        }

        private Vector2Int FindFreeCellIndex()
        {
            for (var y = _fieldSize.y - 1; y >= 0; y--)
            {
                for (var x = _fieldSize.x - 1; x >= 0; x--)
                {
                    if (_fieldModel.FieldMatrix[x, y] == 0)
                        return new Vector2Int(x, y);
                }
            }

            throw new ArgumentException("Cannot find free cell in matrix");
        }
    }
}