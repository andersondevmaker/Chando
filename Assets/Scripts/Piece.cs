using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
    public bool isWhite;
    public bool isKing = false;

    private SpriteRenderer spriteRenderer;
    public Sprite peasantSprite;
    public Sprite kingSprite;

    private void Start()
    {
        // Ensure the right sprite for a pawn
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = peasantSprite;
        }
    }

    public void PositionUpdated(int x, int y)
    {
        // If a piece reaches the opposite end mark it as a king and change its sprite accordingly. 
        if ((y == 7 && isWhite) || (y == 0 && !isWhite))
        {
            isKing = true;
            spriteRenderer.sprite = kingSprite;
        }
    }

    public bool IsForcedToMove(Piece[,] board, int x, int y)
    {
        if(isWhite || isKing)
        {
            // Check if there is an enemy piece above and to the left, and a place to land after jumping the piece. 
            if (x >= 2 && y <= 5)
            {
                Piece p = board[x - 1, y + 1];
                if(p != null && p.isWhite != isWhite && board[x - 2, y + 2] == null)
                {
                    return true;
                }
            }

            // Check if there is an enemy piece above and to the right, and a place to land after jumping the piece. 
            if (x <= 5 && y <= 5)
            {
                Piece p = board[x + 1, y + 1];
                if (p != null && p.isWhite != isWhite && board[x + 2, y + 2] == null)
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
                Piece p = board[x - 1, y - 1];
                if (p != null && p.isWhite != isWhite && board[x - 2, y - 2] == null)
                {
                    return true;
                }
            }

            // Check if there is an enemy piece below and to the right, and a place to land after jumping the piece. 
            if (x <= 5 && y >= 2)
            {
                Piece p = board[x + 1, y - 1];
                if (p != null && p.isWhite != isWhite && board[x + 2, y - 2] == null)
                {
                    return true;
                }
            }
        }

        return false;
    }

	public bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        if (board[endX, endY] != null)
            return false;
        
        int deltaMoveX = Mathf.Abs(endX - startX);
        int deltaMoveY =  endY - startY;

        // White pieces
        if(isWhite || isKing)
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
                    Piece p = board[(startX + endX) / 2, (startY + endY) / 2];
                    if (p != null && p.isWhite != isWhite)
                    {
                        return true;
                    }
                }
            }
        }

        // Black pieces
        if (!isWhite || isKing)
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
                    Piece p = board[(startX + endX) / 2, (startY + endY) / 2];
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        return false;

    }
}
