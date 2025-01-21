using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private bool m_GameOver = false;

    private bool m_isMoving;
    private Vector3 m_TargetPosition;

    public float MoveSpeed = 5.0f;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Init()
    {
        m_GameOver = false;
        m_isMoving = false;
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

        if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, MoveSpeed * Time.deltaTime);
            if (transform.position == m_TargetPosition)
            {
                m_isMoving = false;
                GameManager.Instance.TurnManager.Tick();
                checkPickup();
            }
        }

        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;
        if (GameManager.Instance.TurnManager.PlayerTurn)
        {
            newCellTarget += InputToMoveDirection();
            hasMoved = newCellTarget != m_CellPosition;
        }

        if (hasMoved)
        {
            ProcessPlayerMovement(newCellTarget);
        }
    }

    private Vector2Int InputToMoveDirection()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            return Vector2Int.up;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            return Vector2Int.down;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            return Vector2Int.left;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            return Vector2Int.right;
        }
        else
        {
            return Vector2Int.zero;
        }
    }

    private void ProcessPlayerMovement(Vector2Int newCellTarget)
    {
        if (m_isMoving)
        {
            MoveDirectlyTo(new Vector2Int((int)m_TargetPosition.x, (int)m_TargetPosition.y));
        }

        BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

        if (cellData != null && cellData.isPassable)
        {
            if (cellData.ContainedObject == null)
            {
                MoveSmoothlyTo(newCellTarget);
            }
            else if (cellData.ContainedObject.PlayerWantsToEnter())
            {
                MoveSmoothlyTo(newCellTarget);
            }
            else
            {
                GameManager.Instance.TurnManager.Tick();
            }
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveDirectlyTo(cell);
    }

    public void checkPickup()
    {
        var cellData = m_Board.GetCellData(m_CellPosition);
        if (cellData.ContainedObject != null)
        {
            cellData.ContainedObject.PlayerEntered();
        }
    }

    private void MoveSmoothlyTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        m_isMoving = true;
        m_TargetPosition = m_Board.CellToWorld(cell);
    }

    private void MoveDirectlyTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(cell);
        checkPickup();
    }

    public void GameOver()
    {
        m_GameOver = true;
    }

    public void Attack()
    {
        m_Animator.SetTrigger("Attack");    
    }

    public Vector2Int GetCellPosition()
    {
        return m_CellPosition;
    }

    public void GetHit()
    {
        m_Animator.SetTrigger("GetHit");
    }
}
