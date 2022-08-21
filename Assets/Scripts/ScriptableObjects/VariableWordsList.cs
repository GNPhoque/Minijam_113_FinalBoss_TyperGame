using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VariableWordsList : ScriptableObject
{
	public List<string> list;

	public string PickRandomWord()
	{
		return list[Random.Range(0, list.Count)];
	}
}
