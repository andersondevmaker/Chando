using UnityEngine;
using System.Collections;

public class ChessPawnPiece : ChessPiece {
    public override bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        if(!base.IsValidMove(board, startX, startY, endX, endY))
        {
            return false;
        }

        Piece takenPiece = board[endX, endY];
        int deltaMoveX = Mathf.Abs(endX - startX);
        int deltaMoveY = endY - startY;
        if(IsWhite)
        {
            if(CheckMoveIsValidHelper(startY, deltaMoveX, deltaMoveY, endY, takenPiece != null))
            {
                return true;
            }
        }
        else
        {
            if (CheckMoveIsValidHelper(startY, deltaMoveX, -1 * deltaMoveY, endY, takenPiece != null))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckMoveIsValidHelper(int startY, int deltaMoveX, int deltaMoveY, int endY, bool pieceTaken)
    {
        if (deltaMoveX == 0)
        {
            if (deltaMoveY == 1 || (!HasEverMoved && deltaMoveY == 2))
            {
                if (!pieceTaken)
                {
                    return true;
                }
            }
        }
        else if (deltaMoveX == 1 && deltaMoveY == 1)
        {         
            if (pieceTaken)
            {
                return true;
            }
        }

        return false;
    }
}
