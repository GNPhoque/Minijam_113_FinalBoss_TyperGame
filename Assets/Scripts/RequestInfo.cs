using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Request
{
	public RequestInfo info;
	public int value;
}

[System.Serializable]
public class RequestInfo
{
	public string name;
	public bool expectsAcceptation;
}