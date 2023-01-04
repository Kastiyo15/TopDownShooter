using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _player;
    [SerializeField] private float _range;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            Vector3 mousePos = MouseUtils.GetMousePosition2d();
            Vector3 targetPos = (_player.position + mousePos) / 2f; // to find the midpoint

            targetPos.x = Mathf.Clamp(targetPos.x, -_range + _player.position.x, _range + _player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -_range + _player.position.y, _range + _player.position.y);
            targetPos.z = -10f;

            this.transform.position = targetPos;
        }
        return;
    }
}
