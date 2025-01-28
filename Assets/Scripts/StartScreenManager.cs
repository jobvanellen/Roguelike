using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public InputAction StartGameAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGameAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartGameAction.triggered)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
