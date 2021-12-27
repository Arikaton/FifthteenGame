using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class CellViewModel
    {
        public IReactiveProperty<Vector2Int> Index;
        public IReactiveProperty<int> Number;
        private FieldViewModel _fieldViewModel;

        public CellViewModel(FieldViewModel fieldViewModel, Vector2Int index, int number)
        {
            _fieldViewModel = fieldViewModel;
            Index = new ReactiveProperty<Vector2Int>(index);
            Number = new ReactiveProperty<int>(number);
            fieldViewModel.CellMoved += OnCellMoved;
        }

        ~CellViewModel()
        {
            _fieldViewModel.CellMoved -= OnCellMoved;
        }

        public void MoveCell()
        {
            _fieldViewModel.MoveCellsRequest(Index.Value);
        }

        private void OnCellMoved(Vector2Int prevIndex, Vector2Int nextIndex)
        {
            if (prevIndex != Index.Value)
                return;
            Index.Value = nextIndex;
        }
    }
}