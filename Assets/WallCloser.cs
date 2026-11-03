using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class WallCloser : MonoBehaviour
{
    public enum WallDirection { PositiveX, NegativeX, PositiveZ, NegativeZ }

    [Header("References")]
    public Transform center;              // player rig / camera parent

    [Header("Placement")]
    public WallDirection direction;       // set per wall in the Inspector

    [Header("Movement")]
    public float closeSpeed = 1f;       // units per second inward
    public float maxDistance = 20f;
    public float loseDistance = 2f;

    Rigidbody rb;
    Vector3 outwardDir;
    float currentDistance;
    float homeY;
    bool hasLost;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        homeY = transform.position.y;

        if (center == null)
        {
            Debug.LogError($"{name}: center not assigned — wall will not move.");
            enabled = false;
            return;
        }

        switch (direction)
        {
            case WallDirection.PositiveX: outwardDir = Vector3.right;   break;
            case WallDirection.NegativeX: outwardDir = Vector3.left;    break;
            case WallDirection.PositiveZ: outwardDir = Vector3.forward; break;
            case WallDirection.NegativeZ: outwardDir = Vector3.back;    break;
        }

        currentDistance = Vector3.Dot(transform.position - center.position, outwardDir);
    }

    void FixedUpdate()
    {
        if (hasLost) return;

        currentDistance = Mathf.Min(
            currentDistance - closeSpeed * Time.fixedDeltaTime,
            maxDistance);

        if (currentDistance <= loseDistance)
        {
            currentDistance = loseDistance;
            hasLost = true;
            Debug.Log($"{name} reached the player");
            SceneManager.LoadScene(0); // back to title screen
            return;
        }

        Vector3 target = center.position + outwardDir * currentDistance;
        target.y = homeY;
        rb.MovePosition(target);
    }

    [Header("Impact")]
    public float minPush = 0.3f;   // units of retreat from a tap
    public float maxPush = 2f;   // units from a full charge

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        float charge = 0f;
        BallPrefab ball = collision.gameObject.GetComponent<BallPrefab>();
        if (ball != null) charge = ball.chargeAmount;

        currentDistance = Mathf.Min(
            currentDistance + Mathf.Lerp(minPush, maxPush, charge),
            maxDistance);

        Destroy(collision.gameObject);
    }
}