using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.U2D.Animation;

public class PlayerSpriteManager : MonoBehaviour
{

    public int CharSelected;
    public CharacterDB CharDB;
    public Character character;
    public SpriteLibrary SpriteSource;
    public SpriteResolver resolver;

    void Start()
    {
        CharSelected = PlayerPrefs.GetInt("CharId");
        LoadSprite();
    }

    void LoadSprite()
    {
        character = CharDB.getChar(CharSelected);
        SpriteSource.spriteLibraryAsset = character.sprites;
    }

    public void walk()
    {

    }

}
