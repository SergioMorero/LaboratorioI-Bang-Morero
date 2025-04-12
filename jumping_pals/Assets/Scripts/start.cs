using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public void StartGame(int scene) {
        SceneManager.LoadScene(scene);
    }
}
