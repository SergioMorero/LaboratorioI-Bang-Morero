using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    private float initialPosition;
    private int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position.x;
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= initialPosition - 3) direction = 1;
        if (transform.position.x >= initialPosition + 3) direction = -1;
        transform.Translate(new Vector3(5 * direction * Time.deltaTime, 0, 0));
    }
}
