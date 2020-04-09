using System;
using UnityEngine;
using MemoryGame.Core.Interfaces;

namespace MemoryGame.Card
{
    public abstract class BaseCard<T> : MonoBehaviour, IFlipable where T : BaseCardData, new()
    {
        [SerializeField]
        protected bool _isFrontSide = false;
        public bool IsFrontSide { get => _isFrontSide; }
        [SerializeField]
        protected bool _isOpen = false;
        public bool IsOpen { get => _isOpen; }
        [SerializeField]
        protected Vector2 _cardSize = Vector2.zero, _textureOffset = Vector2.zero;
        public Vector2 CardSize { get => _cardSize; }

        public virtual T CardData { get; set; }

        public virtual event Action<BaseCard<T>> Fliped;
        public virtual event Action<BaseCard<T>> NeedFlip;

        public virtual void Flip()
        {
            if (!_isOpen)
            {
                Fliped?.Invoke(this);
            }
        }

        public virtual void NeedFliping()
        {
            NeedFlip?.Invoke(this);
        }

        public virtual void StayOpened()
        {
            _isOpen = true;
        }

        public virtual void StayClosed()
        {
            _isOpen = false;
        }

        public abstract void UpdateVisual();

        public virtual bool Match(BaseCard<T> obj)
        {
            return this.CardData.UniqueName == obj.CardData.UniqueName;
        }
    }
}