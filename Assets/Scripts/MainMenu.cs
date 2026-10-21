using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string firstScene = "Bedroom";
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject creditsPanel;

    void Start() => ShowMenu();

    public void StartGame() => SceneManager.LoadScene(firstScene);

    public void ShowCredits()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void ShowMenu()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
