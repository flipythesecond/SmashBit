using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneScript : MonoBehaviour
{
   public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
