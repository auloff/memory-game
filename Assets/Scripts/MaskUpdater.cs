using UnityEngine;
using MemoryGame.Card;

namespace MemoryGame.Utilities
{
    [RequireComponent(typeof(SpriteMask))]
    public class MaskUpdater : MonoBehaviour
    {
        private BaseCard<BaseCardData> _card;

        private void Update()
        {
            _card = GetComponentInParent<BaseCard<BaseCardData>>();
            if (_card != null)
            {
                transform.localScale = _card.CardSize;
                Destroy(this);
            }
        }
    }
}