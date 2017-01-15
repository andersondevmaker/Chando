using UnityEngine;
using System.Collections;

public class ChessKingPiece : ChessPiece {
    public override bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        Piece takenPiece = board[endX, endY];
        int deltaMoveX = endX - startX;
        int deltaMoveY = endY - startY;

        if (deltaMoveY == 1)
        {
            if (Mathf.Abs(deltaMoveX) == 1 || deltaMoveX == 0)
            {
                return true;
            }
        }
       
        // TODO: Handle castling

        return false;
    }
}
