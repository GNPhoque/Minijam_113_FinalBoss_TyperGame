using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;

public class Monster : MonoBehaviour
{
    [Header("TMP and Dynamic Text")]
    [SerializeField] TextMeshProUGUI requestText;
    [SerializeField] TextMeshProUGUI requestValueText;
    [SerializeField] Image valueUpDownImage;
    [SerializeField] Color ValueUpColor;
    [SerializeField] Color ValueDownColor;
    [SerializeField] ProgressBarCircle patienceProgressBar;
    [SerializeField] Sprite thumbUp;
    [SerializeField] Sprite thumbDown;
    DynamicText dynamicText;
    public string answer;
    public int currentChar;

    [Header("Key Creation")]
    [SerializeField] GameObject keyElement;
    [SerializeField] Transform wordT;
    [HideInInspector] public List<GameObject> keys = new List<GameObject>();

    [Header("Settings")]
    [SerializeField] RuntimeAnimatorController[] animators;
    public MonsterState state;
    public bool isFrontLine;
    public bool skipLetterCheck;

    [Header("Patience")]
    [SerializeField] private float patienceMin = 5f;
    [SerializeField] private float patienceMax = 7f;
    private float patience;

    [SerializeField] Collider2D detectionZone;
    [SerializeField] float moveSpeedFirstInLine;
    [SerializeField] float moveSpeed;

    [Header("AUDIO")]
    [SerializeField] GameObject arriveSfx;
    [SerializeField] GameObject goneSfx;

    // LOCAL
    private int tempChar;
    private bool isCurrentWordRefusingWord;
    private float patienceTemp;
    bool canMove = true;
    ContactFilter2D contactFilter2D;
    Request request;
    bool answerShown;
    bool patienceTimerStarted;
    bool isAnswerAcceptation;
    Animator animator;
    bool mustStopAnimation;
    bool hasFinished;

