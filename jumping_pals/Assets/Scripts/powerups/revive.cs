using System.Security.Cryptography;
using UnityEngine;

public class Revive : Monobehaviour, PowerUp {

    [SerializeFiled] private Animator animator;
    private Transform transform;

    void start() {
        transform = GetComponent<Transform>();
    }

    public void execute(localPlayer player) {
        // Iterate through all players: There are only two, and one must be alive
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            if (!player.GetComponent<LocalPlayer>().alive) {
                player.revive(transform.position);
            }
        }
    }
}
