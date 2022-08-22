using DG.Tweening;
using System.Collections;
using UnityEngine;
using TMPro;

public class DynamicText : MonoBehaviour
{
    private Monster monster;

    [SerializeField] private Color lockColor;
    [SerializeField] private Color validatedColor;
    [SerializeField] private Color tempColor;
    [SerializeField] private Color comboColor;
    [SerializeField] private float charAnimDuration = .3f;
    [SerializeField] GameObject TypingSfx;

    private TMPro.TextMeshProUGUI tmp;
    private DOTweenTMPAnimator animator;
    private Sequence s;

    [HideInInspector] private Color chosenColor;

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    private void Start()
    {
        monster = GetComponent<Monster>();
        chosenColor = tempColor;
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
        //animator = new DOTweenTMPAnimator(tmp);
    }

    private IEnumerator AnimChar(int tempChar)
    {
        monster.animFadeText.Pause();
        s = DOTween.Sequence();
        s.Join(animator.DOColorChar(tempChar, chosenColor, charAnimDuration));
        s.Join(animator.DOPunchCharRotation(tempChar, new Vector3(0, 0, 10), charAnimDuration));
        s.Join(animator.DOPunchCharScale(tempChar, 1, charAnimDuration));
        yield return s.WaitForCompletion();
        if (tempChar == monster.answer.Length - 1)
        {
            Sequence s2 = DOTween.Sequence();
            s2.Join(transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
            s2.Join(transform.DOPunchRotation(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
            s2.Join(tmp.DOColor(validatedColor, charAnimDuration));
            s2.OnComplete(() => monster.Finish());
        }
    }

    private IEnumerator AnimKey(int tempChar, string str)
    {
        s = DOTween.Sequence();
        s.Join(monster.keys[tempChar].transform.DOPunchScale(new Vector3(.5f, .5f, .5f), charAnimDuration));
        s.Join(monster.keys[tempChar].transform.DOPunchRotation(new Vector3(0, 0, 10), charAnimDuration));
        s.Join(monster.keys[tempChar].GetComponentInChildren<TMPro.TextMeshProUGUI>().DOColor(chosenColor, charAnimDuration));
        yield return s.WaitForCompletion();
        if (tempChar == monster.answer.Length - 1)
        {
            Sequence s2 = DOTween.Sequence();
            foreach (GameObject k in monster.keys)
            {
                s2.Join(k.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOColor(validatedColor, charAnimDuration));
                s2.Join(transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
                s2.Join(transform.DOPunchRotation(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
            }
            s2.OnComplete(() => monster.Finish());
        }
    }

    public void LetterConfirmed()
    {
        //firet line but not yet in place
        if (monster.keys.Count == 0) return;

        Instantiate(TypingSfx);

        // Check if end of the word
        if (monster.currentChar == monster.answer.Length - 1)
        {
            AnimEnd();
        }
		else
		{
            AnimKey(monster.currentChar);
            monster.currentChar++;
        }
    }

    void AnimKey(int tempChar)
    {
		try
		{
			s = DOTween.Sequence();
			s.Join(monster.keys[tempChar].transform.DOPunchScale(new Vector3(.5f, .5f, .5f), charAnimDuration));
			s.Join(monster.keys[tempChar].transform.DOPunchRotation(new Vector3(0, 0, 10), charAnimDuration));
			s.Join(monster.keys[tempChar].GetComponentInChildren<TMPro.TextMeshProUGUI>().DOColor(chosenColor, charAnimDuration));
		}
		catch (System.Exception e)
		{
            Debug.Log("Out of range exception");
		}
    }

    void AnimEnd()
    {
        Sequence s2 = DOTween.Sequence();
        foreach (GameObject k in monster.keys)
        {
            s2.Join(k.GetComponentInChildren<TMPro.TextMeshProUGUI>().DOColor(validatedColor, charAnimDuration));
            s2.Join(transform.DOPunchScale(new Vector3(1.05f, 1.05f, 1.05f), charAnimDuration));
            s2.Join(transform.DOPunchRotation(new Vector3(0, 0, 4), charAnimDuration));
        }
        s2.OnComplete(() => monster.Finish());
    }

    public void ChangeColor(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.IDLE:
                chosenColor = tempColor;
                break;
            case MonsterState.LOCK:
                chosenColor = lockColor;
                break;
            case MonsterState.COMBO:
                chosenColor = comboColor;
                break;
        }
    }

    public void ResetText()
    {
        if (monster.state != MonsterState.IDLE)
        {
            ChangeColor(MonsterState.IDLE);
            monster.currentChar = 0;

            Sequence s2 = DOTween.Sequence();
            foreach (var key in monster.keys)
            {
                TextMeshProUGUI t = key.GetComponentInChildren<TextMeshProUGUI>();
                s2.Join(t.DOColor(Color.white, charAnimDuration/2));
                s2.Join(key.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration/2));
                s2.Join(key.transform.DOPunchRotation(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration/2));
            }
        }
    }

}
