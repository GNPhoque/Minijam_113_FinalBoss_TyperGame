using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BossTextAnimation : MonoBehaviour
{
	[SerializeField] CanvasGroup canvasGroup;

	private void Start()
	{
		Sequence s = DOTween.Sequence();
		s.Join(transform.DOLocalMoveY(5, .6f).SetRelative(true));
		s.Join(canvasGroup.DOFade(1, .8f));
		s.AppendInterval(.2f);
		s.Append(canvasGroup.DOFade(0, .5f));
	}

	private void OnDisable()
	{
		DOTween.Kill(this);
	}
}
