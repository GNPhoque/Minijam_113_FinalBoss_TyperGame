using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    private void RandomPos()
    {
        transform.position = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-.8f, .8f), 0);
    }

    private void Spawn()
    {
        Debug.Log("Spawn");
        RandomPos();
        Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform.position, Quaternion.identity);
    }
}
