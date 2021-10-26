using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region Start Variables
	[Header("Collision")]
    [SerializeField] private float groundLength;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 colliderOffset;
    public bool isGrounded = false;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite sPlayerSprite;
    private bool isStunned;
    [SerializeField] private PhysicsMaterial2D ground;
    [SerializeField] private float frictionValue = 10f;
    [SerializeField] private float knockback = 10f;


    [Header("Shoot variables")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    Vector2 mousePos;
    public float maxAmmo = 2;
    public float ammo = 0;
    public bool shootInput;

    [Header("Velocity variables")]
    [SerializeField] private float fallMultiplier = 5f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float force = 10;
    [SerializeField] private float maxSpeedX = 600;
    [SerializeField] private float maxSpeedY = 600;
    private bool pushedByWind;


    public bool gameComplete = false;

	#endregion
	private void Awake()
	{
        ground.friction = frictionValue;
	}
	void Update()
    {
        if (((Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer))) && !pushedByWind)
            isGrounded = true;
        else isGrounded = false;

        if (isGrounded && rb.velocity.y < .1f && rb.velocity.y > -.1f && !pushedByWind)
            ammo = maxAmmo;

        if (isStunned && isGrounded)
		{
            if(Mathf.Abs(rb.velocity.x) < 1f)
			{
                isStunned = false;
                ground.friction = frictionValue;
                sr.sprite = playerSprite;
            }
        }

        AimNShoot();
    }

    private void FixedUpdate()
	{
        ModifyPhysics();
    }

    private void AimNShoot()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 forceDir = mousePos - rb.position;

        if (Input.GetButtonDown("Fire1") && ammo > 0 && !isStunned)
        {
            shootInput = true;
            if(!pushedByWind)
                rb.velocity = new Vector2(rb.velocity.x * .1f, rb.velocity.y * .1f);

            rb.AddForceAtPosition(forceDir.normalized * -force, mousePos);

            FindObjectOfType<SoundManager>().Play("Shoot");

            CinemachineShake.Instance.ShakeCamera(5, .1f);
            if (!isGrounded || pushedByWind)
                ammo--;
        }
        else if(Input.GetButtonDown("Fire1") && (ammo == 0 || isStunned))
		{
            FindObjectOfType<SoundManager>().Play("NoAmmo");
        }
    }

    private void ModifyPhysics()
	{
        if ((rb.velocity.x > maxSpeedX || rb.velocity.x < -maxSpeedX))
            rb.velocity = new Vector2(maxSpeedX * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        if ((rb.velocity.y > maxSpeedY || rb.velocity.y < -maxSpeedY))
            rb.velocity = new Vector2(rb.velocity.x, maxSpeedY * Mathf.Sign(rb.velocity.y));

        if (pushedByWind)
            rb.gravityScale = 0;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravity * fallMultiplier;
        else
            rb.gravityScale = gravity;
    }

    private void Stun()
	{
        isStunned = true;
        ground.friction = frictionValue/2;
        sr.sprite = sPlayerSprite;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wind")
        {
            pushedByWind = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .2f);
            FindObjectOfType<SoundManager>().Play("Wind");
        }
        if(other.gameObject.tag == "EndScreen")
		{
            gameComplete = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
        if (other.gameObject.tag == "Wind")
        {
            pushedByWind = false;
            rb.velocity = new Vector2(rb.velocity.x * knockback, rb.velocity.y * .1f);
            FindObjectOfType<SoundManager>().Stop("Wind");
        }
    }

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground" && isGrounded)
            FindObjectOfType<SoundManager>().Play("Land");
        else if(other.gameObject.tag == "Ground" && !isGrounded)
            FindObjectOfType<SoundManager>().Play("Collision");
        if (other.gameObject.tag == "EnemyBullet")
            Stun();

    }

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }


}
