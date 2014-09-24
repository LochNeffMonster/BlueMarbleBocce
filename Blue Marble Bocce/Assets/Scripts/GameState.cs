using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum State { Menu, Pallino_Throw, Bocce_Throw, Game_Over }

public class GameState : MonoBehaviour {

    // set up the gamestate as a singleton so that it can be accessed from other scripts
    public static GameState _gameStateInstance;
    public static GameState gameStateInstance
    {
        get
        {
            if (_gameStateInstance == null)
            {

            }
            return _gameStateInstance;
        }
    }
    // System Objects 
    public GameObject playerPrefab;
    public GameObject guiPrefab;
    public MouseLook mouseLook;

    // Game Objects
    public GameObject bocceBallPrefabTeam1;
    public GameObject bocceBallPrefabTeam2;
    public GameObject pallinoBallPrefab;

    // Game State variables
    public State gameState;
    public List<Team> players;
    private List<GameObject> ballSet;
    public Team currentTeam;
    public Team currentWinningTeam;
    public Team currentLosingTeam;
    public GameObject currentPallinoBall = null;
    private int ballsThrown = 0;

    // Members for GUI methods
    public Texture2D redTeamBanner;
    public Texture2D blueTeamBanner;
    public string gameStateMessage;
    public string gameMenuMessage;
    public delegate void MenuFunction();
    public MenuFunction menuFunction;


