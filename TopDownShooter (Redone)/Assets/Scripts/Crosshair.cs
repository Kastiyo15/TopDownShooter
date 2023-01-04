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
            Vector3 mousePos = MouseUtils.GetMousePosition2d();

            // Rotate the crosshair for fun
            transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            this.transform.position = mousePos;
        }
        return;
    }
}
