using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace MemoryGame.Services
{
    public struct JSONCardData
    {
        public string ID;
        public string FrontSource;
        public string BackSource;
    }

    public class JSONRequestSender : RequestSender<string, JSONCardData[]>
    {
        public override void GetData(string url, Action<JSONCardData[]> complete)
        {
            StartCoroutine(SendRequest(url, complete));
        }

        private IEnumerator SendRequest(string url, Action<JSONCardData[]> complete)
        {
            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                complete.Invoke(Parse(request.downloadHandler.text));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
            }

            request.Dispose();
        }

        private JSONCardData[] Parse(string response)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, JSONCardData[]>>(response);
            if (data.ContainsKey("Cards"))
                return data["Cards"];
            else throw new Exception("Cards does not exist");
        }
    }
}