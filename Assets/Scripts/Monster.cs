using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Monster : MonoBehaviour
{
    [Header("TMP and Dynamic Text")]
	[SerializeField] TextMeshProUGUI wordText;
    [SerializeField] DynamicText dynamicText;

    [Header("Random words")]
    [SerializeField] VariableWordsList acceptingWords;
	[SerializeField] VariableWordsList refusingWords;

    [SerializeField] Collider2D detectionZone;
    [SerializeField] float moveSpeed;

    bool isCurrentWordRefusingWord;
	public string word;
    public int actualChar;
    public bool isLocked;
    bool canMove = true;
    public bool isFrontLine;
    public MonsterState state;

    ContactFilter2D contactFilter2D;

    private int tempChar;

	private void Start()
    {
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;

        PickRandomWord();
    }

	private void Update()
	{
		if (canMove)
		{
            Vector3 movement = Vector3.right * Time.deltaTime * moveSpeed; ;
            transform.position += movement;
		}
	}

	private void FixedUpdate()
	{
        List<Collider2D> collided = new List<Collider2D>();
        Physics2D.OverlapCollider(detectionZone, contactFilter2D, collided);
        canMove = !collided.Any(x => x.CompareTag("Monster") || x.CompareTag("EndZone") || (x.CompareTag("WaitingZone") && !isFrontLine));
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

    public bool IsCurrentWordRefusingWord() => isCurrentWordRefusingWord;
}

public enum MonsterState
{
    IDLE,
    LOCK,
    COMBO
}

