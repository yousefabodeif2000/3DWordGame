using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class API : MonoBehaviour
{
    private const string API_URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";
    public static API Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {

    }

    /// <summary>
    /// Call this to check if a word exists in the dictionary.
    /// </summary>
    /// <param name="word">The word to check</param>
    public static void CheckWord(string word)
    {
        API.Instance.StartCoroutine(Instance.CheckWordCoroutine(word));
    }

    private IEnumerator CheckWordCoroutine(string word)
    {
        string url = API_URL + word.ToLower();

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                Debug.Log($"{word} is a valid word.");
                APIEvents.ExecuteWordValid(word);
                Debug.Log("Definition (raw): " + request.downloadHandler.text);
                // Optional: parse the JSON here to extract definitions
            }
            else if(request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"{word} is NOT a valid word. Error: {request.error}");
                APIEvents.ExecuteWordInvalid(word);
            }
            else
            {
                Debug.LogWarning($"Connection Error: {request.error}");
            }
        }
    }
}

public static class  APIEvents
{
    static public event Action<string> OnWordValid;
    static public event Action<string> OnWordInvalid;

    static public void ExecuteWordValid(string word)
    {
        OnWordValid?.Invoke(word);
    }
    static public void ExecuteWordInvalid(string word)
    {
        OnWordInvalid?.Invoke(word);
    }
}