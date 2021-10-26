using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Sprite gunFull;
    [SerializeField] private Sprite gunEmpty;

    public float bulletForce = 20f;

    public PlayerMovement player;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    Vector2 mousePos;

    void Update()
    {
        transform.position = playerTransform.position + new Vector3(offsetX / 1000, offsetY / 1000, 0);

        if (player.ammo > 0)
            sr.sprite = gunFull;
        else
            sr.sprite = gunEmpty;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 forceDir = mousePos - rb.position;
        float angle = Mathf.Atan2(forceDir.y, forceDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        if (angle < 0f && angle > -180f)
            sr.flipX = false;
        else sr.flipX = true;

        if (player.shootInput == true)
		{
            Shoot();
            player.shootInput = false;
        }
    }
    private void Shoot()
	{
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(shootPoint.up * bulletForce, ForceMode2D.Impulse);
	}
}
