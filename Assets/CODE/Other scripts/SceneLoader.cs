using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string levelSceneName = "LevelScene";

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }
}