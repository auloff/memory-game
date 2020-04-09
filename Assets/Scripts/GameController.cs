using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryGame.Generator;
using MemoryGame.Card;
using System;

namespace MemoryGame
{
    [RequireComponent(typeof(BaseGenerator))]
    public class GameController : MonoBehaviour
    {
        public float startDelay, flipDelay;
        public event Action<int> ScoreChanged;

        [SerializeField]
        private int _matchPairScore;
        public int MatchPairScore 
        { 
            get => _matchPairScore; 
            private set
            {
                _matchPairScore = value;
                ScoreChanged?.Invoke(_matchPairScore);
            } 
        }

        private BaseCard<BaseCardData> _matchedCard;
        private BaseGenerator _generator;
        [SerializeField]
        private int _currentActiveCard = 0;
        [SerializeField]
        private bool _pause;

        private void Awake()
        {
            _pause = true;
            _generator = GetComponent<BaseGenerator>();
            _generator.AllDone += StartGame;
        }

        private void StartGame()
        {
            _pause = true;
            _generator.Generate();
            foreach (var item in _generator.GameCards)
            {
                item.NeedFlip += FlipingCard;
                if(item.IsFrontSide)
                {
                    item.Flip();
                }
            }
            OpenAll();
        }

        private void OpenAll()
        {
            foreach (var item in _generator.GameCards)
            {
                StartCoroutine(OpenCard(item));
            }
        }
        private IEnumerator OpenCard(BaseCard<BaseCardData> obj)
        {
            yield return new WaitForSeconds(flipDelay);
            obj.StayOpened();
            yield return new WaitForSeconds(startDelay);
            obj.StayClosed();
            _pause = false;
        }

        private void FlipingCard(BaseCard<BaseCardData> obj)
        {
            if (obj.IsOpen || obj.IsFrontSide || _pause) return;

            obj.Flip();
            if (_currentActiveCard == 0)
            {
                _matchedCard = obj;
                _currentActiveCard++;
            }
            else if(_currentActiveCard == 1 && obj != _matchedCard)
            {
                if (_matchedCard.Match(obj))
                {
                    MatchPairScore+=1;
                    obj.StayOpened();
                    _matchedCard.StayOpened();
                }
                else
                {
                    StartCoroutine(CloseWithDelay(obj, _matchedCard));
                }
                _currentActiveCard = 0;
            }

            CheckCards();
        }

        private IEnumerator CloseWithDelay(BaseCard<BaseCardData> obj1, BaseCard<BaseCardData> obj2)
        {
            _pause = true;
            yield return new WaitForSeconds(flipDelay);
            obj1.Flip();
            obj2.Flip();
            _pause = false;
        }

        private void CheckCards()
        {
            bool isAllOpened = true;
            foreach (var item in _generator.GameCards)
            {
                if (!item.IsOpen) isAllOpened = false;
            }

            if (isAllOpened) StartGame();
        }

    }
}
