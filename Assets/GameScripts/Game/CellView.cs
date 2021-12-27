using DG.Tweening;
using GameScripts.Pool;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class CellView : MonoBehaviour, IPointerClickHandler, IPoolable
    {
        [SerializeField] private TextMeshProUGUI numberText;
        [HideInInspector] public RectTransform _rectTransform;
        
        private CellViewModel _cellViewModel;
        private CompositeDisposable _disposables;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(CellViewModel cellViewModel)
        {
            _disposables = new CompositeDisposable();
            _cellViewModel = cellViewModel;
            _cellViewModel.Index.SkipLatestValueOnSubscribe().Subscribe(OnIndexChanged).AddTo(_disposables);
            _cellViewModel.Number.Subscribe(UpdateCellNumber).AddTo(_disposables);
            Move(_cellViewModel.Index.Value, false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _cellViewModel.MoveCell();
        }

        private void UpdateCellNumber(int number)
        {
            numberText.text = number.ToString();
        }

        private void OnIndexChanged(Vector2Int newIndex)
        {
            Move(newIndex);
        }

        private void Move(Vector2Int newIndex, bool animated = true)
        {
            var movePosition = DefineMovePosition(newIndex);
            if (animated)
                _rectTransform.DOAnchorPos(movePosition, 0.3f);
            else
                _rectTransform.anchoredPosition = movePosition;
        }

        private Vector2 DefineMovePosition(Vector2Int newIndex)
        {
            var cellSize = _rectTransform.sizeDelta;
            var newX = cellSize.x / 2 + cellSize.x * newIndex.x;
            var newY = -cellSize.y / 2 - cellSize.y * newIndex.y;
            return new Vector2(newX, newY);
        }

        public void ResetState()
        {
            _cellViewModel = null;
            _disposables?.Dispose();
        }
    }
}