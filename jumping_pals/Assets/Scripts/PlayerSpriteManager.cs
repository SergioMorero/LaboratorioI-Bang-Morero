using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpriteManager : MonoBehaviour
{

    public int CharSelected;
    public CharacterDB CharDB;
    public Sprite CharSprite;
    public SpriteRenderer Player;
    public Character character;

    void Start()
    {
        CharSelected = PlayerPrefs.GetInt("CharId");
        LoadSprite();
    }

    void LoadSprite()
    {
        character = CharDB.getChar(CharSelected);
        CharSprite = character.sprite;
        Player.sprite = CharSprite;
    }

}
