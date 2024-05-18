using System.Numerics;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager> {
    private const int InitialLifes = 1;
    private int kills = 0;
    public int Kills { 
        get => kills; 
        set {
            kills = value;
            UI_Manager.Instance.ApplyKills();
        } 
    }
    private int lifes = InitialLifes;
    public int Lifes {
        get => lifes; 
        set {
            lifes = value;
            UI_Manager.Instance.ApplyLifes();
            if(lifes <= 0)
                GameOver();
        } 
    }
    public BigInteger totalDamage = 0;
    private bool isGame = true;
    public bool IsGame => isGame;

    private void Start() {
        GameStart();
    }

    public void GameStart() {
        isGame = true;
        kills = 0;
        lifes = InitialLifes;
        totalDamage = 0;
        UI_Manager.Instance.ApplyKills();
        UI_Manager.Instance.ApplyLifes();
        EnemyList.Instance.DeleteAll();
        TowerController.Instance.DeleteAll();
        TowerController.Instance.MakeRandomTowers(2);
        UI_Manager.Instance.HideGameResults();
    }

    private void GameOver() {
        isGame = false;
        UI_Manager.Instance.ShowGameResults();
    }
}
