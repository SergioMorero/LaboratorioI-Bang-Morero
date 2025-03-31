using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomWait : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitRandomTime());
    }

    IEnumerator WaitRandomTime()
    {
        // Generate a random integer between 1 and 5 (inclusive)
        int randomTime = Random.Range(1, 6);
        Debug.Log("Waiting for: " + randomTime + " seconds");

        // Wait for the random time
        yield return new WaitForSeconds(randomTime);

        Debug.Log("Done waiting!");
        SceneManager.LoadSceneAsync(2);

    }
}
