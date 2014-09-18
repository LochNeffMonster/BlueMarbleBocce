using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum State { Menu, Palino_Throw, Bocce_Throw, Game_Over }

public class Team
{
    public GameObject bocceBallPrefab;
    public string teamName;
    public int numberOfBalls;
    public int currentScore = 0;
    private int maxScore;

    public Team()
    {
        numberOfBalls = maxScore = Constants.NUMBER_OF_BALLS_PER_TEAM;
        bocceBallPrefab = new GameObject();
    }
    public Team(GameObject ballPrefab, string name)
    {
        teamName = name;
        numberOfBalls = maxScore = Constants.NUMBER_OF_BALLS_PER_TEAM;
        bocceBallPrefab = ballPrefab;
    }
}

public class Pair<T, U>
{
    public Pair()
    {

    }
    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }
    public T First { get; set; }
    public U Second { get; set; }
}

public class GameState : MonoBehaviour {

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
    public GameObject playerPrefab;
    public GameObject bocceBallPrefabTeam1;
    public GameObject bocceBallPrefabTeam2;
    public GameObject palinoBallPrefab;
    public Team[] players;
    private List<GameObject> ballSet;
    public Team currentTeam;
    public GameObject currentPalinoBall = null;
    public State gameState;
    public GUIStyle scoreStyle;

    private int ballsThrown = 0;



	// Use this for initialization
	void Start () {
        _gameStateInstance = this;
        players = new Team[Constants.NUMBER_OF_PLAYERS];
        players.Initialize();
        players[0] = new Team(bocceBallPrefabTeam1, "Blue Team");
        players[1] = new Team(bocceBallPrefabTeam2, "Red Team");
        currentTeam = players[0];

        ballSet = new List<GameObject>();
        gameState = State.Palino_Throw;
        GameObject.Instantiate(playerPrefab);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2, 10, 200, 40), "Balls Thrown: " + ballsThrown, scoreStyle);
    }

    public GameObject requestBall()
    {
        GameObject ball;
        switch (gameState)
        {
            case State.Menu:
                // should not return ball
                ball = null;
                break;
            case State.Palino_Throw:
                //return palino ball instance
                ball = (GameObject)GameObject.Instantiate(palinoBallPrefab);
                break;
            case State.Bocce_Throw:
                // return bocce ball instance
                ball = (GameObject) GameObject.Instantiate(currentTeam.bocceBallPrefab);
                ball.name = currentTeam.teamName;
                break;
            case State.Game_Over:
                ball = null;
                break;
            default:
                //don't return a ball
                ball =  null;
                break;
        }
        return ball;
    }

    public void updateGame()
    {
        //ballsThrown++;
        // calculate score
        calculateScore();
        // display updated score
        // modify game state
        gameState = determineGameState();
        if (gameState == State.Game_Over)
        {
            // display winner
        }
        if(ballsThrown !=0)
            currentTeam = players[ballsThrown % 2];
    }

    public State determineGameState()
    {
        State currentGameState;

        if (currentPalinoBall == null)
        {
            currentGameState = State.Palino_Throw;
        }
        else if(ballsThrown >= Constants.NUMBER_OF_BALLS_PER_TEAM * 2)
        {
            currentGameState = State.Game_Over;
        }else // there are still bocce balls to be thrown
        {
            currentGameState = State.Bocce_Throw;
        }
        return currentGameState;
    }



    public void addBallToSet(GameObject ball)
    {
        if (currentPalinoBall == null)
        {
            currentPalinoBall = ball;
            Debug.Log("added Pallino ball");
        } else {
            ballSet.Add(ball);
            ballsThrown++;
            Debug.Log("added Bocce ball");
        }     
    }
    public void calculateScore()
    {
        List<Pair<string, float>> bocceSortList = new List<Pair<string, float>>();
        Pair<string, float> closestPair;
        Pair<string, float> firstOutTeamPair;

        if (ballsThrown < 2)
        {
            return;
        }
        else {
            foreach(GameObject ball in ballSet){
                Pair<string, float> boccePair = new Pair<string, float>();
                boccePair.Second = Vector3.Distance(ball.transform.position, currentPalinoBall.transform.position);
                boccePair.First = ball.name;
                bocceSortList.Add(boccePair);
            }
            bocceSortList.Sort((x, y) => x.Second.CompareTo(y.Second));
            closestPair = bocceSortList[0];
            firstOutTeamPair = bocceSortList.Find(item => item.First != closestPair.First);
            int indexOfOutTeamBall = bocceSortList.IndexOf(firstOutTeamPair);
            Debug.Log("List has " + bocceSortList.Count + " elements in it");
            Debug.Log("bocce in team is " + bocceSortList[0].First);
            Debug.Log("Team " + closestPair.First + " wins with a score of " + indexOfOutTeamBall);
            bocceSortList.Clear();
        }
    }

    public State getState()
    {
        return gameState;
    }
}
