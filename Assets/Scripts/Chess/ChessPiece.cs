using UnityEngine;
using System.Collections;

public abstract class ChessPiece : Piece
{
    public bool HasEverMoved = false; // TODO: Notify piece that it has moved

    protected virtual bool CanJumpPieces
    {
        get { return false; }
    }

    protected bool MovingOntoOwnPiece(Piece[,] board, int endX, int endY)
    {
        Piece takenPiece = board[endX, endY];
        if (takenPiece != null && takenPiece.IsWhite == IsWhite)
        {
            return true;
        }

        return false;
    }

    protected bool IsValidMovementPath(int startX, int startY, int endX, int endY)
    {
        return true;
    }

    protected bool IsMakingInvalidJump(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        if(CanJumpPieces)
        {
            return false;
        }

        int deltaMoveX = endX - startX;
        int deltaMoveY = endY - startY;
        Piece jumpedPiece;
        if (Mathf.Abs(deltaMoveX) > 1 || Mathf.Abs(deltaMoveY) > 1)
        {
            if(Mathf.Abs(deltaMoveX) == Mathf.Abs(deltaMoveY)) // diagonal
            {
                int xIncrement = deltaMoveX > 0 ? 1 : -1;
                int yIncrement = deltaMoveY > 0 ? 1 : -1;
                int currentX = startX + xIncrement;
                int currentY = startY + yIncrement;
                while(currentX != endX)
                {
                    jumpedPiece = board[currentX, currentY];
                    if(jumpedPiece != null)
                    {
                        return true;
                    }

                    currentX += xIncrement;
                    currentY += yIncrement;
                }
            }
            else if (deltaMoveY == 0) // moving x only
            {
                int xIncrement = deltaMoveX > 0 ? 1 : -1;
                int currentX = startX + xIncrement;
                while (currentX != endX)
                {
                    jumpedPiece = board[currentX, startY];
                    if (jumpedPiece != null)
                    {
                        return true;
                    }

                    currentX += xIncrement;
                }
            }
            else if (deltaMoveX == 0 ) // moving y only
            {
                int yIncrement = deltaMoveY > 0 ? 1 : -1;
                int currentY = startY + yIncrement;
                while (currentY != endY)
                {
                    jumpedPiece = board[startX, currentY];
                    if (jumpedPiece != null)
                    {
                        return true;
                    }

                    currentY += yIncrement;
                }
            }
        }

        return false;
    }

    public override bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        // TODO: handle moving into/out of check
        if (MovingOntoOwnPiece(board, endX, endY))
        {
            return false;
        }

        if(IsMakingInvalidJump(board, startX, startY, endX, endY))
        {
            return false;
        }

        return true;
    }
}
