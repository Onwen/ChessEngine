using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public static class Extensions
    {
        public static PieceTypes FENtoType(this char piece)
        {
            switch (piece)
            {
                case 'p':
                    return PieceTypes.Black | PieceTypes.Pawn;
                case 'n':
                    return PieceTypes.Black | PieceTypes.Knight;
                case 'b':
                    return PieceTypes.Black | PieceTypes.Bishop;
                case 'r':
                    return PieceTypes.Black | PieceTypes.Rook;
                case 'q':
                    return PieceTypes.Black | PieceTypes.Queen;
                case 'k':
                    return PieceTypes.Black | PieceTypes.King;
                case 'P':
                    return PieceTypes.White | PieceTypes.Pawn;
                case 'N':
                    return PieceTypes.White | PieceTypes.Knight;
                case 'B':
                    return PieceTypes.White | PieceTypes.Bishop;
                case 'R':
                    return PieceTypes.White | PieceTypes.Rook;
                case 'Q':
                    return PieceTypes.White | PieceTypes.Queen;
                case 'K':
                    return PieceTypes.White | PieceTypes.King;
            }
            return 0;
        }

        public static PieceIndex TypeToIndex(this PieceTypes type)
        {
            switch (type)
            {
                case PieceTypes.white_pawn:
                    return PieceIndex.white_pawn;
                case PieceTypes.white_knight:
                    return PieceIndex.white_knight;
                case PieceTypes.white_bishop:
                    return PieceIndex.white_bishop;
                case PieceTypes.white_rook:
                    return PieceIndex.white_rook;
                case PieceTypes.white_queen:
                    return PieceIndex.white_queen;
                case PieceTypes.white_king:
                    return PieceIndex.white_king;
                case PieceTypes.black_pawn:
                    return PieceIndex.black_pawn;
                case PieceTypes.black_knight:
                    return PieceIndex.black_knight;
                case PieceTypes.black_bishop:
                    return PieceIndex.black_bishop;
                case PieceTypes.black_rook:
                    return PieceIndex.black_rook;
                case PieceTypes.black_queen:
                    return PieceIndex.black_queen;
                case PieceTypes.black_king:
                    return PieceIndex.black_king;
            }
            return PieceIndex.white_pawn;
        }
    }
}
