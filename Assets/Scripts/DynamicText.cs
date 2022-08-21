using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DynamicText : MonoBehaviour
{
    [SerializeField] private Monster monster;

    [SerializeField] private Color lockColor;
    [SerializeField] private Color validatedColor;
    [SerializeField] private Color tempColor;
    [SerializeField] private Color comboColor;
    [SerializeField] private float charAnimDuration = .3f;

    private TMPro.TextMeshProUGUI tmp;
    private DOTweenTMPAnimator animator;
    private Sequence s;

    [HideInInspector] private Color chosenColor;

    private void Start()
    {
        chosenColor = tempColor;
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
        animator = new DOTweenTMPAnimator(tmp);
    }

    private IEnumerator AnimChar(int tempChar)
    {
        s = DOTween.Sequence();
        s.Join(animator.DOColorChar(tempChar, chosenColor, charAnimDuration));
        s.Join(animator.DOPunchCharRotation(tempChar, new Vector3(0, 0, 10), charAnimDuration));
        s.Join(animator.DOPunchCharScale(tempChar, 1, charAnimDuration));
        yield return s.WaitForCompletion();
        if (tempChar == tmp.text.Length-1) 
        {
            Sequence s2 = DOTween.Sequence();
            s2.Join(transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
            s2.Join(transform.DOPunchRotation(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration));
            s2.Join(tmp.DOColor(validatedColor, charAnimDuration));
            s2.OnComplete(() => monster.Finish());
        }
    }

    public void LetterConfirmed()
    {
        StartCoroutine(AnimChar(monster.actualChar));
        monster.actualChar++;

        // Check if end of the word
        if (monster.actualChar == tmp.text.Length - 1)
        {
            MonsterManager.ResetAllMonsters(monster);
        }
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

    public void Reset()
    {
        if (monster.state != MonsterState.IDLE)
        {
            ChangeColor(MonsterState.IDLE);
            s.Join(tmp.DOColor(Color.white, charAnimDuration / 2));
            s.Join(tmp.transform.DOPunchRotation(new Vector3(0, 0, 10), charAnimDuration / 2));
            s.Join(tmp.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), charAnimDuration / 2));
        }
    }

}
