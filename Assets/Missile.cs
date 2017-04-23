using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	Gun gun;
	float maxLifeTime;
	Vector3 direction;
	float speed = 10f;

	public void Launch(Gun gun , float lifeTime)
	{
		this.gun = gun;
		maxLifeTime = lifeTime;
		direction = gun.transform.up;
		transform.localPosition = gun.transform.position;
		transform.localRotation = gun.transform.rotation;
		gameObject.SetActive(true);
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (gameObject.activeSelf)
			maxLifeTime -= Time.deltaTime;

		if (maxLifeTime <= 0)
			DestroyMissile();
		
		transform.localPosition += direction * Time.deltaTime * speed;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Asteroid")
		{
			Asteroid ast = col.GetComponent<Asteroid>();
			ast.GetDamage();
			DestroyMissile();
		}
	}

	void DestroyMissile()
	{
		if (gun)
			gun.ReQueueMissile(this);
	}
	
}
