using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpriteRenderer sr;

    public TurretCollision tCollision;

    [SerializeField] private float shootCd = 1f;
    private float shootCounter;

    public float bulletForce = 20f;
    public float viewDistance = 10f;
    public LayerMask targetLayer;

    public PlayerMovement player;

    [SerializeField] private Rigidbody2D rb;
    Vector2 playerPos;

	private void Awake()
	{
        shootCounter = shootCd;
	}
	void Update()
    {
        shootCounter -= Time.deltaTime;

        transform.position = turretTransform.position;

        playerPos = playerTransform.position;

		if (!tCollision.turretIsStunned)
		{
            Vector2 forceDir = playerPos - rb.position;
            float angle = Mathf.Atan2(forceDir.y, forceDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            if (angle < 0f && angle > -180f)
                sr.flipX = true;
            else sr.flipX = false;
        }
    }

	private void FixedUpdate()
	{
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), viewDistance, targetLayer);

		if (hit)
		{
            if(hit.collider.tag == "Player")
                Shoot();
		}
	}

    private void Shoot()
    {
        if(shootCounter <= 0 && tCollision.turretIsStunned == false)
		{
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            rbBullet.AddForce(shootPoint.up * bulletForce, ForceMode2D.Impulse);
            FindObjectOfType<SoundManager>().Play("EnemyShoot");

            shootCounter = shootCd;
        }
    }

}
