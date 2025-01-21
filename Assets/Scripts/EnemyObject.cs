using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyObject : CellObject
{
    public int maxHP;
    public float MoveSpeed;

    private int m_HP;

    private Animator m_animator;

    private bool m_DoAction = false;
    private bool m_IsMoving = false;
    private bool m_Attack = false;

    private Vector3 m_TargetPosition;

 

    private void Awake()
    {
        GameManager.Instance.TurnManager.OnEnemyTurn += OnTurn;
        GameManager.Instance.TurnManager.AmountOfEnemies++;
        m_animator = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnEnemyTurn -= OnTurn;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HP = maxHP;
    }

    private void Update()
    {
        if(GameManager.Instance.TurnManager.PlayerTurn || !m_DoAction)
        {
            return;
        }

        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, MoveSpeed * Time.deltaTime);
            if (transform.position == m_TargetPosition)
            {
                m_IsMoving = false;
                m_DoAction = false;
                m_animator.SetBool("Walk", false);
                GameManager.Instance.TurnManager.Tick();
            }
        }
        else if (m_Attack)
        {
            m_animator.SetTrigger("Attack");
            GameManager.Instance.UpdateFood(-3);
            m_DoAction = false;
            m_Attack = false;
            GameManager.Instance.PlayerController.GetHit();
            GameManager.Instance.TurnManager.Tick();
        }
        else if (m_DoAction)
        {
            // if Enemy can do an action but no specific action was selected, turn is skipped
            m_DoAction = false;
            GameManager.Instance.TurnManager.Tick();
        }
    }
    public override bool PlayerWantsToEnter()
    {
        GameManager.Instance.PlayerController.Attack();

        m_HP--;

        if (m_HP <= 0)
        {
            Debug.Log("Enemy killed");
            GameManager.Instance.TurnManager.AmountOfEnemies--;
            Destroy(gameObject);
        }
        return false;
    }
    public void OnTurn()
    {
        m_DoAction = true;

        var playerCell = GameManager.Instance.PlayerController.GetCellPosition();
        int xDistance = m_Cell.x - playerCell.x;
        int yDistance = m_Cell.y - playerCell.y;

        if(NextToPlayer(playerCell))
        {
            m_Attack = true;
        }
        else if (Mathf.Abs(xDistance) > Mathf.Abs(yDistance))
        {
            if (!TryMoveInX(xDistance))
            {
                TryMoveInY(yDistance);
            }
        }
        else
        {
            if (!TryMoveInY(yDistance))
            {
                TryMoveInX(xDistance);
            }
        }
    }

    private bool NextToPlayer(Vector2Int playerCell)
    {
        return playerCell == m_Cell + Vector2Int.up ||
               playerCell == m_Cell + Vector2Int.down ||
               playerCell == m_Cell + Vector2Int.left ||
               playerCell == m_Cell + Vector2Int.right;
    }

    private bool TryMoveInX(int dist)
    { 
        return dist > 0 ? MoveTo(m_Cell + Vector2Int.left) : MoveTo(m_Cell + Vector2Int.right);
    }

    private bool TryMoveInY(int dist)
    {
        return dist > 0 ? MoveTo(m_Cell + Vector2Int.down) : MoveTo(m_Cell + Vector2Int.up);
    }

    private bool MoveTo(Vector2Int cell)
    {
        var board = GameManager.Instance.BoardManager;
        var newCellData = board.GetCellData(cell);

        if (newCellData == null
            || !newCellData.isPassable
            || newCellData.ContainedObject != null)
        {
            return false;
        }

        var currentCellData = board.GetCellData(m_Cell);
        currentCellData.ContainedObject = null;

        newCellData.ContainedObject = this;

        MoveSmoothlyTo(cell);

        return true;
    }

    private void MoveSmoothlyTo(Vector2Int cell)
    {
        m_animator.SetBool("Walk", true);
        m_IsMoving = true;
        m_Cell = cell;
        m_TargetPosition = GameManager.Instance.BoardManager.CellToWorld(cell);
    }

}
