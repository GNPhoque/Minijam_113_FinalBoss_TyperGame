using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] public List<AudioClip> sounds = new List<AudioClip>();
    AudioSource source;
    void Start()
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Count - 1)];
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(this.gameObject, 1);
    }
}