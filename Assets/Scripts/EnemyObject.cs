using UnityEngine;

public class EnemyObject : CellObject
{
    public int maxHP;

    private int m_HP;

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

    }
}
