using UnityEngine;
using System.Collections;

public abstract class Piece : MonoBehaviour {
    public Sprite whiteSprite;
    public Sprite blackSprite;

    protected SpriteRenderer spriteRenderer;
    protected bool isWhite = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsWhite
    {
        get
        {
            return isWhite;
        }
        set
        {
            isWhite = value;
            UpdateSprite();
        }
    }

    protected virtual void UpdateSprite()
    {
        if (spriteRenderer)
        {
            if (isWhite)
            {
                spriteRenderer.sprite = whiteSprite;
            }
            else
            {
                spriteRenderer.sprite = blackSprite;
            }
        }
    }


    public virtual void PositionUpdated(int x, int y)
    {
    }

    public virtual bool IsForcedToMove(Piece[,] board, int x, int y)
    {
        return false;
    }

	public virtual bool IsValidMove(Piece[,] board, int startX, int startY, int endX, int endY)
    {
        return true;

    }
}
