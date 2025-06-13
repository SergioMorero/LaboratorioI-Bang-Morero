using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    public TMP_Text roomCodeText;
    private string roomId;
    private string host;
    private System.Action<string> onJoinCallback;

    public void Setup(string roomId, string host, System.Action<string> onJoin)
    {
        this.roomId = roomId;
        this.host = host;
        this.onJoinCallback = onJoin;
        roomCodeText.text = "Join " + host + "'s game";

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onJoinCallback?.Invoke(roomId);
    }
}
