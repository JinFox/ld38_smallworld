using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public Sprite[] spriteList;
	
	float rotationalVelocity;

	public Animation hurtAnim;
	SpriteRenderer sr;
	int life = 3;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = spriteList[UnityEngine.Random.Range(0, spriteList.Length -1)];
		rotationalVelocity = UnityEngine.Random.Range(-20f, 20f);
		transform.localScale = Vector3.one * UnityEngine.Random.Range(.8f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles += new Vector3(0f, 0f, 1f) * rotationalVelocity * Time.deltaTime;
		
	}

	public void GetDamage()
	{
		life--;
		hurtAnim.Stop();
		hurtAnim.Play();
		if (life <= 0)
		{
			GameController.Instance.DestroyAsteroid(this);
			GameController.Instance.CreatePickup(transform.localPosition);
			GameController.Instance.PlayBoomSound();
			
			Destroy(gameObject);
		}
	}
}
