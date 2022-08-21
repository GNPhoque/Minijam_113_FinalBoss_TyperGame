using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
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
		List<WaitingLine> availableLines = waitingLines.Where(x => x.Count() < maxMonstersPerLine).ToList();
		int notFullLines = availableLines.Count;
		if (notFullLines > 0)
			availableLines[Random.Range(0, notFullLines)].SpawnNewMonster();
	}
}