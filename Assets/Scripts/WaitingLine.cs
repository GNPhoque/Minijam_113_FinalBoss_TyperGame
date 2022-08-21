using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingLine : MonoBehaviour
{
	[SerializeField] Monster monsterPrefab;

	List<Monster> monsters;

	private void Awake()
	{
		monsters = new List<Monster>();
	}

	public void SpawnNewMonster()
	{
		Monster monster = Instantiate(monsterPrefab, transform.position + Vector3.left * monsters.Count, Quaternion.identity, transform);
		monsters.Add(monster);
		if(monsters.Count == 1)
		{
			PutInFrontLine(monster);
		}
	}

	public void DestroyMonster()
	{
		Destroy(monsters[0].gameObject);
		monsters.RemoveAt(0);
		if (monsters.Count > 0)
		{
			PutInFrontLine(monsters[0]);
		}
	}

	private void PutInFrontLine(Monster m)
	{
		m.ShowWord();
		m.isFrontLine = true;
		MonsterManager.AddMonster(m);
	}

	public int Count() => monsters.Count;
}
