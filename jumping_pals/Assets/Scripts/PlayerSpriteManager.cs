using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.U2D.Animation;

public class PlayerSpriteManager : MonoBehaviour
{

    public int CharSelected;
    public int alt;
    public CharacterDB CharDB;
    public Character character;
    public SpriteLibrary SpriteSource;
    public SpriteResolver resolver;
    
    public string player;
    private KeyCode left;
    private KeyCode right;
    private KeyCode up;

    void Start() {
        CharSelected = PlayerPrefs.GetInt("CharId");
        character = CharDB.getChar(CharSelected);
        alt = PlayerPrefs.GetInt("alt");
        player = gameObject.name;
        switch (player) {
            case "player1":
                SpriteSource.spriteLibraryAsset = character.sprites;
                break;
            case "player2":
                SpriteSource.spriteLibraryAsset = character.altSprites;
                break;
            default:
                LoadSprite();
                break;
        }
    }

    void LoadSprite() {
        character = CharDB.getChar(CharSelected);
        if (alt == 0) {
            SpriteSource.spriteLibraryAsset = character.sprites;
        } else {
            SpriteSource.spriteLibraryAsset = character.altSprites;
        }
    }

}
