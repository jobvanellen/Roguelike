using UnityEngine;

public class EnemyObject : CellObject
{
    public int maxHP;

    private int m_HP;

    private Animator m_animator;


    private void Awake()
    {
        GameManager.Instance.TurnManager.OnEnemyTurn += OnTurn;
        m_animator = GetComponent<Animator>();
        GameManager.Instance.BoardManager.AmountOfEnemies++;
    }
    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnEnemyTurn -= OnTurn;
        GameManager.Instance.BoardManager.AmountOfEnemies--;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HP = maxHP;
    }

    public override bool PlayerWantsToEnter()
    {
        GameManager.Instance.PlayerController.Attack();

        m_HP--;

        if (m_HP <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }
    public void OnTurn()
    {
        var playerCell = GameManager.Instance.PlayerController.GetCellPosition();
        int xDistance = m_Cell.x - playerCell.x;
        int yDistance = m_Cell.y - playerCell.y;

        if(xDistance == 0 && Mathf.Abs(yDistance) == 1 ||
            yDistance == 0 && Mathf.Abs(xDistance) == 1)
        {
            m_animator.SetTrigger("Attack");
            GameManager.Instance.PlayerController.GetHit();

            GameManager.Instance.UpdateFood(-3);
            
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
        GameManager.Instance.TurnManager.EnemyActions--;
        Debug.Log("Enemy Actions: " + GameManager.Instance.TurnManager.EnemyActions);
    }

    private bool TryMoveInX(int dist)
    { 
        return dist > 0 ? MoveTo(m_Cell + Vector2Int.left) : MoveTo(m_Cell + Vector2Int.right);
    }

    private bool TryMoveInY(int dist)
    {
        return dist > 0 ? MoveTo(m_Cell + Vector2Int.down) : MoveTo(m_Cell + Vector2Int.up);
    }

    private bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null
            || !targetCell.isPassable
            || targetCell.ContainedObject != null)
        {
            return false;
        }

        //remove enemy from current cell
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        //add it to the next cell
        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }


}
