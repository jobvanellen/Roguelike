using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager;
    public TurnManager TurnManager { get; private set; }
    public PlayerController PlayerController;

    public GameObject FoodUpdatePrefab;

    public int InitialFood;
    private int m_FoodAmount;

    public UIDocument UIDoc;
    private Label m_FoodLabel;

    private int m_CurrentLevel = 1;
    private Label m_LevelLabel;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverLabel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        TurnManager = new TurnManager();
    }

    void Start()
    {     
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverLabel = m_GameOverPanel.Q<Label>("GameOverMessage");

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_LevelLabel = UIDoc.rootVisualElement.Q<Label>("LevelLabel");

        StartNewGame();
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        m_FoodAmount = InitialFood;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        m_CurrentLevel = 0;
        PlayerController.Init();
        NewLevel();
    }

    public void NewLevel()
    {
        BoardManager.ClearLevel();

        TurnManager.Init();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        m_CurrentLevel++;
        m_LevelLabel.text = "Level : " + m_CurrentLevel;
    }

    public void UpdateFood(int amount)
    {
        IndicateFoodChange(amount);

        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + (m_FoodAmount < 0 ? "0" : m_FoodAmount) ;

        if (m_FoodAmount <= 0)
        {
            PlayerController.GameOver();
            m_GameOverLabel.text = "Game Over\n\nYou traveled through " + m_CurrentLevel + " level" + (m_CurrentLevel == 1 ? "" : "s") + "\n\nPress ENTER to restart";
            m_GameOverPanel.style.visibility = Visibility.Visible;
        }
    }

    private void IndicateFoodChange(int amount)
    {
        GameObject foodUpdate = Instantiate(FoodUpdatePrefab, PlayerController.transform);

        var textMeshPro = foodUpdate.transform.GetChild(0).GetComponent<TextMeshPro>();
        textMeshPro.SetText((amount > 0 ? "+" : "") + amount.ToString());
        textMeshPro.color = amount < 0 ? Color.red : Color.green;
    }
}
