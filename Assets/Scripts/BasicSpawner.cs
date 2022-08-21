using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    PlayerInputs inputs;

	private void Awake()
	{
        inputs = new PlayerInputs();
	}

	private void OnEnable()
	{
		inputs.TEST.test.performed += Test_performed;
		inputs.typing.key.performed += Key_performed;
        inputs.Enable();
	}

	private void Key_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
        Debug.Log(obj.ReadValue<char>());
	}

	private void Test_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
        Spawn();
	}

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
