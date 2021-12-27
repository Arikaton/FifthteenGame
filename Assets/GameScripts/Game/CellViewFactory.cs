using GameScripts.Pool;
using UnityEngine;

namespace GameScripts.Game
{
    public class CellViewFactory
    {
        private SimpleObjectPool _cellPool;
        private RectTransform _fieldContainer;

        public CellViewFactory(SimpleObjectPool cellPool, RectTransform fieldContainer)
        {
            _fieldContainer = fieldContainer;
            _cellPool = cellPool;
        }
        
        public CellView Create(CellViewModel cellViewModel, Vector2Int fieldSize)
        {
            var cellGo = _cellPool.Spawn();
            var cellView = cellGo.GetComponent<CellView>();
            cellView.transform.SetParent(_fieldContainer);
            cellView._rectTransform.anchorMax = Vector2.up;
            cellView._rectTransform.anchorMin = Vector2.up;
            cellView._rectTransform.sizeDelta = new Vector2(
                _fieldContainer.rect.width / fieldSize.x,
                _fieldContainer.rect.height / fieldSize.y);
            cellView.transform.localScale = Vector3.one;
            cellView.Initialize(cellViewModel);
            return cellView;
        }
    }
}