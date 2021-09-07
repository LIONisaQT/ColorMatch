using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class FirebaseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [DllImport("__Internal")]
    public static extern void GetJSON(string path, string objectName, string callback, string fallback);

    [DllImport("__Internal")]
    public static extern void SendJSON(string path, string objectName, string payload, string callback, string fallback);

    private string _data;

    public string GetPlayerData()
    {
        GetJSON("playerData", gameObject.name, "OnRequestSuccess", "OnRequestFail");
        return _data;
    }

    public void SendPlayerData(string payload)
    {
        SendJSON("playerData", gameObject.name, payload, "OnRequestSuccess", "OnRequestFail");
    }

    public void OnRequestSuccess(string data)
    {
        _text.color = Color.green;
        _text.text = data;

        _data = data;
    }

    private void OnRequestFail(string error)
    {
        _text.color = Color.red;
        _text.text = error;
    }
}
