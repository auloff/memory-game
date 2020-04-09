using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using MemoryGame.Card;
using MemoryGame.Services;

namespace MemoryGame.Generator
{
    public class CardGenerator : BaseGenerator
    {
        public string url;
        public GameObject objectWithCards;
        public override event Action AllDone;

        private RequestSender<string, JSONCardData[]> _parseJSON;
        private RequestSender<JSONCardData[], BaseCardData[]> _parseCard;

        private BaseCard<BaseCardData>[] _gameCards;
        private BaseCardData[] _set;

        public override BaseCard<BaseCardData>[] GameCards { get => _gameCards; }

        private void Awake()
        {
            _parseJSON = GetComponent<RequestSender<string, JSONCardData[]>>();
            _parseCard = GetComponent<RequestSender<JSONCardData[], BaseCardData[]>>();
        }

        private void Start()
        {
            _parseJSON?.GetData(url, jsonCardData =>
            {
                _parseCard?.GetData(jsonCardData, parsedSet =>
                    {
                        _set = parsedSet;
                        FillCards();
                        AllDone?.Invoke();
                    });
            });
        }

        protected void FillCards()
        {
            if (objectWithCards == null)
            {
                Debug.LogError("Cards does not exist!");
                return;
            }

            int size = objectWithCards.transform.childCount > _set.Length * 2 ? _set.Length * 2 : objectWithCards.transform.childCount;
            _gameCards = new BaseCard<BaseCardData>[size];

            for (int i = 0; i < objectWithCards.transform.childCount; i++)
            {
                if (i < _set.Length * 2)
                {
                    BaseCard<BaseCardData> tmp = objectWithCards.transform.GetChild(i).GetComponent<BaseCard<BaseCardData>>();
                    if (tmp != null)
                    {
                        tmp.CardData.CardBack = _set?.First().CardBack;
                        tmp.UpdateVisual();
                        tmp.StayClosed();
                        _gameCards[i] = tmp;
                    }
                }
            }
        }

        public override void Generate()
        {
            HashSet<int> values = new HashSet<int>();
            
            foreach (var item in _set)
            {
                for (int i = 0; i < 2; i++)
                {
                    int rIndex;
                    do
                    {
                        rIndex = (int)UnityEngine.Random.Range(0, _gameCards.Length);
                    } while (values.Contains(rIndex));

                    values.Add(rIndex);
                    _gameCards[rIndex].CardData.UniqueName = item.UniqueName;
                    _gameCards[rIndex].CardData.CardFront = item.CardFront;
                    _gameCards[rIndex].UpdateVisual();
                }
            }
        }
    }
}