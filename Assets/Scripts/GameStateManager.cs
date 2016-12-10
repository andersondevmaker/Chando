using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public Transform canvas;
    public CheckersBoard checkersBoard;
    public ChessBoard chessBoard;
    private GameBoard activeBoard;
    public Text victoryText;
    public Button resumeButton;

    void Start()
    {
        newCheckersGame();
        toggleMenu(false);
    }

    public void newCheckersGame()
    {
        victoryText.gameObject.SetActive(false);
        activateBoard(checkersBoard);
        activeBoard.NewGame();
        toggleMenu(false);
        resumeButton.enabled = true;
    }

    public void newChessGame()
    {
        victoryText.gameObject.SetActive(false);
        activateBoard(chessBoard);
        activeBoard.NewGame();
        toggleMenu(false);
        resumeButton.enabled = true;
    }

    private void activateBoard(GameBoard board)
    {
        victoryText.gameObject.SetActive(false);
        checkersBoard.gameObject.SetActive(false);
        chessBoard.gameObject.SetActive(false);
        checkersBoard.enabled = false;
        chessBoard.enabled = false;

        activeBoard = board;
        activeBoard.gameObject.SetActive(true);
        activeBoard.enabled = true;
    }

    private void toggleMenu(bool menuEnabled)
    {
        canvas.gameObject.SetActive(menuEnabled);
        activeBoard.enabled = !menuEnabled;
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
