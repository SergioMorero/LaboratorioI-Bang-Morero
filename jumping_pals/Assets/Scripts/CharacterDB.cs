using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDB", menuName = "Scriptable Objects/CharacterDB")]
public class CharacterDB : ScriptableObject
{
    public Character[] CharList;
    public int CharCount
    {
        get
        {
            return CharList.Length;
        }
    }

    public Character getChar(int index)
    {
        return CharList[index];
    }
}
