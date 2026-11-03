using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPrefab : MonoBehaviour
{

    public float chargeAmount;   // 0–1, set at spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Destroy(gameObject, 8f);

    // Update is called once per frame
    void Update()
    {
        // destroy balls if they fall below a certain height
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
