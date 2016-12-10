using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class GameBoard : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public Piece[,] pieces;

    protected Vector2 boardOffset;
    protected Vector2 pieceOffset;
    protected float scale;

    protected Vector2 mouseOver;
    protected Vector2 startDrag;
    protected Vector2 endDrag;

    protected List<Piece> forcedPieces;
    protected Piece activePiece;

    protected bool isWhiteTurn;
    protected Piece selectedPiece;

    protected abstract void GenerateBoard();
    protected abstract void TryMovePiece(int startX, int startY, int endX, int endY);

    public virtual void NewGame()
    {
        DisposeOfPreviousBoard();
        activePiece = null;
        isWhiteTurn = true;
        startDrag = new Vector2(-1, -1);
        endDrag = new Vector2(-1, -1);
        forcedPieces = new List<Piece>();

        GenerateBoard();
    }

    protected virtual void DisposeOfPreviousBoard()
    {
        if (pieces != null)
        {
            foreach(Piece p in pieces)
            { 
                if (p != null)
                {
                    Destroy(p.gameObject);
                }
            }
        }
    }

    protected void MovePiece(Piece p, int x, int y)
    {
        if(p != null)
        {
            p.PositionUpdated(x, y);
        }

        p.transform.position = Vector2.right * x * scale + Vector2.up * y * scale + boardOffset + pieceOffset;
    }

    protected virtual List<Piece> FindForcedPieces()
    {
        forcedPieces = new List<Piece>();

        if (activePiece != null)
        {
            forcedPieces.Add(activePiece);
            return forcedPieces;
        }

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece p = pieces[x, y];
                if (p && ((p.IsWhite && isWhiteTurn) || (!p.IsWhite && !isWhiteTurn)))
                {
                    if (p.IsForcedToMove(pieces, x, y))
                    {
                        forcedPieces.Add(p);
                    }
                }
            }
        }

        return forcedPieces;
    }

    protected virtual void ResetSelectedPiece()
    {
        selectedPiece = null;
        startDrag = new Vector2(-1, -1);
    }

    protected virtual void EndTurn()
    {
        activePiece = null;
        ResetSelectedPiece();
        CheckVictory();
        isWhiteTurn = !isWhiteTurn;
    }

    protected abstract void CheckVictory();

    protected void Victory(bool blackVictory)
    {
        gameStateManager.GameOver(blackVictory);
    }

    protected void UpdatePieceDrag(Piece p)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            p.transform.position = hit.point;
        }
    }

    protected virtual void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
        {
            return;
        }

        forcedPieces = FindForcedPieces();
        Piece p = pieces[x, y];
        if (p != null && p.IsWhite == isWhiteTurn)
        {
            if (forcedPieces.Count == 0 || forcedPieces.Contains(p))
            {
                selectedPiece = p;
                startDrag = mouseOver;
            }
        }
    }

    // Update is called once per frame
    protected void Update()
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

    protected void UpdateMouseOver()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            mouseOver.x = (int)((hit.point.x - boardOffset.x)/scale);
            mouseOver.y = (int)((hit.point.y - boardOffset.y)/scale);
        }
        else
        {
            mouseOver = new Vector2(-1, -1);
        }
    }
}