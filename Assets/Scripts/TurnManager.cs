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
        PlayerTurn = GameManager.Instance.BoardManager.AmountOfEnemies <= 0 || !PlayerTurn;

        //Debug.Log("Turn " + m_turnCount + ", " + (PlayerTurn ? "Player" : "Enemy"));
        if(PlayerTurn)
        {
            OnPlayerTurn?.Invoke();
        }
        else
        {
            Debug.Log("Amount of enemies: " + GameManager.Instance.BoardManager.AmountOfEnemies);
            EnemyActions = GameManager.Instance.BoardManager.AmountOfEnemies;
            Debug.Log("Enemy Actions: " + EnemyActions);

            OnEnemyTurn?.Invoke();
        }
    }
}
