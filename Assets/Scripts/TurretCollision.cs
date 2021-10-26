using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCollision : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite turretGun;
    [SerializeField] private Sprite sTurretGun;
    [SerializeField] private float stunDuration = 5f;
    private float stunCountdown = 0f;
    public bool turretIsStunned = false;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stunCountdown > 0)
            stunCountdown -= Time.deltaTime;
        else if(stunCountdown <= 0)
		{
            turretIsStunned = false;
            sr.sprite = turretGun;
        }
        if(stunCountdown > 0 && stunCountdown < .1f)
            FindObjectOfType<SoundManager>().Play("EnemyRecover");


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet" && !turretIsStunned)
            Stun();
    }

    private void Stun()
	{
        stunCountdown = stunDuration;
        turretIsStunned = true;
        sr.sprite = sTurretGun;
        FindObjectOfType<SoundManager>().Play("EnemyStun");
    }
}
