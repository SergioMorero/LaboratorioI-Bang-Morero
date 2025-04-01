using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

    [SerializeField] private int scene;

    public void backToMainMenu() {
        SceneManager.LoadSceneAsync(scene);
    }
}
