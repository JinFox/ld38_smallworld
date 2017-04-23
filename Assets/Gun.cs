using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	public GameObject missilePrefab;
	AudioSource audioSource;
	
	Queue<Missile> missilePool;
	public int missilePoolSize = 20;

	public float missilePerSecond = 1f;
	float timeUntilNextShoot;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		missilePool = new Queue<Missile>();
		for (int i = 0; i < missilePoolSize; i++)
		{
			Missile obj = GameObject.Instantiate<GameObject>(missilePrefab, GameController.Instance.transform).GetComponent<Missile>();
			missilePool.Enqueue(obj);
			obj.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if (timeUntilNextShoot >= 0f)
			timeUntilNextShoot -= Time.deltaTime;
	}

	public void Shoot()
	{
		if (timeUntilNextShoot<=0)
		{
			timeUntilNextShoot = 1f/missilePerSecond;
			audioSource.Play();
			Missile m = missilePool.Dequeue();
			m.Launch(this, missilePoolSize / missilePerSecond);
		}
	}

	internal void ReQueueMissile(Missile missile)
	{
		missile.gameObject.SetActive(false);
		missilePool.Enqueue(missile);
	}
}
