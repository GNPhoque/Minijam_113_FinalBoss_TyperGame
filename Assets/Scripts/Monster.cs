using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI wordText;
	[SerializeField] new Collider2D collider;

	[SerializeField] VariableWordsList acceptingWords;
	[SerializeField] VariableWordsList refusingWords;

	bool isCurrentWordRefusingWord;
	bool canMove = true;
	string word;

	private void Start()
	{
		isCurrentWordRefusingWord = Random.Range(0, 2) == 0;
		if (isCurrentWordRefusingWord)
		{
			word = refusingWords.PickRandomWord();
		}
		else word = acceptingWords.PickRandomWord();
		wordText.text = word;
	}

	private void Update()
	{
		if (canMove)
		{
			transform.Translate(Vector3.right * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Trigger");
		canMove = false;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		canMove = true;
	}

	public void ShowWord()
	{
		wordText.enabled = true;
	}
}
