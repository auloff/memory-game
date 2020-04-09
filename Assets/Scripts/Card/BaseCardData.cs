using UnityEngine;

namespace MemoryGame.Card
{
    public class BaseCardData
    {
        public virtual string UniqueName { get; set; }
        public virtual Texture2D CardBack { get; set; }
        public virtual Texture2D CardFront { get; set; }
    }
}