using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector3 _mousePos;
    private RaycastHit2D _hit;


    private void Start()
    {
        StartCoroutine("DetectEnemy");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.GameIsPaused)
        {
            // Move and rotate the mouse
            #region  MOVE AND ROTATE MOUSE
            _mousePos = MouseUtils.GetMousePosition2d();

            // Rotate the crosshair for fun
            transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            this.transform.position = _mousePos;
            #endregion

            if (DetectEnemy())
            {
                Debug.Log("Target Position: ");
            }
        }
        return;
    }


    private bool DetectEnemy()
    {
        int _layerMask = LayerMask.GetMask("Enemy");

        //Check if the laser hit something
        RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero, _layerMask);

        //If the object is not null
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            _animator.SetTrigger("OnHover");
            return true;
        }
        else
        {
            _animator.SetTrigger("Normal");
            return false;
        }
    }
}
