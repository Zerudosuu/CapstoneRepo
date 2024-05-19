using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ExperiencePoints { get; private set; }
    public int Coins { get; private set; }
    public int Energy { get; private set; }
    public int Level { get; private set; }

    void Awake()
    {
        ExperiencePoints = 0;
        Coins = 99999;
        Energy = 8;
        Level = 1;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() { }

    public void IncreaseExperience(int points)
    {
        ExperiencePoints += points;

        if (ExperiencePoints >= 100)
        {
            Level++;
            ExperiencePoints = 0;
        }
    }

    public void IncreaseCoins(int coins)
    {
        Coins += coins;
    }

    public void DecreaseCoins(int coins)
    {
        Coins -= coins;
    }

    public void ResetGame()
    {
        ExperiencePoints = 0;
        Coins = 0;
    }
}
