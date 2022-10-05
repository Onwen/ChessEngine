using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    [Flags]
    public enum MoveDetails
    {
        //flags
        promotion               = 0b1000,
        capture                 = 0b0100,
        special_a               = 0b0010,
        special_b               = 0b0001,
        //composite
        quiet_move              = 0b0000,
        double_pawn_push        = 0b0001,
        king_castle             = 0b0010,
        queen_castle            = 0b0011,
        captures                = 0b0100,
        ep_capture              = 0b0101,
        knight_promotion        = 0b1000,
        bishop_promotion        = 0b1001,
        rook_promotion          = 0b1010,
        queen_promotion         = 0b1011,
        knight_promo_capture    = 0b1100,
        bishop_promo_capture    = 0b1101,
        rook_promo_capture      = 0b1110,
        queen_promo_capture     = 0b1111
    }

    [Flags]
    public enum CastlingRights
    {
        White_KingSide  =   0b1000,
        White_QueenSide =   0b0100,
        Black_KingSide  =   0b0010,
        Black_QueenSide =   0b0001
    }

    [Flags]
    public enum PieceTypes
    {
        Pawn = 1,
        Knight = 2,
        Bishop = 4,
        Rook = 8,
        Queen = 16,
        King = 32,

        Black = 64,
        White = 128,

        white_pawn = Pawn | White,
        white_knight = Knight | White,
        white_bishop = Bishop | White,
        white_rook = Rook | White,
        white_queen = Queen | White,
        white_king = King | White,

        black_pawn = Pawn | Black,
        black_knight = Knight | Black,
        black_bishop = Bishop | Black,
        black_rook = Rook | Black,
        black_queen = Queen | Black,
        black_king = King | Black,
    }
    public enum PieceIndex
    {
        white_pawn = 0,
        white_knight = 1,
        white_bishop = 2,
        white_rook = 3,
        white_queen = 4,
        white_king = 5,
        white_all = 6,

        black_pawn = 7,
        black_knight = 8,
        black_bishop = 9,
        black_rook = 10,
        black_queen = 11,
        black_king = 12,
        black_all = 13,

        all = 14,

        total = 15
    }
}
