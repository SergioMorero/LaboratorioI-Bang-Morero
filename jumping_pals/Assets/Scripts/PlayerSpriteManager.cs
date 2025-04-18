using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteManager : MonoBehaviour
{

    public int CharSelected;
    public CharacterDB CharDB;
    public Sprite CharSprite;
    public SpriteRenderer Player;

    void Start()
    {
        CharSelected = PlayerPrefs.GetInt("CharId");
        LoadSprite();
    }

    void LoadSprite()
    {

        Character character = CharDB.getChar(CharSelected);
        CharSprite = character.sprite;
        Player.sprite = CharSprite;
    }
}
