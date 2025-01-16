using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TurnManager
{
    public event System.Action OnPlayerTurn;
    public event System.Action OnEnemyTurn;
    private int m_turnCount;

    public int EnemyActions { get; set; }

    public bool PlayerTurn {  get; private set; }
    
    public TurnManager()
    {
        m_turnCount = 1;
        Init();
    }

    public void Init()
    {
        PlayerTurn = true;
    }

    public void Tick()
    {
        m_turnCount++;
        PlayerTurn = GameManager.Instance.BoardManager.AmountOfEnemies > 0 ? !PlayerTurn : true;

        Debug.Log("Turn " + m_turnCount + ", " + (PlayerTurn ? "Player" : "Enemy"));
        if(PlayerTurn)
        {
            OnPlayerTurn?.Invoke();
        }
        else
        {
            EnemyActions = GameManager.Instance.BoardManager.AmountOfEnemies;
            OnEnemyTurn?.Invoke();
        }
    }
}
