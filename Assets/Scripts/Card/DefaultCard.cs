using UnityEngine;
using UnityEngine.EventSystems;

namespace MemoryGame.Card
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DefaultCard : BaseCard<BaseCardData>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Color32 _hoverColor = Color.gray, _startedColor = Color.white;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;
        [SerializeField]
        private Sprite _spriteBack, _spriteFront;

        void Awake()
        {
            CardData = new BaseCardData();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = gameObject.AddComponent<BoxCollider2D>();
        }

        void Start()
        {
            _collider.size = _cardSize;
            FlipToBack();
        }

        public override void Flip()
        {
            if (!_isFrontSide)
            {
                FlipToFront();
            }
            else
            {
                FlipToBack();
            }
            base.Flip();
        }

        private void FlipToFront()
        {
            if (_spriteFront == null) return;

            _spriteRenderer.sprite = _spriteFront;
            _isFrontSide = true;
        }

        private void FlipToBack()
        {
            if (_spriteBack == null) return;

            _spriteRenderer.sprite = _spriteBack;
            _isFrontSide = false;
        }

        public override void StayOpened()
        {
            if (!_isFrontSide)
            {
                FlipToFront();
            }

            _spriteRenderer.color = _hoverColor;
            base.StayOpened();
        }

        public override void StayClosed()
        {
            if (_isFrontSide)
            {
                FlipToBack();
            }

            _spriteRenderer.color = _startedColor;
            base.StayClosed();
        }

        public override void UpdateVisual()
        {
            if (CardData.CardBack != null)
            {
                _spriteBack = Sprite.Create(base.CardData.CardBack, new Rect(0, 0, base.CardData.CardBack.width, base.CardData.CardBack.height), _textureOffset);
            }
            if (CardData.CardFront != null)
            {
                _spriteFront = Sprite.Create(base.CardData.CardFront, new Rect(0, 0, base.CardData.CardFront.width, base.CardData.CardFront.height), _textureOffset);
            }
            if (_isFrontSide)
                FlipToFront();
            else
                FlipToBack();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            base.NeedFliping();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isOpen)
                _spriteRenderer.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isOpen)
                _spriteRenderer.color = _startedColor;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1,0,0,0.5f);
            Gizmos.DrawCube(this.transform.position, _cardSize);
        }
    }
}