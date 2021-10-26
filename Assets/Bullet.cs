using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private GameObject hitEffect;
	[SerializeField] private bool enemyBullet;
	private void Awake()
	{
		Destroy(gameObject, 5f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((!enemyBullet) && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemies" || collision.gameObject.tag == "EnemyBullet"))
		{
			GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(effect, 0.2f);
			Destroy(gameObject);

			FindObjectOfType<SoundManager>().Play("Hit");
		}
		if ((enemyBullet) && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet"))
		{
			GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(effect, 0.2f);
			Destroy(gameObject);

			FindObjectOfType<SoundManager>().Play("Stun");
		}
	}
}
