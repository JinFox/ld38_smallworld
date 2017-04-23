using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	public ParticleSystem engineParticle;
	public ParticleSystem[] stabilizers;
	public PlanetController planet;
	public Transform arrow;
	public Transform asteroidArrow;
	public Transform GaugeStatus;
	

	public Gun gun;
	float maxGaugeScale;

	public float maxSpeed;
	public float maxRotationSpeed;
	public float acceleration;
	public float angularAcceleration;
	[Range(1,5)]
	public float stabilizationFactor;

	int maxCapacity = 3;
	int currentPickupNumber = 0; 

	Vector3 velocity;

	public AudioSource audioSource;
	// Use this for initialization
	void Start () {
		maxGaugeScale = GaugeStatus.localScale.y;
		UpdateGauge();
	}

	// Update is called once per frame
	bool fireEngine;
	bool stabilize;
	float angularVelocity;
	void Update ()
	{
		float vAxis = Input.GetAxisRaw("Vertical");
		
		fireEngine = vAxis> 0;
		stabilize = vAxis < 0;

		float hAxis = -Input.GetAxisRaw("Horizontal");
		if (Mathf.Sign(angularVelocity) != Math.Sign(hAxis)) // change direction
			angularVelocity = 0;
		angularVelocity += angularAcceleration* hAxis * Time.deltaTime;
		angularVelocity = Mathf.Clamp(angularVelocity, -maxRotationSpeed, maxRotationSpeed);
		transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, angularVelocity);

		if (fireEngine)
		{
			Vector3 accelerationAngle = transform.up * acceleration;
			velocity = Vector3.ClampMagnitude(velocity + accelerationAngle, maxSpeed);
		}
		if (stabilize)
		{
			velocity *= 1f - (stabilizationFactor * Time.deltaTime);
		}
		PlayEngineSound(fireEngine || stabilize);
		SetParticleEmitter();

		transform.localPosition += velocity * Time.deltaTime;

		SetArrowDirection();
		SetAsteroidArrowDirection();


		if (Input.GetButton("Jump") || Input.GetMouseButton(0))
		{
			gun.Shoot();
		}
	}

	private void PlayEngineSound(bool play)
	{
		if (audioSource.isPlaying && !play)
		{
			audioSource.Stop();
		}
		else if (!audioSource.isPlaying && play)
		{
			audioSource.Play();
		}
	}

	private void SetParticleEmitter()
	{
		ParticleSystem.EmissionModule st = stabilizers[0].emission;
		st.enabled = stabilize;
		ParticleSystem.EmissionModule st2 = stabilizers[1].emission;
		st2.enabled = stabilize;
		ParticleSystem.EmissionModule thrustEngine = engineParticle.emission;
		thrustEngine.enabled = fireEngine || stabilize;
	}

	private void SetArrowDirection()
	{
		Vector2 planetpos = planet.transform.localPosition;
		Vector2 pos = transform.localPosition;
		//Debug.DrawLine(pos, planetpos);

		Vector2 direction = new Vector2(pos.x - planetpos.x, pos.y - planetpos.y);
		float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		arrow.localEulerAngles = -transform.localEulerAngles + new Vector3(0, 0, 90f+angleDeg);
	}

	private void SetAsteroidArrowDirection()
	{
		Asteroid ast = GameController.Instance.currentAsteroid;
		if (ast == null)
		{
			asteroidArrow.gameObject.SetActive(false);
			return;
		}
		asteroidArrow.gameObject.SetActive(true);

		Vector2 astPos = ast.transform.localPosition;
		Vector2 pos = transform.localPosition;
		Debug.DrawLine(pos, astPos);

		Vector2 direction = new Vector2(pos.x - astPos.x, pos.y - astPos.y);
		float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		asteroidArrow.localEulerAngles = -transform.localEulerAngles + new Vector3(0, 0, 90f + angleDeg);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Pickups" && currentPickupNumber < maxCapacity)
		{
			currentPickupNumber++;
			GameController.Instance.DestroyPickup(col.gameObject);
			UpdateGauge();
		}
	}

	private void UpdateGauge()
	{
		Vector3 copy = GaugeStatus.localScale;
		copy.y = currentPickupNumber * maxGaugeScale / maxCapacity;
		GaugeStatus.localScale = copy;
		GaugeStatus.GetComponent<SpriteRenderer>().color = currentPickupNumber >= maxCapacity ? Color.red : Color.green;
	}

	public int EmptyCargo()
	{
		int points = currentPickupNumber;
		currentPickupNumber = 0;
		UpdateGauge();
		return points;
		
	}
}
