using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	static GameController _instance;
	public static GameController Instance
	{
		get
		{
			return _instance;
		}
	}

	[SerializeField]
	GameObject asteroidPrefab;
	[SerializeField]
	GameObject pickupPrefab;
	[SerializeField]
	Rocket player;
	[SerializeField]
	PlanetController planet;

	public float minSpawnDistance = 30f;
	public float maxSpawnDistance = 30f;
	[HideInInspector]
	public Asteroid currentAsteroid;

	[SerializeField]
	AudioSource audiosource;
	[SerializeField]
	AudioClip boomSound;
	[SerializeField]
	AudioClip pickupSound;
	[SerializeField]
	AudioClip planetUpdateSound;

	[SerializeField]
	Button restart;

	void Awake()
	{
		_instance = this;
	}
	// Use this for initialization
	void Start () {
		restart.onClick.AddListener(OnRestartClicked);
	}

	private void OnRestartClicked()
	{
		SceneManager.LoadScene("main");
	}

	// Update is called once per frame
	void Update () {
		if (currentAsteroid == null)
		{
			float rad = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
			float angle = UnityEngine.Random.Range(0, Mathf.PI * 2f);

			currentAsteroid = GameObject.Instantiate<GameObject>(asteroidPrefab).GetComponent<Asteroid>();
			currentAsteroid.transform.localPosition = new Vector3(rad * Mathf.Cos(angle), rad * Mathf.Sin(angle), 20f);
		}
	}

	
	internal void CreatePickup(Vector2 position)
	{
		GameObject.Instantiate<GameObject>(pickupPrefab, position, Quaternion.identity);
	}

	public void DestroyAsteroid(Asteroid ateroid)
	{
		currentAsteroid = null;
	}

	internal void PlayBoomSound()
	{
		audiosource.clip = boomSound;
		audiosource.Play();
	}

	internal void DestroyPickup(GameObject pickup)
	{
		Destroy(pickup);
		audiosource.clip = pickupSound;
		audiosource.Play();
	}
}
