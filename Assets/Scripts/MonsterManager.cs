using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static List<Monster> firstLineMonsters;

    public static void AddMonster(Monster m)
    {
        foreach (Monster monster in firstLineMonsters)
        {
            if (monster.state == MonsterState.LOCK)
            {
                m.isLocked = true;
            }
        }
        firstLineMonsters.Add(m);
    }

    public static void RemoveMonster(Monster m)
    {
        firstLineMonsters.Remove(m);
        ResetAllMonsters(m);
    }

    public static void ResetAllMonsters(Monster m)
    {
        if (m.state == MonsterState.LOCK || m.state == MonsterState.COMBO)
        {
            foreach (Monster monster in MonsterManager.firstLineMonsters)
            {
                if (monster.state != MonsterState.COMBO)
                {
                    monster.isLocked = false;
                }
            }
        }
    }

	private void Start()
	{
        firstLineMonsters = new List<Monster>();
    }

	private void OnEnable()
    {
        InputManager.TypingLetter += InputManager_TypingLetter;
    }

    private void OnDisable()
    {
        InputManager.TypingLetter -= InputManager_TypingLetter;
    }

    private void InputManager_TypingLetter(string obj)
    {
        List<Monster> selected = new List<Monster>();
        List<Monster> notSelected = new List<Monster>();
        bool canContinue = true;

        foreach (Monster m in firstLineMonsters)
        {
            if (!m.isLocked)
            {
                if (m.actualChar < m.word.Length)
                {
                    if (m.word[m.actualChar].ToString().ToUpper() == obj)
                    {
                        if (m.state != MonsterState.LOCK)
                        {
                            selected.Add(m);
                        }
                        else
                        {
                            m.ConfirmedLetter();
                            canContinue = false;
                        }
                    }
                    else
                    {
                        notSelected.Add(m);
                    }
                }
            }
        }

        if (canContinue)
        {
            if (selected.Count == 1)
            {
                selected[0].state = MonsterState.LOCK;
                selected[0].ConfirmedLetter();
                foreach (Monster m in notSelected)
                {
                    m.ResetMonster();
                }
            }
            else if (selected.Count > 1)
            {
                foreach (Monster m in selected)
                {
                    m.state = MonsterState.COMBO;
                    m.ConfirmedLetter();
                    foreach (Monster mon in notSelected)
                    {
                        mon.ResetMonster();
                    }
                }
            }
        }

    }

}
