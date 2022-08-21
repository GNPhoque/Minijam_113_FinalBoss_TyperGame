using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer _rHead;
    [SerializeField] private SpriteRenderer _rWeapon;
    [SerializeField] private SpriteRenderer _rBody;
    [SerializeField] private SpriteRenderer _rLeg;

    [Header("Assets")]
    [SerializeField] private List<Sprite> heads = new List<Sprite>();
    [SerializeField] private List<Sprite> weapons = new List<Sprite>();
    [SerializeField] private List<Sprite> bodies = new List<Sprite>();
    [SerializeField] private List<Sprite> legs = new List<Sprite>();

    [Header("Controls")]
    [SerializeField] private List<float> sizes = new List<float>();

    private void Start()
    {
        RandomizeLook();
    }

    private void RandomizeLook()
    {
        float newSize = sizes[Random.Range(0, sizes.Count)];

        transform.localScale = new Vector3 (newSize, newSize, 1);
        _rHead.sprite = heads[Random.Range(0, heads.Count)];
        _rWeapon.sprite = weapons[Random.Range(0, weapons.Count)];
        _rBody.sprite = bodies[Random.Range(0, bodies.Count)];
        _rLeg.sprite = legs[Random.Range(0, legs.Count)];
    }
}
