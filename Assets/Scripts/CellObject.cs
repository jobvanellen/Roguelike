using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_Cell;

    virtual public void PlayerEntered()
    {

    }

    virtual public void Init(Vector2Int cell)
    {
        m_Cell = cell;
    }

    virtual public bool PlayerWantsToEnter()
    {
        return true;
    }
}
