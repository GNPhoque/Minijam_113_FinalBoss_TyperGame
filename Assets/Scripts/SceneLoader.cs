using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void LoadNextScene()
	{
		StartCoroutine(Delay(SceneManager.GetActiveScene().buildIndex + 1));
	}

	public void ReloadScene()
	{
		StartCoroutine(Delay(SceneManager.GetActiveScene().buildIndex));
	}

	public void ReloadGameScene()
	{
		StartCoroutine(Delay(1));
	}

	IEnumerator Delay(int index)
	{
		yield return new WaitForSeconds(.15f);
		SceneManager.LoadScene(index);
	}

}
