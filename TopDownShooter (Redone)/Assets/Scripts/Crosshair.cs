using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _player;
    [SerializeField] private float _range;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPos = ((Vector2)_player.position + mousePos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -_range + _player.position.x, _range + _player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -_range + _player.position.y, _range + _player.position.y);

            // Rotate the crosshair for fun
            transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            this.transform.position = targetPos;
        }
        return;
    }
}
