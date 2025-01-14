using UnityEngine;

public class TurnManager
{
    private int m_turnCount;
    
    public TurnManager()
    {
        m_turnCount = 1;
    }

    public void Tick()
    {
        m_turnCount++;
        Debug.Log("Turn " + m_turnCount);
    }




}
