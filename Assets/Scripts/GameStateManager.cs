using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {
    public Transform canvas;
    public Transform victoryText;
    public CheckersBoard board;
    

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleMenu();
        }
    }

    public void newGame()
    {
        board.NewGame();
        toggleMenu(false);
    }

    private void toggleMenu(bool menuEnabled)
    {
        canvas.gameObject.SetActive(menuEnabled);
        board.enabled = !menuEnabled;
    }

    public void toggleMenu()
    {
        toggleMenu(!canvas.gameObject.activeInHierarchy);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void GameOver(bool blackVictory)
    {
        // victoryText\
    }
}
