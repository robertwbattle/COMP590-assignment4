using UnityEngine;
using UnityEngine.InputSystem;


// AI generated script for emulating head movement in the editor with the keyboard
public class EditorHeadEmulator : MonoBehaviour
{
    public float rotationSpeed = 60f;

    void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current == null) return;

        float horizontal = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;

        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime, Space.World);
#endif
    }
}