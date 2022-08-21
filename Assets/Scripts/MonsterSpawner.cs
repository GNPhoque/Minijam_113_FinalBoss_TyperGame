using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	[SerializeField] Monster[] monsterPrefabs;
	[SerializeField] List<WaitingLine> waitingLines;
	[SerializeField] float timeBetweenSpawns;
	[SerializeField] int maxMonstersPerLine;

	bool gameOver;

	private void Start()
	{
		StartCoroutine(SpawnTimer());
	}

	IEnumerator SpawnTimer()
	{
		while (!gameOver)
		{
			yield return new WaitForSeconds(timeBetweenSpawns);
			SpawnMonster();
		}
	}

	public void SpawnMonster()
	{
		//int notFullLines = waitingLines.Where(x => x.Count() < maxMonstersPerLine).ToList().Count;
		List<WaitingLine> notFullLines = waitingLines.Where(x => x.CanSpawnMonster()).ToList();
		if (notFullLines.Count > 0)
			notFullLines[Random.Range(0, notFullLines.Count)].SpawnNewMonster(monsterPrefabs[Random.Range(0,monsterPrefabs.Length)]);
	}
}
