using UnityEngine;
using System.Collections;

public class menuGUI : MonoBehaviour {
    public float textWidth = 15.0f;
    public float textHeight = 10.0f;
    public GUIStyle menuStyle;
    public Texture2D menuTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GameState._gameStateInstance.getState() == State.Menu){
            Rect menuTextureRect = new Rect((Screen.width - menuTexture.width) / 2, (Screen.height - menuTexture.height) / 2, menuTexture.width, menuTexture.height);
            GUI.DrawTexture(menuTextureRect, menuTexture);
            if(GUI.Button(new Rect((Screen.width - textWidth)/2, (Screen.height - textHeight)/2, textWidth, textHeight), GameState._gameStateInstance.getMenuMessage(), menuStyle)){
                GameState._gameStateInstance.menuFunction();
            }
        }
    }
}
