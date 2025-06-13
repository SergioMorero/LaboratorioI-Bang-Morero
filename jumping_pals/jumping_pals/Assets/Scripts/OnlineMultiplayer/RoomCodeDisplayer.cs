using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class RoomCodeDisplayer : NetworkBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Room Code: " + PlayerPrefs.GetString("roomId");
    }
}
