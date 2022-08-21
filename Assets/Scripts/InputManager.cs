using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    Array keycodes;

    private void Start()
	{
        //keycodes = System.Enum.GetValues(typeof(KeyCode));
        keycodes = new KeyCode[] 
        {
            KeyCode.A, 
            KeyCode.B, 
            KeyCode.C, 
            KeyCode.D, 
            KeyCode.E, 
            KeyCode.F, 
            KeyCode.G, 
            KeyCode.H, 
            KeyCode.I, 
            KeyCode.J, 
            KeyCode.K, 
            KeyCode.L, 
            KeyCode.M, 
            KeyCode.N, 
            KeyCode.O, 
            KeyCode.P, 
            KeyCode.Q, 
            KeyCode.R, 
            KeyCode.S, 
            KeyCode.T, 
            KeyCode.U, 
            KeyCode.V, 
            KeyCode.W, 
            KeyCode.X, 
            KeyCode.Y, 
            KeyCode.Z, 
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            TryKey();
        }
    }

    private void TryKey()
    {
        foreach (KeyCode vKey in keycodes)
        {
            if (Input.GetKeyDown(vKey))
            {
                TypingLetter?.Invoke(vKey.ToString());
            }
        }
    }

    public static event Action <string> TypingLetter;

}
