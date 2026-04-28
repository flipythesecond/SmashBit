using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SceneScript : MonoBehaviour
{

    public GameObject level;
        
    // show level
    public void ShowLevel()
    {
        level.SetActive(true);
    }

    // hide level (optional)
    public void HideLevel()
    {
        level.SetActive(false);
    }

   public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("MainGame");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
