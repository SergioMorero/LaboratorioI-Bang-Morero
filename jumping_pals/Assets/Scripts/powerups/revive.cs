using System.Security.Cryptography;
using UnityEngine;

public class Revive : MonoBehaviour, PowerUp {

    [SerializeField] private Animator animator;

    void Start() {

    }

    public void execute(LocalPlayer player) {
        // Iterate through all players: There are only two, and one must be alive
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
            if (!p.GetComponent<LocalPlayer>().alive) {
                player.revivePlayer(transform.position);
            }
        }
    }
}
