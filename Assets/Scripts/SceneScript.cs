using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneScript : MonoBehaviour
{

   
   public void LoadScene()
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
