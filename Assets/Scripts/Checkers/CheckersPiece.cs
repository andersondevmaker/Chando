using UnityEngine;
using System.Collections;

public class CheckersPiece : Piece {
    public bool isKing = false;

    public Sprite blackKingSprite;
    public Sprite whiteKingSprite;

    protected override void UpdateSprite()
    {
        if (spriteRenderer)
        {
            if(isKing)
            {
                if (IsWhite)
                {
                    spriteRenderer.sprite = whiteKingSprite;
                }
                else
                {
                    spriteRenderer.sprite = blackKingSprite;
                }
            }
            else
            {
                base.UpdateSprite();
            }
        }
    }

    public override void PositionUpdated(int x, int y)
    {
        // If a piece reaches the opposite end mark it as a king and change its sprite accordingly. 
        if ((y == 7 && IsWhite) || (y == 0 && !IsWhite))
        {
            isKing = true;
            UpdateSprite();
        }
    }

    public override bool IsForcedToMove(Piece[,] board, int x, int y)
    {
        CheckersPiece p;
        if (IsWhite || isKing)
        {
            // Check if there is an enemy piece above and to the left, and a place to land after jumping the piece. 
            if (x >= 2 && y <= 5)
            {
                p = (CheckersPiece) board[x - 1, y + 1];
                if(p != null && p.IsWhite != IsWhite && board[x - 2, y + 2] == null)
                {
                    return true;
                }
            }

            // Check if there is an enemy piece above and to the right, and a place to land after jumping the piece. 
            if (x <= 5 && y <= 5)
            {
                p = (CheckersPiece) board[x + 1, y + 1];
                if (p != null && p.IsWhite != IsWhite && board[x + 2, y + 2] == null)
                {
                    return true;
                }
            }
        }

        if (!isWhite || isKing)
        {
            // Check if there is an enemy piece below and to the left, and a place to land after jumping the piece. 
            if (x >= 2 && y >= 2)
            {
                p = (CheckersPiece) board[x - 1, y - 1];
                if (p != null && p.IsWhite != IsWhite && board[x - 2, y - 2] == null)
                {
                    return true;
                }
            }

            // Check if there is an enemy piece below and to the right, and a place to land after jumping the piece. 
            if (x <= 5 && y >= 2)
            {
                p = (CheckersPiece) board[x + 1, y - 1];
                if (p != null && p.IsWhite != IsWhite && board[x + 2, y - 2] == null)
                {
                    return true;
                }
            }
        }

        return false;
    }

	public override bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        if (board[endX, endY] != null)
            return false;
        
        int deltaMoveX = Mathf.Abs(endX - startX);
        int deltaMoveY =  endY - startY;

        CheckersPiece p;
        // White pieces
        if (IsWhite || isKing)
        {
            if(deltaMoveX == 1)
            {
                if (deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if(deltaMoveX == 2)
            {
                if (deltaMoveY == 2)
                {
                    p = (CheckersPiece) board[(startX + endX) / 2, (startY + endY) / 2];
                    if (p != null && p.IsWhite != IsWhite)
                    {
                        return true;
                    }
                }
            }
        }

        // Black pieces
        if (!IsWhite || isKing)
        {
            if (deltaMoveX == 1)
            {
                if (deltaMoveY == -1)
                {
                    return true;
                }
            }
            else if (deltaMoveX == 2)
            {
                if (deltaMoveY == -2)
                {
                    p = (CheckersPiece) board[(startX + endX) / 2, (startY + endY) / 2];
                    if (p != null && p.IsWhite != IsWhite)
                        return true;
                }
            }
        }

        return false;

    }
}
