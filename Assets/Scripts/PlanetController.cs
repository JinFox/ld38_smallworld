using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlanetObjects
{
	public GameObject obj;
	public int levelToEnable;
	public float angle;
	public float offsettoRadius;
	public float zPos;
}

public class PlanetController : MonoBehaviour {
	public SpriteRenderer planet;
	[SerializeField]
	float minRadius = 1f;
	[SerializeField]
	float maxRadius = 5f;
	[SerializeField]
	Gradient colorEvolution;

	public CircleCollider2D col;

	const int levelMax = 20;
	[Range(0, levelMax)]
	public float currentLevel = 0;

	[SerializeField]
	AudioSource audioSource;

	[SerializeField]
	public GameObject VictoryPanel;
	[SerializeField]
	public GameObject Atmosphere;
	public AnimationCurve AtmosphereGrowCurve;

	[SerializeField]
	public PlanetObjects[] planetObjects;

	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < planetObjects.Length; i++)
		{
			planetObjects[i].obj.SetActive(false);
			planetObjects[i].zPos = planetObjects[i].obj.transform.localPosition.z;
			Vector2 objPos = planetObjects[i].obj.transform.localPosition;
			planetObjects[i].angle = Mathf.Atan2(objPos.y, objPos.x);
		}
		VictoryPanel.SetActive(false);
		UpdateLevelDisplay();
	}

	// Update is called once per frame

	void Update () {
		float radius = GetCurrentRadius();
		planet.transform.localScale = Vector3.one * radius;
		planet.color = colorEvolution.Evaluate(currentLevel / (float)levelMax);
		col.radius = radius + .2f;

		transform.localEulerAngles = transform.localRotation.eulerAngles +new Vector3(0, 0, 1f)* Time.deltaTime;
	}

	float GetCurrentRadius()
	{
		return Mathf.Lerp(minRadius, maxRadius, currentLevel / (float)levelMax);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			Rocket player = col.GetComponent<Rocket>();
			int points = player.EmptyCargo();
			if (points > 0)
			{
				//Debug.Log("YAY +" + points + " points !");
				currentLevel = Mathf.Min(currentLevel + points, levelMax);
				UpdateLevelDisplay();
				audioSource.Play();
			}

		}
	}

	private void UpdateLevelDisplay()
	{
		for (int i = 0; i < planetObjects.Length; i++)
		{
			PlanetObjects po = planetObjects[i];
			if (po.levelToEnable <= currentLevel)
			{
				Vector3 pos = new Vector3(Mathf.Cos(po.angle) * (GetCurrentRadius() + po.offsettoRadius),
										Mathf.Sin(po.angle) * (GetCurrentRadius() + po.offsettoRadius),
										planetObjects[i].zPos);
				//Debug.Log(po.obj.name + " : angle " + po.angle + " | pos " + pos);
				po.obj.transform.localPosition = pos;
				po.obj.SetActive(true);
			}
			else
			{
				po.obj.SetActive(false);
			}			
		}
		Atmosphere.transform.localScale = Vector3.one * AtmosphereGrowCurve.Evaluate(currentLevel);
		VictoryPanel.SetActive(currentLevel >= levelMax);
		
	}
}
