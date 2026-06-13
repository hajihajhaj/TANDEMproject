using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerScrollRect : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float speed = 4f;

    void Update()
    {
        if (Gamepad.current == null) return;

        float y = Gamepad.current.leftStick.y.ReadValue();

        if (Mathf.Abs(y) > 0.1f)
        {
            scrollRect.verticalNormalizedPosition += y * speed * Time.deltaTime;
        }
    }
}