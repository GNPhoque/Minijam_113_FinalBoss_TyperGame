using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	[SerializeField] VariableInt money;
	[SerializeField] TextMeshProUGUI moneyText;
	[SerializeField] int moneyLossPerRaise;

	[SerializeField] RequestList requestList;
	[SerializeField] CanvasGroup gameOverPanel;

	[Header("Speed")]
	[SerializeField] float speedMultiplier = 1f;
	[SerializeField] float delay = 10f;
	[SerializeField] float increaseStep = 1.1f;

	[Header("Random words")]
	[SerializeField] VariableWordsList acceptingWords;
	[SerializeField] VariableWordsList refusingWords;
	[SerializeField] float answerFitsRequestChance = .7f;

	public static GameManager instance;

	private void Awake()
	{
		if (instance) Destroy(instance.gameObject);
		instance = this;

		DOTween.Init(false, true);

		money.Value = 100;
		moneyText.text = money.Value.ToString();

		InvokeRepeating("IncreaseSpeed", delay, delay);
	}

	void IncreaseSpeed()
	{
		speedMultiplier *= increaseStep;
		speedMultiplier = Mathf.Clamp(speedMultiplier, 1, 10);
	}

	private void OnEnable()
	{
		money.OnValueChanged += Money_OnValueChanged;
	}

	#region MONEY
	private void Money_OnValueChanged(int value)
	{
		moneyText.text = value.ToString();
		if (value <= 0)
		{
			Debug.Log("GAME OVER");
			//GameOver
			GetComponent<SceneLoader>().LoadNextScene();
		}
	}

	public void EarnMoney(Request request)
	{
		return;
		money.Value += request.value * moneyLossPerRaise;
	}

	public void LoseMoney(Request request)
	{
		money.Value -= request.value * moneyLossPerRaise;
	} 
	#endregion

	public Request GetRandomRequest()
	{
		RequestInfo info = requestList.requestInfos[Random.Range(0, requestList.requestInfos.Count)];
		Request ret = new Request() { info = info, value = GetRandomRequestValue() };
		return ret;
	}

	int GetRandomRequestValue()
	{
		int value;
		float rng = Random.value;
		if (rng < .1f)
		{
			value = 3;
		}
		else if (rng < .35)
		{
			value = 2;
		}
		else
		{
			value = 1;
		}
		return value;
	}

	public string GetRandomAnswer(Request request, ref bool isAcceptation)
	{
		float random = Random.value;
		string answer;
		if (random < answerFitsRequestChance)
		{
			//Must type answer
			if (request.info.expectsAcceptation)
			{
				answer = acceptingWords.list[Random.Range(0, acceptingWords.list.Count)];
				isAcceptation = true;
			}
			else
			{
				answer = refusingWords.list[Random.Range(0, refusingWords.list.Count)];
				isAcceptation = false;
			}
		}
		else
		{
			//must wait
			if (request.info.expectsAcceptation)
			{
				answer = refusingWords.list[Random.Range(0, refusingWords.list.Count)];
				isAcceptation = false;
			}
			else
			{
				answer = acceptingWords.list[Random.Range(0, acceptingWords.list.Count)];
				isAcceptation = true;
			}
		}

		return answer;
	}

	public float GetSpeedMultiplier() => speedMultiplier;
}
