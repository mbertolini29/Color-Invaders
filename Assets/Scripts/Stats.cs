using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int life;
    public int score;
    public int highestScoreght;

    public Stats()
    {
        life = 3;
        score = 0;
        highestScoreght = 0;
    }
}
