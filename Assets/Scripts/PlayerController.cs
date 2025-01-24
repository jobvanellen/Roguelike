using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private bool m_GameOver = false;

    private bool m_isMoving;
    private Vector3 m_TargetPosition;


    public InputAction MoveLeft;
    public InputAction MoveRight;
    public InputAction MoveUp;
    public InputAction MoveDown;
    public InputAction SkipTurn;

    public float MoveSpeed = 5.0f;


    private Animator m_Animator;
    private bool m_HasMovedThisTurn = false;


    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        MoveLeft.Enable();
        MoveRight.Enable();
        MoveUp.Enable();
        MoveDown.Enable();
        SkipTurn.Enable();
        GameManager.Instance.TurnManager.OnPlayerTurn += StartTurn;
    }

    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnPlayerTurn -= StartTurn;
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

        if(!m_HasMovedThisTurn)
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        Vector2Int newCellTarget = m_CellPosition;
        if (GameManager.Instance.TurnManager.PlayerTurn)
        {
            if (SkipTurn.WasPressedThisFrame())
            {
                Debug.Log("Skip Player Turn");
                GameManager.Instance.TurnManager.Tick();
                return;
            }
            newCellTarget += InputToMoveDirection();
            m_HasMovedThisTurn = newCellTarget != m_CellPosition;
        }

        if (m_HasMovedThisTurn)
        {
            ProcessPlayerMovement(newCellTarget);
        }
    }

    private Vector2Int InputToMoveDirection()
    {
        if (MoveUp.WasPressedThisFrame())
        {
            return Vector2Int.up;
        }
        else if (MoveDown.WasPressedThisFrame())
        {
            return Vector2Int.down;
        }
        else if (MoveLeft.WasPressedThisFrame())
        {
            return Vector2Int.left;
        }
        else if (MoveRight.WasPressedThisFrame())
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
            GameManager.Instance.UpdateFood(-1);
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
        else
        {
            m_HasMovedThisTurn = false;
        }
    }

    private void StartTurn()
    {
        m_HasMovedThisTurn = false;
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
