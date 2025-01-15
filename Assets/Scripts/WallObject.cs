using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public Tile BrokenTile;
    public int Max_HP = 3;

    private int m_HP;

    private Tile m_OriginalTile;


    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HP = Max_HP;
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        GameManager.Instance.PlayerController.Attack();
        m_HP--;
        if(m_HP == 1)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, BrokenTile);
        }
        if(m_HP <= 0)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
            Destroy(gameObject);
        }
        return false;
    }
}
