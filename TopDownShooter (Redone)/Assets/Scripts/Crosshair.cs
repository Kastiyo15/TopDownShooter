using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private Animator _animator;
    [SerializeField] private Material _flashMaterial;
    private Material _defaultMaterial;
    private Vector3 _mousePos;
    private RaycastHit2D _hit;
    private int _layerMask;


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

    /*     private IEnumerator DetectEnemy()
        {
            while (true)
            {
                yield return null;

                _layerMask = LayerMask.GetMask("Enemy");

                // Raycast from crosshair to game
                _hit = Physics2D.Raycast(_mousePos, Vector2.zero, _layerMask);



                if (_hit.collider != null)
                {
                    _animator.SetTrigger("OnHover");
                    _hit.collider.gameObject.GetComponent<SpriteRenderer>().material = _flashMaterial;
                    Debug.Log("Target Position: " + _hit.collider.gameObject.transform.position + _hit.collider.name);
                }
                else
                {
                    _animator.SetTrigger("Normal");
                    _hit.collider.gameObject.GetComponent<SpriteRenderer>().material = _defaultMaterial;
                    Debug.Log("No Target");
                }
            }
        } */


    private bool DetectEnemy()
    {
        //Check if the laser hit something
        RaycastHit2D hit = Physics2D.Raycast(_mousePos, Vector2.zero, _layerMask);

        //If the object is not null
        if (hit.collider != null)
        {
            _animator.SetTrigger("OnHover");
        }
        else
        {
            _animator.SetTrigger("Normal");
        }
        
        //If the ray hits Enemy return true, false otherwise
        return hit.collider != null;
    }
}
