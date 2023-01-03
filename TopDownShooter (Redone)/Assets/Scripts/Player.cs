using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _movementSpeed;

    private Vector2 _moveVelocity;
    private Vector2 _mousePos;


    // Start is called before the first frame update
    private void Start()
    {
        
    }


    // Update is called once per frame
    private void Update()
    {
        // Work out player movement part 1
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveVelocity = moveInput.normalized;

        // Rotate player towards crosshair part 1
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }


    // Update physics
    private void FixedUpdate()
    {
        // Player movement part 2
        _rb.MovePosition(_rb.position + _moveVelocity * _movementSpeed * Time.fixedDeltaTime);

        // Rotate player part 2
        Vector2 lookDir = (_mousePos - _rb.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = angle;
    }
}
