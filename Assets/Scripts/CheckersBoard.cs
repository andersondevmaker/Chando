using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckersBoard : MonoBehaviour
{
    public Piece[,] pieces;
    public GameObject redPiecePrefab;
    public GameObject whitePiecePrefab;

    public Vector2 boardOffset;
    public Vector2 pieceOffset;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    private Piece selectedPiece;

    public Text mouseText;

    // Use this for initialization
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        boardOffset = new Vector2(-4f, -4f);
        pieceOffset = new Vector2(0.5f, 0.5f); // Shouldn't need a piece offset. 
        pieces = new Piece[8, 8];
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 3; y++)
            {
                GeneratePiece(x + (y % 2), y);
            }
        }

        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 5; y < 8; y++)
            {
                GeneratePiece(x + (y % 2), y);
            }
        }
    }

    private void GeneratePiece(int x, int y)
    {
        GameObject piecePrefab = y < 4 ? whitePiecePrefab : redPiecePrefab;
        GameObject pieceGameObject = Instantiate(piecePrefab) as GameObject;
        pieceGameObject.transform.SetParent(transform);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        pieces[x, y] = piece;
        MovePiece(piece, x, y);

    }

    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = Vector2.right * x + Vector2.up * y + boardOffset + pieceOffset;
    }

    private void TryMovePiece(int startX, int startY, int endX, int endY)
    {
        startDrag = new Vector2(startX, startY);
        endDrag = new Vector2(endX, endY);

        Piece piece = pieces[startX, startY];

        Debug.Log("Trying to move the piece.");

        if (endX < 0 || endX >= pieces.Length || endY < 0 || endY >= pieces.Length)
        {
            Debug.Log("Out of bounds.");
            if (selectedPiece != null)
            {
                MovePiece(selectedPiece, startX, startY);
            }
            selectedPiece = null;         
            startDrag = Vector2.zero;
            return;
        }

        if(selectedPiece != null)
        {
            if(startDrag == endDrag)
            {
                MovePiece(selectedPiece, startX, startY);
                selectedPiece = null;
                startDrag = Vector2.zero;
                return;
            }
        }

        Debug.Log("Moving the piece.");
        MovePiece(piece, endX, endY);
        pieces[startX, startY] = null;
        pieces[endX, endY] = piece;
        selectedPiece = null;
        startDrag = Vector2.zero;
    }

    private void UpdatePieceDrag(Piece p)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            p.transform.position = hit.point;
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
        {
            return;
        }

        Piece p = pieces[x, y];
        if(p != null)
        {
            selectedPiece = p;
            startDrag = mouseOver;
        }      
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMouseOver();

        // If it is my turn
        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if(selectedPiece != null)
        {
            UpdatePieceDrag(selectedPiece);
        }

        if(Input.GetMouseButtonDown(0))
        {
            SelectPiece(x, y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryMovePiece((int) startDrag.x, (int) startDrag.y, x, y);
        }
    }

    private void UpdateMouseOver()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.y - boardOffset.y);
        }
        else
        {
            mouseOver = new Vector2(-1, -1);
        }

        mouseText.text = "MouseText: (" + (int) mouseOver.x + "," + (int) mouseOver.y + "). ";
    }
}