	// Used for initialization
	void Start () {
        _gameStateInstance = this;
        players = new List<Team>();
        players.Add(new Team(bocceBallPrefabTeam1, "BLUE TEAM", blueTeamBanner));
        players.Add(new Team(bocceBallPrefabTeam2, "RED TEAM", redTeamBanner));
        currentTeam = players[0];

        ballSet = new List<GameObject>();


        GameObject.Instantiate(playerPrefab);
        // now that the player is created, store a reference to its MouseLook component
        mouseLook = (MouseLook)FindObjectOfType(typeof(MouseLook));
        GameObject.Instantiate(guiPrefab);

        mouseLook.toggleMouseLock();
        menuFunction = resetGame;
        gameMenuMessage = "Play Next Round";
        gameState = State.Menu;


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // updateGame is used to update the current score, game state, and implement turns for each player
    public void updateGame()
    {
        // calculate score
        calculateScore();
        // modify game state
        gameState = determineGameState();
        if (gameState == State.Menu)
        {
            // winner text is displayed by a separate component
            // the game will remain in this state until the player uses the menu GUI to reset the game
        }else if (ballsThrown != 0) {
            // alternate between players as long as there are still balls to be thrown
            currentTeam = players[ballsThrown % 2];
            // updates the GUI to show which player's turn it is
            gameStateMessage = currentTeam.teamName + "'s Turn";
        }
    }


    // function used by BallController to receive the appropriate kind of ball given the game state
    // returns an instance of a pallino ball or bocce ball prefab
    //          unless it's not time to throw a ball, in which case null is returned.
    public GameObject requestBall()
    {
        GameObject ball;
        switch (gameState)
        {
            case State.Menu:
                // should not return ball
                ball = null;
                break;
            case State.Pallino_Throw:
                //return palino ball instance
                ball = (GameObject)GameObject.Instantiate(pallinoBallPrefab);
                break;
            case State.Bocce_Throw:
                // return bocce ball instance
                ball = (GameObject) GameObject.Instantiate(currentTeam.bocceBallPrefab);
                ball.name = currentTeam.teamName;
                break;
            default:
                //don't return a ball
                ball =  null;
                break;
        }
        return ball;
    }


    // determineGameState returns the state of the game after checking the game state variables
    // returns States Pallino_Throw, Bocce_Throw, or Menu
    public State determineGameState()
    {
        State currentGameState;

        // If a pallino ball has not been set, then the state is activated to begin the game
        if (currentPallinoBall == null)
        {
            currentGameState = State.Pallino_Throw;
            gameStateMessage = currentTeam.teamName + "'s pallino throw";
        }
        // If all the balls have been thrown for a round, an appriopriate menuFunction is set 
        // in order to start the next round or game
        else if(ballsThrown >= Constants.NUMBER_OF_BALLS_PER_TEAM * 2)
        {
            // set the state to menu so that player can have a moment to look at the game results
            currentGameState = State.Menu;
            mouseLook.toggleMouseLock();

            // add the round score to the in-team's score
            currentWinningTeam.totalScore += currentWinningTeam.currentScore;

            // if the team has reached the max score possible, end the game
            if (currentWinningTeam.totalScore >= Constants.MAX_SCORE)
            {
                currentGameState = State.Menu;
                gameStateMessage = currentWinningTeam.teamName + " Wins!!!!";
                gameMenuMessage = "Play Next Game";
                menuFunction = resetGame;
            }
            // if the max score has not been reached, switch to the next round
            else
            {
                currentGameState = State.Menu;
                gameStateMessage = currentWinningTeam.teamName + " wins this round";
                gameMenuMessage = "Play Next Round";
                menuFunction = resetRound;
            }
            
        }else // there are still bocce balls to be thrown
        {
            currentGameState = State.Bocce_Throw;
        }
        return currentGameState;
    }


    // addBallToSet stores the input object so that the score can be calculated using its position relative to other balls
    public void addBallToSet(GameObject ball)
    {
        // if there is no pallino ball yet, the input ball must be it
        if (currentPallinoBall == null)
        {
            currentPallinoBall = ball;
        } 
        // add the ball to the set so that it can be used for calculating the score
        else {
            ballSet.Add(ball);
            ballsThrown++;
            currentTeam.numberOfBalls--;
        }     
    }

    // calculateScore using the distance between the pallino and bocce balls to determine who has won a round
    public void calculateScore()
    {
        // A list for storing each bocce balls distance to the pallino, which preserving its team affiliation
        List<Pair<string, float>> bocceSortList = new List<Pair<string, float>>();
        Pair<string, float> closestPair;
        Pair<string, float> firstOutTeamPair;

        // if each team doesn't have a ball out, don't bother calculating the score
        if (ballsThrown < 2)
        {
            return;
        }
        else {
            // go through each ball on the court and calculate the distance from it to the pallino ball
            foreach(GameObject ball in ballSet){
                Pair<string, float> boccePair = new Pair<string, float>();
                boccePair.Second = Vector3.Distance(ball.transform.position, currentPallinoBall.transform.position);
                boccePair.First = ball.name;
                bocceSortList.Add(boccePair);
            }
            // now that all the distances are calculated, sort the balls in ascending order
            bocceSortList.Sort((x, y) => x.Second.CompareTo(y.Second));
            // get the closest ball to the pallino and set the in-team to whoever threw it
            closestPair = bocceSortList[0];
            firstOutTeamPair = bocceSortList.Find(item => item.First != closestPair.First);
            // find the first ball from the out-team
            int indexOfOutTeamBall = bocceSortList.IndexOf(firstOutTeamPair);

            // go through the players to set their current score
            foreach (Team player in players)
            {
                // if the closest ball has the same team name, set their score
                if (player.teamName.Equals(closestPair.First))
                {
                    // since the index of the first out team ball also equals the number of closest in-team balls
                    // set the in-team score to that number
                    player.currentScore = indexOfOutTeamBall;
                    currentWinningTeam = player;
                }
                // if player is the out team, set their score to zero
                else
                {
                    player.currentScore = 0;
                    currentLosingTeam = player;
                }
            }
        }
    }

/// RESET FUNCTIONS
/// These functions are used to reset the game for either the next round or game
/// 

    // Resets all the round variables so that a new round can be started
    public void resetRound()
    {
        // changes the current player to the last round's losing team
        currentTeam = currentLosingTeam;
        clearCourt();

        // reverses the team list if necessary in order to work with the generalized turn system
        if (!currentLosingTeam.teamName.Equals(players[0].teamName))
        {
            players.Reverse();
        }
        // unlocks the mouse so that the player can aim
        mouseLook.toggleMouseLock();
        // sets the state so that the next ball will be the pallino ball
        gameState = State.Pallino_Throw;
    }

    // Resets all game values so that a new game can be started
    public void resetGame()
    {
        foreach (Team player in players)
        {
            player.totalScore = 0;
        }
        currentTeam = players[0];
        clearCourt();
        // unlocks the mouse so that the player can aim
        mouseLook.toggleMouseLock();
    }

    // Clears the court of all the current balls on the field
    // also resets the ball and score values for the players
    public void clearCourt()
    {
        ballsThrown = 0;
        foreach (GameObject ball in ballSet)
        {
            GameObject.Destroy(ball);
        }
        ballSet.Clear();
        foreach (Team player in players)
        {
            player.currentScore = 0;
            player.numberOfBalls = Constants.NUMBER_OF_BALLS_PER_TEAM;
        }
        GameObject.Destroy(currentPallinoBall);
        currentPallinoBall = null;
        updateGame();
    }

    /// Getters and Setters
    /// //////
    /// These are simple functions to set or get data from the game state

    // Returns the state of the game
    public State getState()
    {
        return gameState;
    }

    // Returns the current game state message which communicates the state of the game
    public string getMessage(){
        return gameStateMessage;
    }

    // Returns the appropriate menu button text given the state of the game
    public string getMenuMessage()
    {
        return gameMenuMessage;
    }

}
