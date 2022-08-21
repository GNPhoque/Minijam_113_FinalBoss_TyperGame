using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Monster : MonoBehaviour
{
    [Header("TMP and Dynamic Text")]
	[SerializeField] TextMeshProUGUI wordText;
    [SerializeField] DynamicText dynamicText;

    [Header("Random words")]
    [SerializeField] VariableWordsList acceptingWords;
	[SerializeField] VariableWordsList refusingWords;

	bool isCurrentWordRefusingWord;
	public string word;
    public int actualChar;
    public bool isLocked;

    public bool isFrontLine;
    public MonsterState state;

    private int tempChar;

    private void Start()
    {
        PickRandomWord();
    }

    private void PickRandomWord()
    {
        isCurrentWordRefusingWord = UnityEngine.Random.Range(0, 2) == 0;
        if (isCurrentWordRefusingWord)
        {
            word = refusingWords.PickRandomWord();
        }
        else word = acceptingWords.PickRandomWord();
        wordText.text = word;
    }

	public void ShowWord()
	{
		wordText.enabled = true;
	}

    public void ConfirmedLetter()
    {
        switch (state)
        {
            case MonsterState.IDLE:
                dynamicText.LetterConfirmed();
                break;
            case MonsterState.LOCK:
                state = MonsterState.LOCK;
                dynamicText.ChangeColor(state);
                dynamicText.LetterConfirmed();
                break;
            case MonsterState.COMBO:
                state = MonsterState.COMBO;
                dynamicText.ChangeColor(state);
                dynamicText.LetterConfirmed();
                break;
        }
    }

    public void Finish()
    {
        MonsterManager.RemoveMonster(this);
        transform.parent.GetComponent<WaitingLine>().DestroyMonster();
    }

    public void Reset()
    {
         state = MonsterState.IDLE;
         isLocked = true;
         dynamicText.Reset();
    }
}

public enum MonsterState
{
    IDLE,
    LOCK,
    COMBO
}

