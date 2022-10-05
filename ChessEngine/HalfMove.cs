using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class HalfMove
    {
        private static int _fromMask = 0b1111110000000000;
        private static int _toMask = 0b0000001111110000;
        private static int _detailMask = 0b0000000000001111;
        private short _data;
        public PieceTypes Piece
        {
            get
            {
                if (parentState.state[(int)PieceIndex.white_all].GetBit(From))
                {
                    if (parentState.state[(int)PieceIndex.white_queen].GetBit(From))
                    {
                        return PieceTypes.white_queen;
                    }
                    if (parentState.state[(int)PieceIndex.white_rook].GetBit(From))
                    {
                        return PieceTypes.white_rook;
                    }
                    if (parentState.state[(int)PieceIndex.white_bishop].GetBit(From))
                    {
                        return PieceTypes.white_bishop;
                    }
                    if (parentState.state[(int)PieceIndex.white_knight].GetBit(From))
                    {
                        return PieceTypes.white_knight;
                    }
                    if (parentState.state[(int)PieceIndex.white_pawn].GetBit(From))
                    {
                        return PieceTypes.white_pawn;
                    }
                    if (parentState.state[(int)PieceIndex.white_king].GetBit(From))
                    {
                        return PieceTypes.white_king;
                    }
                }
                else
                {
                    if (parentState.state[(int)PieceIndex.black_queen].GetBit(From))
                    {
                        return PieceTypes.black_queen;
                    }
                    if (parentState.state[(int)PieceIndex.black_rook].GetBit(From))
                    {
                        return PieceTypes.black_rook;
                    }
                    if (parentState.state[(int)PieceIndex.black_bishop].GetBit(From))
                    {
                        return PieceTypes.black_bishop;
                    }
                    if (parentState.state[(int)PieceIndex.black_knight].GetBit(From))
                    {
                        return PieceTypes.black_knight;
                    }
                    if (parentState.state[(int)PieceIndex.black_pawn].GetBit(From))
                    {
                        return PieceTypes.black_pawn;
                    }
                    if (parentState.state[(int)PieceIndex.black_king].GetBit(From))
                    {
                        return PieceTypes.black_king;
                    }
                }

                return 0;
            }
        }
        public int From
        {
            get
            {
                return (_data & _fromMask) >> 10;
            }
        }
        public int To
        {
            get
            {
                return (_data & _toMask) >> 4;
            }
        }
        public MoveDetails Details
        {
            get
            {
                return (MoveDetails)(_data & _detailMask);
            }
        }
        public BoardState parentState;
        public BoardState resultState;

        public HalfMove(MoveDetails details, int from, int to, BoardState parentState)
        {
            _data = (short)(from << 10);
            _data |= (short)(to << 4);
            _data |= (short)(_detailMask & (int)details);
            this.parentState = parentState;
            this.resultState = new BoardState(this.parentState);
            ApplyMove();
        }
        public void ApplyMove()
        {
            foreach (var s in resultState.state)
            {
                s.SetBitOff(From);
                s.SetBitOff(To);
            }
            //resultState.epTarg = 0;
            var piece = Piece;
            //handle promotions
            if ((Details & MoveDetails.promotion) == MoveDetails.promotion)
            {
                //piece promoted
                switch (Details)
                {
                    case MoveDetails.queen_promotion:
                    case MoveDetails.queen_promo_capture:
                        resultState.state[(int)((piece & PieceTypes.White) != 0 ? PieceIndex.white_queen : PieceIndex.black_queen)].SetBitOn(To);
                        break;
                    case MoveDetails.rook_promotion:
                    case MoveDetails.rook_promo_capture:
                        resultState.state[(int)((piece & PieceTypes.White) != 0 ? PieceIndex.white_rook : PieceIndex.black_rook)].SetBitOn(To);
                        break;
                    case MoveDetails.bishop_promotion:
                    case MoveDetails.bishop_promo_capture:
                        resultState.state[(int)((piece & PieceTypes.White) != 0 ? PieceIndex.white_bishop : PieceIndex.black_bishop)].SetBitOn(To);
                        break;
                    case MoveDetails.knight_promotion:
                    case MoveDetails.knight_promo_capture:
                        resultState.state[(int)((piece & PieceTypes.White) != 0 ? PieceIndex.white_knight : PieceIndex.black_knight)].SetBitOn(To);
                        break;
                }
            }
            else if (Details == MoveDetails.double_pawn_push)
            {
                var v = piece.TypeToIndex();
                resultState.state[(int)v].SetBitOn(To);
                resultState.epTarg = (To - From) / 2 + From;
            }
            else if (Details == MoveDetails.ep_capture)
            {
                var v = piece.TypeToIndex();
                resultState.state[(int)v].SetBitOn(To);

                //file of the target(epTarg) & rank of attacker from
                int dpx = parentState.epTarg % 8;
                int dpy = From / 8;
                var enemySide = piece & (PieceTypes.White | PieceTypes.Black);
                if (enemySide == PieceTypes.White)
                    enemySide = PieceTypes.Black;
                else if (enemySide == PieceTypes.Black)
                    enemySide = PieceTypes.White;
                var enemyP = (PieceTypes.Pawn | enemySide).TypeToIndex();
                resultState.state[(int)enemyP].SetBitOff(dpx + dpy * 8);
            }
            else if (Details == MoveDetails.king_castle)
            {
                var v = piece.TypeToIndex();
                var side = piece & (PieceTypes.White | PieceTypes.Black);
                var rook = ((piece & side) | PieceTypes.Rook).TypeToIndex();
                resultState.state[(int)v].SetBitOn(To);
                resultState.state[(int)rook].SetBitOff(To + 1);
                resultState.state[(int)rook].SetBitOn(To - 1);
            }
            else if (Details == MoveDetails.queen_castle)
            {
                var v = piece.TypeToIndex();
                var side = piece & (PieceTypes.White | PieceTypes.Black);
                var rook = ((piece & side) | PieceTypes.Rook).TypeToIndex();
                resultState.state[(int)v].SetBitOn(To);
                resultState.state[(int)rook].SetBitOff(To -2);
                resultState.state[(int)rook].SetBitOn(To + 1);
            }
            else
            {
                var v = piece.TypeToIndex();
                resultState.state[(int)v].SetBitOn(To);
            }

            resultState.castleRights = parentState.castleRights;
            //handle castling after effects if rook or kign was moved
            if ((piece & (PieceTypes.King | PieceTypes.Rook)) != 0)
            {
                //either king or rook was moved
                var side = piece & (PieceTypes.White | PieceTypes.Black);

                if ((piece & PieceTypes.white_king) == PieceTypes.white_king)
                {
                    resultState.castleRights &= CastlingRights.Black_KingSide | CastlingRights.Black_QueenSide;
                }
                if ((piece & PieceTypes.black_king) == PieceTypes.black_king)
                {
                    resultState.castleRights &= CastlingRights.White_KingSide | CastlingRights.White_QueenSide;
                }
                if ((piece & PieceTypes.white_rook) == PieceTypes.white_rook)
                {
                    //kingside rook
                    if (From == 7)
                        resultState.castleRights &= CastlingRights.White_QueenSide | CastlingRights.Black_QueenSide | CastlingRights.Black_KingSide;
                    //queenside rook
                    if (From == 0)
                        resultState.castleRights &= CastlingRights.White_KingSide | CastlingRights.Black_QueenSide | CastlingRights.Black_KingSide;
                }
                if ((piece & PieceTypes.black_rook) == PieceTypes.black_rook)
                {
                    //kingside rook moved
                    if (From == 63)
                        resultState.castleRights &= CastlingRights.Black_QueenSide | CastlingRights.White_QueenSide | CastlingRights.White_KingSide;
                    //queenside rook moved
                    if (From == 56)
                        resultState.castleRights &= (CastlingRights.Black_KingSide | CastlingRights.White_QueenSide | CastlingRights.White_KingSide);
                }
            }


            resultState.RecalcSharedIndexs();
            resultState.ToggleTurn();
        }
    }
}
