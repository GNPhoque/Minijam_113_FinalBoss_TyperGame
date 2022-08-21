using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class Monster : MonoBehaviour
{
    [Header("TMP and Dynamic Text")]
    [SerializeField] GameObject wordElement;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] DynamicText dynamicText;
    [SerializeField] CanvasGroup boxBehindText;
    public string word;
    public int actualChar;

    [Header("Random words")]
    [SerializeField] VariableWordsList acceptingWords;
    [SerializeField] VariableWordsList refusingWords;

    [Header("Settings")]
    public MonsterState state;
    public bool isFrontLine;
    public bool isLocked;

    [Header("Patience")]
    [SerializeField] private float patienceMin = 1f;
    [SerializeField] private float patienceMax = 2f;
    private float patience;

    [SerializeField] Collider2D detectionZone;
    [SerializeField] float moveSpeed;

    // LOCAL
    private int tempChar;
    private bool isCurrentWordRefusingWord;
    private float patienceTemp;
    bool canMove = true;
    ContactFilter2D contactFilter2D;

    //ANIM DOTWEEN
    private Sequence animTextPatience;
    public Tween animFadeText;

    private void Start()
    {
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;

        PickRandomWord();
        RandomPatience();
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

    private void RandomPatience()
    {
        patience = UnityEngine.Random.Range(patienceMin, patienceMax);
    }

    public void PatienceTextAnim()
    {
        animTextPatience = DOTween.Sequence();
        animTextPatience.Join(boxBehindText.DOFade(0.2f, patience));
        animTextPatience.Join(wordElement.transform.DOLocalMoveY(20, patience));
        animTextPatience.OnComplete(() => Living());
        animFadeText = tmp.DOFade(0.4f, patience);
    }

    private void PickRandomWord()
    {
        isCurrentWordRefusingWord = UnityEngine.Random.Range(0, 2) == 0;
        if (isCurrentWordRefusingWord)
        {
            word = refusingWords.PickRandomWord();
        }
        else word = acceptingWords.PickRandomWord();
        tmp.text = word;
    }

    private void Living()
    {
        // animation grogne
        //GameManager.instance.LoseMoney();
        Finish();
    }

    public void ShowWord()
    {
        wordElement.SetActive(true);
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
        animTextPatience.Kill();
        MonsterManager.RemoveMonster(this);
        transform.parent.GetComponent<WaitingLine>().DestroyMonster();
    }

    public void ResetMonster()
    {
        state = MonsterState.IDLE;
        isLocked = true;
        dynamicText.ResetText();
    }

    public bool IsCurrentWordRefusingWord() => isCurrentWordRefusingWord;
}

public enum MonsterState
{
    IDLE,
    LOCK,
    COMBO
}

