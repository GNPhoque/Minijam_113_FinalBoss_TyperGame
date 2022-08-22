using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject bossText;
    [SerializeField] VariableInt money;
	[SerializeField] GameObject NextSFXPrefab;

	private void OnEnable()
	{
		money.OnValueChanged += Money_OnValueChanged;
	}

	private void OnDisable()
	{
		money.OnValueChanged -= Money_OnValueChanged;
	}

	private void Money_OnValueChanged(int value)
	{
		GameObject text = Instantiate(bossText, transform);
		Instantiate(NextSFXPrefab);
		Destroy(text, 3f);
	}
}
