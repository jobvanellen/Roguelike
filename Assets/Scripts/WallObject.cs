using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public int Max_HP = 3;
    private int HP;

    private Tile m_OriginalTile;


    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        HP = Max_HP;
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        HP--;
        if(HP <= 0)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
