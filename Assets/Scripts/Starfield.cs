using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfield : MonoBehaviour {
	public Transform player;
	public Transform[] backgrounds;
	float wordToIndexRatio;
	float offset;
	
	public Queue<Transform> spawned;
	// Use this for initialization
	int curIndexX = int.MaxValue;
	int curIndexY = int.MaxValue;

	void Start () {
		spawned = new Queue<Transform>();
		Sprite sp = backgrounds[0].GetComponent<SpriteRenderer>().sprite;
		wordToIndexRatio = (sp.rect.height / sp.pixelsPerUnit);
		offset = wordToIndexRatio / 2f;
		foreach (Transform t in backgrounds)
		{
			spawned.Enqueue(t);
		}
	}

	void drawPos()
	{
		float left = curIndexX * wordToIndexRatio;
		float bottom = curIndexY * wordToIndexRatio;
		float right = (1+curIndexX) * wordToIndexRatio;
		float top = (1+curIndexY) * wordToIndexRatio;
		Debug.DrawLine(new Vector3(left, bottom), new Vector3(right, bottom));
		Debug.DrawLine(new Vector3(left, bottom), new Vector3(left, top));
		Debug.DrawLine(new Vector3(right, top), new Vector3(left, top));
		Debug.DrawLine(new Vector3(right, top), new Vector3(right, bottom));

	}
	// Update is called once per frame
	void Update () {

		int playerIndexPosX = Mathf.FloorToInt((player.localPosition.x / wordToIndexRatio));
		int playerIndexPosY = Mathf.FloorToInt((player.localPosition.y / wordToIndexRatio));
		
		if (playerIndexPosX != curIndexX || playerIndexPosY != curIndexY) // index changed
		{
			curIndexX = playerIndexPosX;
			curIndexY = playerIndexPosY;
			
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (!(x == 0 && y == 0))
					{
						RePositionTransform(x + curIndexX, y + curIndexY);
					}
				}
			}
			RePositionTransform(curIndexX, curIndexY);
		}
		
		drawPos();
	}

	void	RePositionTransform(int x, int y)
	{
		Transform background;
		Vector2 pos = new Vector2(
							x * wordToIndexRatio + offset,
							y * wordToIndexRatio + offset);
		background = spawned.Dequeue();
		background.localPosition = pos;
		spawned.Enqueue(background);

	}

	Vector2 GetIndexesByWorldPos(Vector2 position)
	{
		return new Vector2((int)position.x / wordToIndexRatio, (int)position.y / wordToIndexRatio);
	}
}