    //ANIM DOTWEEN
    private Sequence animTextPatience;
    public Tween animFadeText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animators[UnityEngine.Random.Range(0, animators.Length)];
        SetRequestAndAnswer();
        RandomPatience();
    }

	private void OnDisable()
	{
        DOTween.Kill(this);
	}

	private void Start()
    {
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;

        dynamicText = GetComponent<DynamicText>();
        SetRequestAndAnswer();
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3 movement = Vector3.right * Time.deltaTime * (isFrontLine ? moveSpeedFirstInLine : moveSpeed);
            transform.position += movement;
			if (mustStopAnimation)
			{
				mustStopAnimation = false;
                animator.speed = 1;
            }
        }
        else
		{
            animator.speed = 0;
            mustStopAnimation = true;
        }

		if (patienceTimerStarted)
		{
            patienceTemp += Time.deltaTime;
            patienceProgressBar.BarValue = ((patience - patienceTemp)/patience)*100;
			if (patienceTemp >= patience && !hasFinished)
			{
                hasFinished = true;
                state = MonsterState.GONE;
                Finish();
			}
		}
    }

    private void FixedUpdate()
    {
        List<Collider2D> collided = new List<Collider2D>();
        Physics2D.OverlapCollider(detectionZone, contactFilter2D, collided);
        canMove = !collided.Any(x => x.CompareTag("Monster") || x.CompareTag("EndZone") || (x.CompareTag("WaitingZone") && !isFrontLine));
        if (!canMove && collided.Any(x => x.CompareTag("EndZone")))
		{
            if (!answerShown)
            {
                answerShown = true;
                ShowAnswerAndRequest();
                patienceTimerStarted = true;
                patienceProgressBar.gameObject.SetActive(true);
                patienceProgressBar.BarValue = 100;
            }
		}
    }

    public void AnimationEventCanStopHere()
	{
		if (mustStopAnimation)
		{
            animator.StopPlayback();
        }
    }

    private void RandomPatience()
    {
        patience = UnityEngine.Random.Range(patienceMin * GameManager.instance.GetSpeedMultiplier(), patienceMax * GameManager.instance.GetSpeedMultiplier());
    }

    public void PatienceTextAnim()
    {
	}

    private void SetRequestAndAnswer()
    {
        PickRandomRequest();
        PickRandomAnswer();
    }

	private void PickRandomRequest()
	{		
        request = GameManager.instance.GetRandomRequest();
    }

    private void PickRandomAnswer()
    {
        answer = GameManager.instance.GetRandomAnswer(request, ref isAnswerAcceptation);
    }

    public void ShowAnswerAndRequest()
    {
        requestText.text = request.info.name;
        foreach (char a in answer)
        {
            GameObject g = Instantiate(keyElement, wordT);
            g.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = a.ToString();
            keys.Add(g);
        }
        requestValueText.text = request.value == 1 ? "$" : request.value == 2 ? "$$" : request.value == 3 ? "$$$" : "";
        valueUpDownImage.gameObject.SetActive(true);
        if (request.info.expectsAcceptation)
        {
            valueUpDownImage.sprite = thumbUp;
        }
        else
        {
            valueUpDownImage.sprite = thumbDown;
        }

        Instantiate(arriveSfx);

        transform.DOPunchPosition(new Vector3(0, .3f, 0), .3f, 0, 0);
    }

    public void SetAnswerScreenPosition(Transform t)
	{
        wordT = t;
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
        //float valueMultiplyer = (patience - patienceTemp) / patience;
        //request.value = (int)(valueMultiplyer * request.value);
        //Debug.Log(valueMultiplyer);
        if (state == MonsterState.GONE) //Did not answer
        {
            if (request.info.expectsAcceptation)//request requires a YES
            {
                if (isAnswerAcceptation)//Answer is a YES
                    GameManager.instance.LoseMoney(request);
                else//Answer is a NO
                    GameManager.instance.EarnMoney(request);
            }
            else//request requires a NO
            {
                if (isAnswerAcceptation)//Answer is a YES
                    GameManager.instance.EarnMoney(request);
                else//Answer is a NO
                    GameManager.instance.LoseMoney(request);
            }
            GoneAnim();
            return;
        }
        else //did answer
        {
            if (request.info.expectsAcceptation)//request requires a YES
            {
                if (isAnswerAcceptation)//Answer is a YES
                    GameManager.instance.EarnMoney(request);
                else//Answer is a NO
                    GameManager.instance.LoseMoney(request);
            }
            else//request requires a NO
            {
                if (isAnswerAcceptation)//Answer is a YES
                    GameManager.instance.LoseMoney(request);
                else//Answer is a NO
                    GameManager.instance.EarnMoney(request);
            }
        }
        Final();
    }

    void GoneAnim()
	{
        Instantiate(goneSfx);

        skipLetterCheck = true;
		Sequence s = DOTween.Sequence();
        s.Join(requestText.GetComponent<CanvasGroup>().DOFade(0, .2f));
        s.Append(transform.DORotate(new Vector3(0, 180, 0), .2f));
		s.Join(transform.DOLocalMoveX(-2, .5f).SetRelative(true).SetEase(Ease.InQuad));
		s.Join(GetComponent<SpriteRenderer>().DOFade(0, .5f));
        s.OnComplete(() => Final());
	}

	private void Final()
	{
		MonsterManager.RemoveMonster(this);
		transform.parent.GetComponent<WaitingLine>().DestroyMonster();
		foreach (Transform item in wordT)
		{
			Destroy(item.gameObject);
		}
		MonsterManager.ResetAllMonsters(this);
    }

    public void ResetMonster()
    {
        //skipLetterCheck = true;
        dynamicText.ResetText();
        state = MonsterState.IDLE;
    }

    public bool IsCurrentWordRefusingWord() => isCurrentWordRefusingWord;
}

public enum MonsterState
{
    IDLE,
    LOCK,
    COMBO,
    GONE
}

