using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableInt : ScriptableObject
{
	[SerializeField] int value;

	public int Money { get => value; set { this.value = value; OnValueChanged?.Invoke(value); } }

	public event Action<int> OnValueChanged;
}
