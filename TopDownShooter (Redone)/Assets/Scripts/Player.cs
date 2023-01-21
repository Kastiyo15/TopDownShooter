using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IKnockable
{
    [Header("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private AbilitiesController _abilityHolder;
    [HideInInspector] public Rigidbody2D Rb;

    [HideInInspector] public float MovementSpeed;
    public float NormalMovementSpeed;
    [HideInInspector] public Vector2 MoveVelocity;
    private Vector2 _mousePos, _knockDirection, _lookDir, moveInlineWithMouse;
    private bool IsKnocked = false;
    private float _knockForce = 0;
    [SerializeField][Range(0f, 10f)] private float _knockForceMultiplier;

    [Header("Direction Bools")]
    private bool IsFacingNorth, IsFacingEast, IsFacingSouth, IsFacingWest;

    [Header("Ability Variables")]
    public bool IsDashing = false;
    public int DashDamage = 100;


    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        MovementSpeed = NormalMovementSpeed;
        _abilityHolder = gameObject.GetComponent<AbilitiesController>();
    }


    // Update is called once per frame
    private void Update()
    {
        //CheckDirectionFacing();
        GetPlayerMovement();
        GetPlayerRotation();
    }


/*     private void CheckDirectionFacing()
    {
        if (transform.rotation.eulerAngles.z > 240 && transform.rotation.eulerAngles.z < 300)
        {
            IsFacingEast = true;
            IsFacingSouth = false;
            IsFacingWest = false;
            IsFacingNorth = false;
        }
        else if (transform.rotation.eulerAngles.z > 120 && transform.rotation.eulerAngles.z < 240)
        {
            IsFacingSouth = true;
            IsFacingEast = false;
            IsFacingWest = false;
            IsFacingNorth = false;
        }
        else if (transform.rotation.eulerAngles.z > 60 && transform.rotation.eulerAngles.z < 120)
        {
            IsFacingWest = true;
            IsFacingEast = false;
            IsFacingSouth = false;
            IsFacingNorth = false;
        }
        else
        {
            IsFacingNorth = true;
            IsFacingEast = false;
            IsFacingSouth = false;
            IsFacingWest = false;
        }
    } */


    // Update the new input vectors
    private void GetPlayerMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveVelocity = moveInput.normalized;
    }


    // Update where the mouse position is
    private void GetPlayerRotation()
    {
        // Rotate player towards crosshair
        _mousePos = MouseUtils.GetMousePosition2d();
        _lookDir = (_mousePos - (Vector2)transform.position).normalized;
        transform.up = _lookDir.normalized;
    }


    // Update physics
    private void FixedUpdate()
    {
        /*         Vector2 _lookDir = (_mousePos - Rb.position);
                float angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg - 90f;
                Rb.rotation = angle; */

        if (!IsKnocked)
        {
            //Rb.MovePosition(Rb.position + MoveVelocity * MovementSpeed * Time.fixedDeltaTime);
            Rb.velocity += (MoveVelocity * MovementSpeed * Time.fixedDeltaTime);
            #region NEW CHARACTER MOVEMENT
            /* if (IsFacingNorth)  // looking upwards
            {
                if (MoveVelocity.x != 0)
                {
                    moveInlineWithMouse = MoveVelocity;
                }
                else
                {
                    moveInlineWithMouse = (Vector2)transform.up * MoveVelocity.y;
                }
            }
            else if (IsFacingSouth) // looking downwards
            {
                if (MoveVelocity.x != 0)
                {
                    moveInlineWithMouse = MoveVelocity;
                }
                else
                {
                    moveInlineWithMouse = (Vector2)transform.up * -MoveVelocity.y;
                }
            }
            else if (IsFacingEast)   // looking Right
            {
                if (MoveVelocity.y != 0)
                {
                    moveInlineWithMouse = MoveVelocity;
                }
                else
                {
                    moveInlineWithMouse = (Vector2)transform.up * MoveVelocity.x;
                }
            }
            else if (IsFacingWest)  // looking Left
            {
                if (MoveVelocity.y != 0)
                {
                    moveInlineWithMouse = MoveVelocity;
                }
                else
                {
                    moveInlineWithMouse = (Vector2)transform.up * -MoveVelocity.x;
                }
            }
            //var currentVelocity = Rb.velocity;
            Rb.velocity += (moveInlineWithMouse).normalized * MovementSpeed * Time.fixedDeltaTime;
            //Rb.velocity = Vector2.Lerp(currentVelocity, (moveInlineWithMouse).normalized * MovementSpeed, Time.fixedDeltaTime); */
            #endregion
        }
        else if (IsKnocked)
        {
            //Rb.MovePosition(Rb.position - _knockDirection * _knockForce * Time.fixedDeltaTime);
            Rb.velocity -= _knockDirection * _knockForce * Time.fixedDeltaTime;
            IsKnocked = false;
        }
    }

/* 
    public void MoveParallel()
    {
        if (MoveVelocity.x != 0)
        {
            moveInlineWithMouse = MoveVelocity;
        }
    } */


    // Knock back the Player when shooting
    public void KnockedBack(Vector2 direction, float knockForce)
    {
        IsKnocked = true;
        _knockForce = knockForce;
        _knockDirection = direction;
    }


    // When colliding with anything (Enemies)
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        if (IsDashing && hitInfo.gameObject.CompareTag("Enemy"))
        {
            // Interface: Will minus damage from health of target hit
            if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
            {
                health.Damage(DashDamage);

                // Interface: Will knock back object
                var hittable = hitInfo.gameObject.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.OnHit(DashDamage);
                }

                // Interface: Will knock back object
                var knockable = hitInfo.gameObject.GetComponent<IKnockable>();
                if (knockable != null)
                {
                    knockable.KnockedBack(MoveVelocity, MovementSpeed);
                }
            }
        }
    }
}
