using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public Transform canvas;
    public CheckersBoard board;
    public Text victoryText;

    void Start()
    {
        victoryText.gameObject.SetActive(false);
        board.NewGame();
    }

    public void newGame()
    {
        victoryText.gameObject.SetActive(false);
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
        string victoryTextStr = blackVictory ? "BLACK WINS!" : "RED WINS!";
        victoryText.text = victoryTextStr;
        victoryText.gameObject.SetActive(true);
        toggleMenu(true);
    }
}
