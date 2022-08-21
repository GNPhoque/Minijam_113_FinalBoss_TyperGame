using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VariableInt : ScriptableObject
{
	[SerializeField] int value;

	public int Value 
	{ 
		get => value; 
		set 
		{ 
			if (value < 0) value = 0; 
			this.value = value; 
			OnValueChanged?.Invoke(value); 
		} 
	}

	public event Action<int> OnValueChanged;
}
