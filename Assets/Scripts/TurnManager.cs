using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    private int m_turnCount;
    
    public TurnManager()
    {
        m_turnCount = 1;
    }

    public void Tick()
    {
        m_turnCount++;
        Debug.Log("Turn " + m_turnCount);
        OnTick?.Invoke();
    }
}
