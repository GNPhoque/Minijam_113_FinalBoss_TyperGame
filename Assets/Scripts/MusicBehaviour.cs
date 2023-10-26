using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehaviour : MonoBehaviour
{
    public static bool alreadyExists;

    AudioSource source;
    [SerializeField] AudioClip musicIntro;
    [SerializeField] AudioClip musicLoop;

    bool hasLooped;

	private void Awake()
	{
        if (alreadyExists)
        {
            Destroy(this);
            return;
        }

        alreadyExists = true;
        DontDestroyOnLoad(this);
	}

	void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = musicIntro;
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying && !hasLooped)
        {
            hasLooped = true;
            PlayLoop();
        }
    }

    private void PlayLoop()
    {
        source.loop = true;
        source.clip = musicLoop;
        source.Play();
    }
}
