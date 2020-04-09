using UnityEngine;
using System;
using System.Collections;

namespace MemoryGame.Services
{
    public abstract class RequestSender<T, TT>: MonoBehaviour
    {
        public abstract void GetData(T url, Action<TT> complete);
    }
}