using UnityEngine;

public class FoodObject : CellObject
{
    public int FoodValue;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.UpdateFood(FoodValue);
    }
}
