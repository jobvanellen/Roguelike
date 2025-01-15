using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private bool m_GameOver = false;

    public void Init()
    {
        m_GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_GameOver)
        {
            if(Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if(cellData != null && cellData.isPassable)
            {
                GameManager.Instance.TurnManager.Tick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(cell);
    }

    public void GameOver()
    {
        m_GameOver = true;
    }
}
