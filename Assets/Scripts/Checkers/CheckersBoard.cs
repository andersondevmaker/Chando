using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CheckersBoard : GameBoard
{
    public GameObject piecePrefab;
    protected bool hasJumpedPiece;

    public override void NewGame()
    {
        hasJumpedPiece = false;
        base.NewGame();
    }

    protected override void GenerateBoard()
    {
        boardOffset = new Vector2(-5.12f, -5.12f);
        pieceOffset = new Vector2(0.64f, 0.64f);
        scale = 1.28f;
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
        bool isWhite = y < 3;
        GameObject pieceGameObject = Instantiate(piecePrefab) as GameObject;
        pieceGameObject.transform.SetParent(transform);
        Piece piece = pieceGameObject.GetComponent<CheckersPiece>();
        piece.IsWhite = isWhite;
        pieces[x, y] = piece;
        MovePiece(piece, x, y);
    }

    protected override void TryMovePiece(int startX, int startY, int endX, int endY)
    {
        selectedPiece = pieces[startX, startY];
        if (selectedPiece == null)
        {
            return;
        }

        startDrag = new Vector2(startX, startY);
        endDrag = new Vector2(endX, endY);

        if (endX < 0 || endX >= 8 || endY < 0 || endY >= 8)
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

        forcedPieces = FindForcedPieces();

        if (selectedPiece.IsValidMove(pieces, startX, startY, endX, endY))
        {
            // Check if a piece was destroyed
            bool pieceDestroyed = false;
            if (Mathf.Abs(endX - startX) == 2)
            {
                pieceDestroyed = true;
            }

            if(forcedPieces.Count != 0 && !pieceDestroyed)
            {
                MovePiece(selectedPiece, startX, startY);
                ResetSelectedPiece();
                return;
            }

            if (pieceDestroyed)
            {
                pieceDestroyed = true;
                Piece destroyedPiece = pieces[(startX + endX) / 2, (startY + endY) / 2];
                pieces[(startX + endX) / 2, (startY + endY) / 2] = null;
                Destroy(destroyedPiece.gameObject);
                hasJumpedPiece = true;
            }

            pieces[endX, endY] = selectedPiece;
            pieces[startX, startY] = null;
            MovePiece(selectedPiece, endX, endY);

            // If the selected piece can keep jumping then the turn is not complete.
            if (selectedPiece.IsForcedToMove(pieces, endX, endY) && hasJumpedPiece)
            {
                activePiece = selectedPiece;
                ResetSelectedPiece();
            }
            else
            {
                EndTurn();
            }
        }
        else
        {
            MovePiece(selectedPiece, startX, startY);
            ResetSelectedPiece();
        }
    }

    protected override void CheckVictory()
    {
        var hasWhitePieces = false;
        var hasBlackPieces = false;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece piece = pieces[x, y];
                if (piece != null)
                {
                    if (piece.IsWhite)
                    {
                        hasWhitePieces = true;
                    }
                    else
                    {
                        hasBlackPieces = true;
                    }
                }
            }
        }

        if(!hasWhitePieces || !hasBlackPieces)
        {
            Victory(hasBlackPieces);
        }
    }
}