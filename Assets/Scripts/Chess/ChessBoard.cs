using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChessBoard : GameBoard
{
    public GameObject pawnPiecePrefab;
    public GameObject rookPiecePrefab;
    public GameObject knightPiecePrefab;
    public GameObject bishopPiecePrefab;
    public GameObject queenPiecePrefab;
    public GameObject kingPiecePrefab;

    protected override void GenerateBoard()
    {
        boardOffset = new Vector2(-5.12f, -5.12f);
        pieceOffset = new Vector2(0.64f, 0.64f);
        scale = 1.28f;
        pieces = new Piece[8, 8];

        int x = 0;
        int[] yCoordinates = new int[2] { 0, 7 };

        foreach (int y in yCoordinates) {
            bool isWhite = (y == 0);
            GeneratePiece(0, y, rookPiecePrefab);
            GeneratePiece(1, y, knightPiecePrefab);
            GeneratePiece(2, y, bishopPiecePrefab);
            GeneratePiece(3, y, kingPiecePrefab);
            GeneratePiece(4, y, queenPiecePrefab);
            GeneratePiece(5, y, bishopPiecePrefab);
            GeneratePiece(6, y, knightPiecePrefab);
            GeneratePiece(7, y, rookPiecePrefab);
        }

        for (x = 0; x < 8; x++)
        {
            GeneratePiece(x , 6, pawnPiecePrefab);
            GeneratePiece(x, 1, pawnPiecePrefab);
        }
    }

    private void GeneratePiece(int x, int y, GameObject piecePrefab)
    {
        bool isWhite = y < 2;
        GameObject pieceGameObject = Instantiate(piecePrefab) as GameObject;
        pieceGameObject.transform.SetParent(transform);
        ChessPiece piece = pieceGameObject.GetComponent<ChessPiece>();
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

    protected override void CheckVictory()
    {
        // TODO: Check victory
        bool victory = false;
        var blackWins = false;

        if(victory)
        {
            Victory(blackWins);
        }
    }
}