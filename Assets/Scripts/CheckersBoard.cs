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
    private Vector2 startDrag = new Vector2(-1, -1);
    private Vector2 endDrag = new Vector2(-1, -1);

    private bool isWhite;
    private bool isWhiteTurn;
    private Piece selectedPiece;

    // Use this for initialization
    void Start()
    {
        isWhiteTurn = true;
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        boardOffset = new Vector2(-4f, -4f);
        pieceOffset = new Vector2(0.5f, 0.5f);
        pieces = new Piece[8, 8];
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 3; y++)
            {
                GeneratePiece(x + (y % 2), y);
            }

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
        if(p != null)
        {
            p.PositionUpdated(x, y);
        }

        p.transform.position = Vector2.right * x + Vector2.up * y + boardOffset + pieceOffset;
    }

    private void TryMovePiece(int startX, int startY, int endX, int endY)
    {
        selectedPiece = pieces[startX, startY];
        if (selectedPiece == null)
        {
            return;
        }

        startDrag = new Vector2(startX, startY);
        endDrag = new Vector2(endX, endY);

        if (endX < 0 || endX >= pieces.Length || endY < 0 || endY >= pieces.Length)
        {
            MovePiece(selectedPiece, startX, startY);           
            ResetSelectedPiece();
            return;
        }

        if(startDrag == endDrag)
        {
            MovePiece(selectedPiece, startX, startY);
            ResetSelectedPiece();
            return;
        }

        if (selectedPiece.IsValidMove(pieces, startX, startY, endX, endY))
        {
            // Check if a piece was destroyed
            if (Mathf.Abs(endX - startX) == 2)
            {
                Piece destroyedPiece = pieces[(startX + endX) / 2, (startY + endY) / 2];
                pieces[(startX + endX) / 2, (startY + endY) / 2] = null;
                Destroy(destroyedPiece.gameObject);        
            }

            pieces[endX, endY] = selectedPiece;
            pieces[startX, startY] = null;
            MovePiece(selectedPiece, endX, endY);
            EndTurn();
        }
        else
        {
            MovePiece(selectedPiece, startX, startY);
            ResetSelectedPiece();
        }
    }

    private void ResetSelectedPiece()
    {
        selectedPiece = null;
        startDrag = new Vector2(-1, -1);
    }

    private void EndTurn()
    {
        ResetSelectedPiece();
        isWhiteTurn = !isWhiteTurn;
    }

    private void CheckVictory()
    {
        var pieces = FindObjectsOfType<Piece>();
        var hasWhitePieces = false;
        var hasBlackPieces = false;
        foreach (Piece piece in pieces)
        {
            if(piece.isWhite)
            {
                hasWhitePieces = true;
            } else
            {
                hasBlackPieces = true;
            }
        }

        if(!hasWhitePieces || !hasBlackPieces)
        {
            Victory(hasWhitePieces);
        }
    }

    private void Victory(bool whiteVictory)
    {

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
        if(p != null && p.isWhite == isWhiteTurn)
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
            int startX = (int)startDrag.x;
            int startY = (int)startDrag.y;
            if (startX >= 0 && startY >= 0) {
                TryMovePiece(startX, startY, x, y);
            }
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
    }
}