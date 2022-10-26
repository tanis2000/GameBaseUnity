using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace GameBase.Leaderboards
{
    [Serializable]
    public class LeaderboardData
    {
        public bool success;
        public string reason;
        public LeaderboardMember[] members;
    }

    [Serializable]
    public class LeaderboardMember
    {
        public string publicID;
        public int score;
        public int rank;
        public int previousRank;
        public int expireAt;

        public static LeaderboardMember CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<LeaderboardMember>(jsonString);
        }
    }
    
    [Serializable]
    public class LeaderboardSubmitData
    {
        public bool success;
        public LeaderboardMember member;
    }
    
    public class ScoreSystem: MonoBehaviour
    {
        public Action OnLoaded;
        public Action OnSaved;

        public string GameName;
        public int PageSize = 5;
        
        private static ScoreSystem instance;

        private const string BaseURL = "https://podium.altralogica.it";

        private LeaderboardData data;

        public static ScoreSystem Instance()
        {
            if (instance == null)
            {
                instance = (ScoreSystem)FindObjectOfType(typeof(ScoreSystem));
                if (instance == null) 
                {
                    var container = new GameObject
                    {
                        name = typeof(ScoreSystem).ToString()
                    };
                    instance = (ScoreSystem)container.AddComponent(typeof(ScoreSystem));
                }
            }
            return instance;
        }
        
        public bool EndReached { get; private set; }

        private void OnEnable()
        {
            instance = this;
        }

        public void LoadLeaderboards(int page)
        {
            StartCoroutine(LoadLeaderboardsAsync(page));
        }

        private IEnumerator LoadLeaderboardsAsync(int page)
        {
            var req = UnityWebRequest.Get($"{BaseURL}/l/{GameName}/top/{page}?pageSize={PageSize}");
            yield return req.SendWebRequest();
            if (string.IsNullOrEmpty(req.error))
            {
                data = JsonUtility.FromJson<LeaderboardData>(req.downloadHandler.text);
                if (data.success)
                {
                    EndReached = data.members.Length < PageSize;
                    OnLoaded?.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"ScoreSystem error: ${req.error}");
            }
        }

        public LeaderboardData GetData()
        {
            return data;
        }

        public void CancelLeaderboards()
        {
            StopAllCoroutines();
        }

        public void SaveScore(string playerId, int score)
        {
            StartCoroutine(DoSaveScore(playerId, score));
        }

        public IEnumerator DoSaveScore(string playerId, int score)
        {
            var body = $"{{\"score\":{score}}}";
            var req = UnityWebRequest.Put($"{BaseURL}/l/{GameName}/members/{playerId}/score", body);
            yield return req.SendWebRequest();
            if (string.IsNullOrEmpty(req.error))
            {
                var d = JsonUtility.FromJson<LeaderboardData>(req.downloadHandler.text);
                if (d.success)
                {
                    OnSaved?.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"ScoreSystem error: ${req.error}");
            }
        }
    }
}