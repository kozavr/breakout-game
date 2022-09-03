using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuManager : MonoBehaviour
{
    // references
    public Button startButton;
    public Button quitButton;
    public TMP_InputField inputField;
    string defaultUserName = "guest";

    // variables

    public void StartGame()
    {
        if (inputField.text != null)
        {
            ScenePersistance.Instance.playerName = inputField.text;
        }
        else
        {
            ScenePersistance.Instance.playerName = defaultUserName;
        }

        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
