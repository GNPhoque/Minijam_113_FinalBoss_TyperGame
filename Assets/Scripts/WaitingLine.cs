using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitingLine : MonoBehaviour
{
	new Collider2D collider;
	List<Monster> monsters;

	private void Awake()
	{
		collider = GetComponent<Collider2D>();
		monsters = new List<Monster>();
	}

	public void SpawnNewMonster(Monster prefab)
	{
		Monster monster = Instantiate(prefab, transform.position, Quaternion.identity, transform);
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

	public bool CanSpawnMonster()
	{
		List<Collider2D> collided = new List<Collider2D>();

		ContactFilter2D contactFilter2D = new ContactFilter2D();
		contactFilter2D.useTriggers = true;
		int col = Physics2D.OverlapCollider(collider, contactFilter2D, collided);
		int ret = collided.Where(x => x.CompareTag("Monster")).ToList().Count;
		return ret == 0;
	}

	public int Count() => monsters.Count;
}
