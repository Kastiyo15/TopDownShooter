// Script from 'PitiIT' YouTube
// Creates a static function that can be called anywhere which gets the mouse position in world space
using UnityEngine;

public class MouseUtils
{
    public static Vector2 GetMousePosition2d()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
