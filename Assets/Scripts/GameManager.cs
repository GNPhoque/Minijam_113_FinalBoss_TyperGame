using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] VariableInt money;

	private void OnEnable()
	{
		money.OnValueChanged += Money_OnValueChanged;
	}

	private void Money_OnValueChanged(int value)
	{
		if (value <= 0)
		{
			//GameOver
		}
	}
}
