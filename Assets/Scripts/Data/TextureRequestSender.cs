using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using MemoryGame.Card;

namespace MemoryGame.Services
{
    public class TextureRequestSender : RequestSender<JSONCardData[], BaseCardData[]>
    {
        private int _activity = 0;
        private BaseCardData[] _baseCardDatas;

        public override void GetData(JSONCardData[] cards, Action<BaseCardData[]> complete)
        {
            _baseCardDatas = new BaseCardData[cards.Length];
            for (int i = 0; i < _baseCardDatas.Length; i++)
            {
                _baseCardDatas[i] = new BaseCardData();
                StartCoroutine(LoadCard(i, cards[i], complete));
            }
        }

        private IEnumerator LoadCard(int i, JSONCardData data, Action<BaseCardData[]> complete)
        {
            _activity++;
            yield return LoadFront(i, data.FrontSource);
            yield return LoadBack(i, data.BackSource);

            _baseCardDatas[i].UniqueName = data.ID;
            _activity--;

            if (_activity == 0)
                complete(_baseCardDatas);
        }

        private IEnumerator LoadBack(int i, string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                _baseCardDatas[i].CardBack = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
            }

            request.Dispose();
        }
        private IEnumerator LoadFront(int i, string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                _baseCardDatas[i].CardFront = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
            }

            request.Dispose();
        }
    }
}