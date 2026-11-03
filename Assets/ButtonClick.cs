using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public BallPrefab ballPrefab;

    [Header("Power Settings")]
    public float maxBuildUpTime = 1.5f;
    public float minSpeed = 4f;    // meters per second
    public float maxSpeed = 14f;

    [Header("Spawn")]
    public float spawnDistance = 1.5f;
    public float ballLifetime = 8f;

    float touchStartTime = -1f;
    bool isCharging = false;

    void Update()
    {
        bool pressedThisFrame =
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        bool held =
            OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) ||
            OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        if (pressedThisFrame && !isCharging)
        {
            touchStartTime = Time.time;
            isCharging = true;
        }
        else if (isCharging && !held)
        {
            if (touchStartTime >= 0f)
                Fire();

            touchStartTime = -1f;
            isCharging = false;
        }
    }

    void Fire()
    {
        float heldTime = Time.time - touchStartTime;
        float charge = Mathf.Clamp01(heldTime / maxBuildUpTime);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, charge);

        Debug.Log($"Released after {heldTime:F2}s — speed {speed:F1} m/s");

        Transform cam = Camera.main.transform;

        BallPrefab ball = Instantiate(ballPrefab);
        ball.transform.position = cam.position + cam.forward * spawnDistance;
        ball.tag = "Ball";

        ball.chargeAmount = charge;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(cam.forward * speed, ForceMode.VelocityChange);

        Destroy(ball.gameObject, ballLifetime);
    }
}