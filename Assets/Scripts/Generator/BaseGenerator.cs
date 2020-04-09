using System;
using UnityEngine;
using MemoryGame.Card;

namespace MemoryGame.Generator
{
    public abstract class BaseGenerator : MonoBehaviour
    {
        public abstract event Action AllDone;
        public abstract BaseCard<BaseCardData>[] GameCards { get;}
        public abstract void Generate();
    }
}