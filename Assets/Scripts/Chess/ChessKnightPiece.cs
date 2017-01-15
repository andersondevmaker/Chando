using UnityEngine;
using System.Collections;

public class ChessKnightPiece : ChessPiece {
    protected override bool CanJumpPieces
    {
        get { return false; }
    }
}
