using UnityEngine;
using System.Collections;

public class Team
{
    public GameObject bocceBallPrefab;
    public Texture2D teamBanner;
    public string teamName;
    public int numberOfBalls;
    public int currentScore = 0;
    public int totalScore = 0;

    public Team()
    {
        numberOfBalls = Constants.NUMBER_OF_BALLS_PER_TEAM;
        bocceBallPrefab = new GameObject();
    }
    public Team(GameObject ballPrefab, string name)
    {
        teamName = name;
        numberOfBalls = Constants.NUMBER_OF_BALLS_PER_TEAM;
        bocceBallPrefab = ballPrefab;
    }
    public Team(GameObject ballPrefab, string name, Texture2D banner)
    {
        teamName = name;
        numberOfBalls = Constants.NUMBER_OF_BALLS_PER_TEAM;
        bocceBallPrefab = ballPrefab;
        teamBanner = banner;
    }
}
