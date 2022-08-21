using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] VariableInt money;
	[SerializeField] TextMeshProUGUI moneyText;
	[SerializeField] int moneyLossPerRaise;

	public static GameManager instance;

	private void Awake()
	{
		if (instance) Destroy(instance.gameObject);
		instance = this;
		money.Value = 100;
		moneyText.text = "$" + money.Value;
	}

	private void OnEnable()
	{
		money.OnValueChanged += Money_OnValueChanged;
	}

	private void Money_OnValueChanged(int value)
	{
		moneyText.text = "$" + value;
		if (value <= 0)
		{
			Debug.Log("GAME OVER");
			//GameOver
		}
	}

	public void LoseMoney()
	{
		money.Value -= moneyLossPerRaise;
	}
}
