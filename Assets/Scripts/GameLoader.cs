using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] float timeBeforeLoadScene = 2f;
    AudioSource audioSource;

    private void Start()
    {
            audioSource = GetComponent<AudioSource>();

    }

    public void LoadStartMenu()
    {
        audioSource.Play();
        SceneManager.LoadScene(0);
    }

    public void DeferGameOverSceneLoading()
    {
        Invoke("LoadGameOver", timeBeforeLoadScene);
    }

    public void LoadGameScene()
    {
        audioSource.Play();
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene(1);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGamee()
    {
        audioSource.Play();
        Application.Quit();
    }
}
