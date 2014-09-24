using UnityEngine;
using System.Collections;

public class ScoreGUI : MonoBehaviour {
    public GUIStyle scoreStyle;
    public GUIStyle teamNameStyle;
    public GameState gameState;
    public Texture2D topTexture;
    public float edgeBuffer = 10.0f;
    public float textWidth = 15.0f;
    public float textHeight = 40.0f;
    public float statusBarHeight = 80.0f;

	// Use this for initialization
	void Start () {
        gameState = GameState._gameStateInstance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // display a bar on top of the screen as a background to the game state status text
        GUI.DrawTexture(new Rect(0, 0, Screen.width, statusBarHeight), topTexture, ScaleMode.StretchToFill, true);
        // show the game state status
        GUI.Label(new Rect(Screen.width/2 - textWidth/2, 10, textWidth, textHeight), gameState.getMessage() , scoreStyle);

        // calculate the offset for the text
        float textOffset = gameState.players[0].teamBanner.width / 2;

        // display a background for the first team's score information
        GUI.Label(new Rect(edgeBuffer, edgeBuffer, gameState.players[0].teamBanner.width, gameState.players[0].teamBanner.height), gameState.players[0].teamBanner);        
        // display the text for the first team's score information
        GUI.Label(new Rect(textOffset, 10, textWidth, textHeight), gameState.players[0].teamName, teamNameStyle);
        GUI.Label(new Rect(textOffset, 50, textWidth, textHeight), "Total Score", scoreStyle);
        GUI.Label(new Rect(textOffset, 85, textWidth, textHeight), gameState.players[0].totalScore.ToString(), scoreStyle);
        GUI.Label(new Rect(textOffset, 120, textWidth, textHeight), "Round Score", scoreStyle);
        GUI.Label(new Rect(textOffset, 155, textWidth, textHeight), gameState.players[0].currentScore.ToString(), scoreStyle);
        GUI.Label(new Rect(textOffset, 190, textWidth, textHeight), "Balls Left", scoreStyle);
        GUI.Label(new Rect(textOffset, 225, textWidth, textHeight), gameState.players[0].numberOfBalls.ToString(), scoreStyle);


        // display a background for the second team's score information
        GUI.Label(new Rect(Screen.width - gameState.players[1].teamBanner.width, 10, gameState.players[1].teamBanner.width, gameState.players[1].teamBanner.height), gameState.players[1].teamBanner);
        // display the text for the second team's score information        
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 10, textWidth, textHeight), gameState.players[1].teamName, teamNameStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 50, textWidth, textHeight), "Total Score", scoreStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 85, textWidth, textHeight), gameState.players[1].totalScore.ToString(), scoreStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 120, textWidth, textHeight), "Round Score", scoreStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 155, textWidth, textHeight), gameState.players[1].currentScore.ToString(), scoreStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 190, textWidth, textHeight), "Balls Left", scoreStyle);
        GUI.Label(new Rect(Screen.width - textOffset - edgeBuffer, 225, textWidth, textHeight), gameState.players[1].numberOfBalls.ToString(), scoreStyle);

    }
}
