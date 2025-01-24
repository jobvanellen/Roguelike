using UnityEngine;

public class TurnManager
{
    public event System.Action OnPlayerTurn;
    public event System.Action OnEnemyTurn;
    private int m_turnCount;

    private int EnemyActions;
    public int AmountOfEnemies { get; set; }

    public bool PlayerTurn {  get; private set; }

    public TurnManager()
    {
        m_turnCount = 1;
    }

    public void Init()
    {
        AmountOfEnemies = 0;
        PlayerTurn = true;
        OnPlayerTurn.Invoke();
    }

    public void Tick()
    {
        if (!PlayerTurn)
        {
            EnemyActions++;
            if (EnemyActions < AmountOfEnemies)
            {
                return;
            }
        }

        PlayerTurn = AmountOfEnemies <= 0 || !PlayerTurn;
        m_turnCount++;
        EnemyActions = 0;

        if (PlayerTurn)
        {
            OnPlayerTurn?.Invoke();
        }
        else
        {
            OnEnemyTurn?.Invoke();
        }
    }
}
