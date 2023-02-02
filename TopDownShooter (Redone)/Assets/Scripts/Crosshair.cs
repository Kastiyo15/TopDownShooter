using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _layermask;

    private Vector3 _mousePos;
    private RaycastHit2D _hit;


    private void Start()
    {
        StartCoroutine("DetectEnemy");
    }


    // Update is called once per frame
    private void Update()
    {
        // Move and rotate the mouse
        #region  MOVE AND ROTATE MOUSE
        _mousePos = MouseUtils.GetMousePosition2d();

        // Rotate the crosshair for fun
        //_crosshairImage.transform.Rotate(Vector3.forward * 0.1f);

        transform.position = _mousePos;
        #endregion

        if (DetectEnemy())
        {
            _animator.SetTrigger("OnHover");
        }
        else
        {
            _animator.SetTrigger("Normal");
        }
    }


    private bool DetectEnemy()
    {
        //Check if the laser hit something
        RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero, Vector3.Distance(_mousePos, Vector2.zero), _layermask);

        //If the object is not null
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void ShowCrosshair()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        gameObject.SetActive(true);
    }


    public void HideCrosshair()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(false);
    }
}
