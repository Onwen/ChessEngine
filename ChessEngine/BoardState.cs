using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Perft
    {
        public long moveCount, captures, enpassant, castles, promotions, checks;
        public Perft()
        {

        }
        public Perft(long moveCount, long captures, long enpassant, long castles, long promotions, long checks)
        {
            this.moveCount = moveCount;
            this.captures = captures;
            this.enpassant = enpassant;
            this.castles = castles;
            this.promotions = promotions;
            this.checks = checks;
        }
        public static Perft operator +(Perft a, Perft b)
        => new Perft(a.moveCount + b.moveCount, a.captures + b.captures, a.enpassant + b.enpassant, a.castles + b.castles, a.promotions + b.promotions, a.checks + b.checks);
    }
    public class BoardState
    {
        public BitSet[] state;
        public PieceTypes Turn;
        public int epTarg;
        public CastlingRights castleRights;
        public string BoardFEN
        {
            get
            {
                return GetBoardFEN();
            }
        }

        private string GetBoardFEN()
        {
            string fen = string.Empty;
            char[,] board = new char[8, 8];
            for (int i = 0; i < 64; ++i)
            {
                int x = i % 8;
                int y = i / 8;
                board[x, y] = '0';
                if (state[(int)PieceIndex.white_all].GetBit(i))
                {
                    if (state[(int)PieceIndex.white_pawn].GetBit(i))
                    {
                        board[x, y] = 'P';
                    }
                    if (state[(int)PieceIndex.white_knight].GetBit(i))
                    {
                        board[x, y] = 'N';
                    }
                    if (state[(int)PieceIndex.white_bishop].GetBit(i))
                    {
                        board[x, y] = 'B';
                    }
                    if (state[(int)PieceIndex.white_rook].GetBit(i))
                    {
                        board[x, y] = 'R';
                    }
                    if (state[(int)PieceIndex.white_queen].GetBit(i))
                    {
                        board[x, y] = 'Q';
                    }
                    if (state[(int)PieceIndex.white_king].GetBit(i))
                    {
                        board[x, y] = 'K';
                    }
                }
                if (state[(int)PieceIndex.black_all].GetBit(i))
                {
                    if (state[(int)PieceIndex.black_pawn].GetBit(i))
                    {
                        board[x, y] = 'p';
                    }
                    if (state[(int)PieceIndex.black_knight].GetBit(i))
                    {
                        board[x, y] = 'n';
                    }
                    if (state[(int)PieceIndex.black_bishop].GetBit(i))
                    {
                        board[x, y] = 'b';
                    }
                    if (state[(int)PieceIndex.black_rook].GetBit(i))
                    {
                        board[x, y] = 'r';
                    }
                    if (state[(int)PieceIndex.black_queen].GetBit(i))
                    {
                        board[x, y] = 'q';
                    }
                    if (state[(int)PieceIndex.black_king].GetBit(i))
                    {
                        board[x, y] = 'k';
                    }
                }
            }

            int count = 0;
            for (int i = 0; i < 64; ++i)
            {
                int x = i % 8;
                int y = 7 - (i / 8);

                if (i != 0 && x == 0)
                {
                    if (count > 0)
                        fen += $"{count}/";
                    else
                        fen += $"/";
                    count = 0;
                }

                if (board[x,y] != '0')
                {
                    if (count > 0)
                        fen += $"{count}{board[x, y]}";
                    else
                        fen += $"{board[x, y]}";
                    count = 0;
                }
                else if(board[x, y] == '0')
                {
                    count++;
                }
            }
            if (count > 0)
                fen += $"{count}";

            if (Turn == PieceTypes.White)
                fen += " w";
            else
                fen += " b";

            string castle = "";
            if ((castleRights & CastlingRights.White_KingSide) > 0)
                castle += "K";
            if ((castleRights & CastlingRights.White_QueenSide) > 0)
                castle += "Q";
            if ((castleRights & CastlingRights.Black_KingSide) > 0)
                castle += "k";
            if ((castleRights & CastlingRights.Black_QueenSide) > 0)
                castle += "q";

            if (string.IsNullOrWhiteSpace(castle) == false)
                fen += $" {castle}";
            else
                fen += $" -";

            int epx = epTarg % 8;
            int epy = epTarg / 8;

            if (epTarg == 0)
                fen += $" -";
            else
            {
                fen += $" {(char)(epx + 'a')}{epy + 1}";
            }

            return fen;
        }

        public BoardState(BoardState parentState)
        {
            state = new BitSet[(int)PieceIndex.total];
            for (int i = 0; i < (int)PieceIndex.total; ++i)
            {
                state[i] = new BitSet(parentState.state[i].bits);
            }
            Turn = parentState.Turn;
        }

        public BoardState(string FEN)
        {
            state = new BitSet[(int)PieceIndex.total];
            for (int i = 0; i < (int)PieceIndex.total; ++i)
            {
                state[i] = new BitSet(0);
            }
            int currCell = 0;

            var rem = FEN.Split(' ');

            if (rem.Length < 3)
                throw new Exception("FEN string ended unexpectedly");

            if (rem[1].Contains('b'))
                Turn = PieceTypes.Black;
            if (rem[1].Contains('w'))
                Turn = PieceTypes.White;

            if (rem[2] != "-")
            {
                if (rem[2].Contains('K'))
                    castleRights |= CastlingRights.White_KingSide;
                if (rem[2].Contains('Q'))
                    castleRights |= CastlingRights.White_QueenSide;
                if (rem[2].Contains('k'))
                    castleRights |= CastlingRights.Black_KingSide;
                if (rem[2].Contains('q'))
                    castleRights |= CastlingRights.Black_QueenSide;
            }

            if (rem[3] != "-")
            {
                int epx = rem[3][0] - 'a';
                int epy = rem[3][1] - '1';
                epTarg = epx + epy * 8;
            }

            string pieces = rem[0];

            foreach (var c in pieces)
            {
                //if letter spawn piece in cell
                if (c >= 'A' && c <= 'Z')
                {
                    //white piece
                    SpawnInFENCell(currCell, c);
                    currCell++;
                }
                else if (c >= 'a' && c <= 'z')
                {
                    //black piece
                    SpawnInFENCell(currCell, c);
                    currCell++;
                }
                else if (c >= '0' && c <= '9')
                {
                    //skip cells
                    currCell += int.Parse(c.ToString());
                    continue;
                }
                //if number skip # cells
            }

            RecalcSharedIndexs();
        }

        public void RecalcSharedIndexs()
        {
            state[(int)PieceIndex.white_all] = state[(int)PieceIndex.white_pawn] | state[(int)PieceIndex.white_knight] | state[(int)PieceIndex.white_bishop] | state[(int)PieceIndex.white_rook] | state[(int)PieceIndex.white_queen] | state[(int)PieceIndex.white_king];
            state[(int)PieceIndex.black_all] = state[(int)PieceIndex.black_pawn] | state[(int)PieceIndex.black_knight] | state[(int)PieceIndex.black_bishop] | state[(int)PieceIndex.black_rook] | state[(int)PieceIndex.black_queen] | state[(int)PieceIndex.black_king];
            state[(int)PieceIndex.all] = state[(int)PieceIndex.white_all] | state[(int)PieceIndex.black_all];
        }

        private void SpawnInFENCell(int fenCell, char piece)
        {
            //convert fencell to x,y
            int x = fenCell % 8;
            int y = 8 - (fenCell / 8) - 1;
            //determine piece to spawn
            var type = piece.FENtoType();
            var index = type.TypeToIndex();
            var i = x + y * 8;
            state[(int)index].SetBitOn(i);
        }

        public void ToggleTurn()
        {
            if ((Turn & PieceTypes.White) != 0)
            {
                Turn = PieceTypes.Black;
            }
            else
            {
                Turn = PieceTypes.White;
            }
        }

        public Perft Perft(int depth, Perft perft = null)
        {
            if (perft == null)
            {
                perft = new Perft();
            }

            var moves = FindValidMoves();

            if (depth == 1)
            {
                perft.moveCount += moves.Count;
                var v = moves.Where(b => (b.Details & MoveDetails.capture) != 0);
                perft.captures += moves.Where(b => (b.Details & MoveDetails.capture) != 0).Count();
                perft.castles += moves.Where(b => b.Details == MoveDetails.king_castle || b.Details == MoveDetails.queen_castle).Count();
                perft.enpassant += moves.Where(b => b.Details == MoveDetails.ep_capture).Count();
                perft.promotions += moves.Where(b => (b.Details & MoveDetails.promotion) == MoveDetails.promotion).Count();
                return perft;
            }

            foreach (var m in moves)
            {
                perft += m.resultState.Perft(depth - 1);
            }

            return perft;
        }


        public List<HalfMove> FindValidMoves()
        {
            List<HalfMove> moves = new List<HalfMove>();

            var t = Turn & (PieceTypes.White | PieceTypes.Black);
            if (t == 0 || t == (PieceTypes.White | PieceTypes.Black))
            {
                //error scenario
            }

            bool inCheck = IsKingInCheck(Turn);
            for (int i = 0; i < 64; ++i)
            {
                if (Turn == PieceTypes.White && state[(int)PieceIndex.white_all].GetBit(i))
                {
                    bool isPinned = IsPinned(PieceTypes.White, KingSquares[0], i) || inCheck;
                    //determine pawn moves
                    if (state[(int)PieceIndex.white_pawn].GetBit(i))
                    {
                        CalculateWhitePawnMoves(i, moves, isPinned);
                    }
                    //determine knight moves
                    else if (state[(int)PieceIndex.white_knight].GetBit(i))
                    {
                        CalculateKnightMoves(i, PieceTypes.white_knight, PieceIndex.white_all, PieceIndex.black_all, moves, isPinned);
                    }
                    //determine bishop moves
                    else if (state[(int)PieceIndex.white_bishop].GetBit(i))
                    {
                        CalculateBishopMoves(i, PieceTypes.white_bishop, PieceIndex.white_all, PieceIndex.black_all, moves, isPinned);
                    }
                    //determine rook moves
                    else if (state[(int)PieceIndex.white_rook].GetBit(i))
                    {
                        CalculateRookMoves(i, PieceTypes.white_rook, PieceIndex.white_all, PieceIndex.black_all, moves, isPinned);
                    }
                    //determine queen moves
                    else if (state[(int)PieceIndex.white_queen].GetBit(i))
                    {
                        CalculateQueenMoves(i, PieceTypes.white_queen, PieceIndex.white_all, PieceIndex.black_all, moves, isPinned);
                    }
                    //determine king moves
                    else if (state[(int)PieceIndex.white_king].GetBit(i))
                    {
                        CalculateWhiteKingMoves(i, moves);
                    }
                }
                if (Turn == PieceTypes.Black && state[(int)PieceIndex.black_all].GetBit(i))
                {
                    bool isPinned = IsPinned(PieceTypes.Black, KingSquares[1], i) || inCheck;
                    //determine pawn moves
                    if (state[(int)PieceIndex.black_pawn].GetBit(i))
                    {
                        CalculateBlackPawnMoves(i, moves, isPinned);
                    }
                    //determine knight moves
                    else if (state[(int)PieceIndex.black_knight].GetBit(i))
                    {
                        CalculateKnightMoves(i, PieceTypes.black_knight, PieceIndex.black_all, PieceIndex.white_all, moves, isPinned);
                    }
                    //determine bishop moves
                    else if (state[(int)PieceIndex.black_bishop].GetBit(i))
                    {
                        CalculateBishopMoves(i, PieceTypes.black_bishop, PieceIndex.black_all, PieceIndex.white_all, moves, isPinned);
                    }
                    //determine rook moves
                    else if (state[(int)PieceIndex.black_rook].GetBit(i))
                    {
                        CalculateRookMoves(i, PieceTypes.black_rook, PieceIndex.black_all, PieceIndex.white_all, moves, isPinned);
                    }
                    //determine queen moves
                    else if (state[(int)PieceIndex.black_queen].GetBit(i))
                    {
                        CalculateQueenMoves(i, PieceTypes.black_queen, PieceIndex.black_all, PieceIndex.white_all, moves, isPinned);
                    }
                    //determine king moves
                    else if (state[(int)PieceIndex.black_king].GetBit(i))
                    {
                        CalculateBlackKingMoves(i, moves);
                    }
                }
            }

            return moves;
        }

        private bool IsPinned(PieceTypes side, int kingSquare, int curr)
        {
            int kx = kingSquare % 8;
            int ky = kingSquare / 8;

            int cx = curr % 8;
            int cy = curr / 8;

            int dx = kx - cx;
            int dy = ky - cy;

            int kxs = dx * dx;
            int kys = dy * dy;

            dx = dx == 0 ? 0 : dx > 0 ? 1 : -1;
            dy = dy == 0 ? 0 : dy > 0 ? 1 : -1;

            PieceIndex king = (side & PieceTypes.White) == PieceTypes.White ? PieceIndex.white_king : PieceIndex.black_king;
            PieceIndex bishop = (side & PieceTypes.White) == PieceTypes.White ? PieceIndex.black_bishop : PieceIndex.white_bishop;
            PieceIndex rook = (side & PieceTypes.White) == PieceTypes.White ? PieceIndex.black_rook : PieceIndex.white_rook;
            PieceIndex queen = (side & PieceTypes.White) == PieceTypes.White ? PieceIndex.black_queen : PieceIndex.white_queen;

            if (kxs == kys)
            {
                if (LineSearchTarget(cx, cy, dx, dy, state[(int)king]) == false)
                {
                    return false;
                }
                //inline diagonally
                return LineSearchTarget(cx, cy, dx * -1, dy * -1, state[(int)bishop] | state[(int)queen]);
            }
            else if (dx == 0 || dy == 0)
            {
                if (LineSearchTarget(cx, cy, dx, dy, state[(int)king]) == false)
                {
                    return false;
                }
                //inline horivertically
                return LineSearchTarget(cx, cy, dx * -1, dy * -1, state[(int)rook] | state[(int)queen]);
            }

            return false;
        }

        private bool LineSearchTarget(int prevX, int prevY, int mapX, int mapY, BitSet enemyTarg)
        {
            int destX = prevX + mapX;
            int destY = prevY + mapY;
            if (destX >= 0 && destX <= 7 && destY >= 0 && destY <= 7)
            {
                int target = destX + destY * 8;
                if (enemyTarg.GetBit(target) == true)
                {
                    return true;
                }

                if (state[(int)PieceIndex.all].GetBit(target) == false)
                    return LineSearchTarget(destX, destY, mapX, mapY, enemyTarg);
            }

            return false;
        }

        private int[] _kingSquares;
        private int[] KingSquares
        {
            get
            {
                if (_kingSquares != null)
                    return _kingSquares;

                _kingSquares = new int[2];

                for (int i = 0; i < 64; ++i)
                {
                    if (state[(int)PieceIndex.white_king].GetBit(i))
                    {
                        _kingSquares[0] = i;
                    }
                    else if (state[(int)PieceIndex.black_king].GetBit(i))
                    {
                        _kingSquares[1] = i;
                    }
                }

                return _kingSquares;
            }
        }

        public bool IsKingInCheck(PieceTypes side)
        {
            var kingSq = side == PieceTypes.White ? KingSquares[0] : KingSquares[1];
            var eKingSq = side == PieceTypes.White ? KingSquares[1] : KingSquares[0];

            PieceIndex bishop = side == PieceTypes.White ? PieceIndex.black_bishop : PieceIndex.white_bishop;
            PieceIndex rook = side == PieceTypes.White ? PieceIndex.black_rook : PieceIndex.white_rook;
            PieceIndex queen = side == PieceTypes.White ? PieceIndex.black_queen : PieceIndex.white_queen;
            PieceIndex knight = side == PieceTypes.White ? PieceIndex.black_knight : PieceIndex.white_knight;
            PieceIndex pawn = side == PieceTypes.White ? PieceIndex.black_pawn : PieceIndex.white_pawn;

            var rookSet = state[(int)rook] | state[(int)queen];
            var bishopSet = state[(int)bishop] | state[(int)queen];

            int[] xMap = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] yMap = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

            int cx = kingSq % 8;
            int cy = kingSq / 8;
            int ex = eKingSq % 8;
            int ey = eKingSq / 8;

            //check for bishops, rooks & queens
            for (int i = 0; i < 8; ++i)
            {
                if (LineSearchTarget(cx, cy, xMap[i], yMap[i], (i % 2) == 0 ? rookSet : bishopSet))
                {
                    return true;
                }
            }

            //check for knights
            int[] xnMap = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };
            int[] ynMap = new int[] { 2, 1, -1, -2, -2, -1, 1, 2 };
            for (int i = 0; i < 8; ++i)
            {
                int nx = cx + xnMap[i];
                int ny = cy + ynMap[i];

                if (nx < 0 || nx > 7 || ny < 0 || ny > 7)
                    continue;

                if (state[(int)knight].GetBit(nx + ny * 8))
                {
                    return true;
                }
            }

            //check for pawns
            if (side == PieceTypes.White)
            {
                //check for black pawns
                int ap = (cx - 1) + (cy + 1) * 8; // \
                int bp = (cx + 1) + (cy + 1) * 8; // /
                if ((cx % 8) > 0 && state[(int)pawn].GetBit(ap))
                    return true;
                if (((cx + 1) % 8) > 0 && state[(int)pawn].GetBit(bp))
                    return true;
            }
            else
            {
                //check for white pawns
                int ap = (cx - 1) + (cy - 1) * 8; // \
                int bp = (cx + 1) + (cy - 1) * 8; // /
                if ((cx % 8) > 0 && state[(int)pawn].GetBit(ap))
                    return true;
                if (((cx + 1) % 8) > 0 && state[(int)pawn].GetBit(bp))
                    return true;
            }

            //check for king
            if (MathF.Abs(ex - cx) <= 1 && MathF.Abs(ey - cy) <= 1)
            {
                //kings are possibly checking each other, implement safety checks
                return true;
            }

            return false;
        }

        private static ulong _whitekingsideMask  = 0b0000000000000000000000000000000000000000000000000000000001100000;
        private static ulong _whitequeensideMask = 0b0000000000000000000000000000000000000000000000000000000000001110;
        private static ulong _blackkingsideMask  = 0b0110000000000000000000000000000000000000000000000000000000000000;
        private static ulong _blackqueensideMask = 0b0000111000000000000000000000000000000000000000000000000000000000;

        public void AddMove(HalfMove proposedMove, List<HalfMove> moves, PieceTypes side, bool checkValidation)
        {
            if (checkValidation)
            {
                if (proposedMove.resultState.IsKingInCheck(side))
                    return;
            }

            moves.Add(proposedMove);
        }

        private void CalculateWhiteKingMoves(int origin, List<HalfMove> moves)
        {
            CalculateKingMoves(origin, PieceTypes.white_king, PieceIndex.white_all, PieceIndex.black_all, moves);

            if (!IsKingInCheck(Turn))
            {
                bool castleKing = moves.Where(b => b.From == origin && b.To == origin + 1 && (b.Details & MoveDetails.capture) == 0).Count() == 1;
                bool castleQueen = moves.Where(b => b.From == origin && b.To == origin - 1 && (b.Details & MoveDetails.capture) == 0).Count() == 1;
                castleKing = castleKing && LineSearchTarget(origin % 8, origin / 8, 1, 0, state[(int)PieceIndex.white_rook]);
                castleQueen = castleQueen && LineSearchTarget(origin % 8, origin / 8, -1, 0, state[(int)PieceIndex.white_rook]);
                //handle castling
                if (castleKing && (castleRights & CastlingRights.White_KingSide) != 0 && (state[(int)PieceIndex.white_all] & _whitekingsideMask) == 0ul)
                {
                    //can castle kingside
                    AddMove(new HalfMove(MoveDetails.king_castle, origin, origin + 2, this), moves, Turn, true);
                }
                if (castleQueen && (castleRights & CastlingRights.White_QueenSide) != 0 && (state[(int)PieceIndex.white_all] & _whitequeensideMask) == 0ul)
                {
                    //can castle queenside
                    AddMove(new HalfMove(MoveDetails.queen_castle, origin, origin - 2, this), moves, Turn, true);
                }
            }
        }
        private void CalculateBlackKingMoves(int origin, List<HalfMove> moves)
        {
            CalculateKingMoves(origin, PieceTypes.black_king, PieceIndex.black_all, PieceIndex.white_all, moves);

            if (!IsKingInCheck(Turn))
            {
                bool castleKing = moves.Where(b => b.From == origin && b.To == origin + 1 && (b.Details & MoveDetails.capture) == 0).Count() == 1;
                bool castleQueen = moves.Where(b => b.From == origin && b.To == origin - 1 && (b.Details & MoveDetails.capture) == 0).Count() == 1;
                castleKing = castleKing && LineSearchTarget(origin % 8, origin / 8, 1, 0, state[(int)PieceIndex.black_rook]);
                castleQueen = castleQueen && LineSearchTarget(origin % 8, origin / 8, -1, 0, state[(int)PieceIndex.black_rook]);
                //handle castling
                if (castleKing && (castleRights & CastlingRights.Black_KingSide) != 0 && (state[(int)PieceIndex.black_all] & _blackkingsideMask) == 0ul)
                {
                    //can castle kingside
                    AddMove(new HalfMove(MoveDetails.king_castle, origin, origin + 2, this), moves, Turn, true);
                }
                if (castleQueen && (castleRights & CastlingRights.Black_QueenSide) != 0 && (state[(int)PieceIndex.black_all] & _blackqueensideMask) == 0ul)
                {
                    //can castle queenside
                    AddMove(new HalfMove(MoveDetails.queen_castle, origin, origin - 2, this), moves, Turn, true);
                }
            }
        }

        private void CalculateKingMoves(int origin, PieceTypes me, PieceIndex allyAll, PieceIndex enemyAll, List<HalfMove> moves)
        {
            int x = origin % 8;
            int y = origin / 8;

            int[] xMap = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] yMap = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

            for (int i = 0; i < 8; ++i)
            {
                LineSearch(origin, 1, x, y, xMap[i], yMap[i], me, allyAll, enemyAll, moves, true);
            }
        }

        private void CalculateQueenMoves(int origin, PieceTypes me, PieceIndex allyAll, PieceIndex enemyAll, List<HalfMove> moves, bool isPinned)
        {
            int x = origin % 8;
            int y = origin / 8;

            int[] xMap = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] yMap = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

            for (int i = 0; i < 8; ++i)
            {
                LineSearch(origin, 8, x, y, xMap[i], yMap[i], me, allyAll, enemyAll, moves, isPinned);
            }
        }

        private void CalculateRookMoves(int origin, PieceTypes me, PieceIndex allyAll, PieceIndex enemyAll, List<HalfMove> moves, bool isPinned)
        {
            int x = origin % 8;
            int y = origin / 8;

            int[] xMap = new int[] { 0, 1, 0, -1 };
            int[] yMap = new int[] { 1, 0, -1, 0 };

            for (int i = 0; i < 4; ++i)
            {
                LineSearch(origin, 8, x, y, xMap[i], yMap[i], me, allyAll, enemyAll, moves, isPinned);
            }
        }

        private void LineSearch(int origin, int depth, int prevX, int prevY, int mapX, int mapY, PieceTypes type, PieceIndex teamAll, PieceIndex enemyAll, List<HalfMove> moves, bool isPinned)
        {
            if (depth <= 0)
                return;

            int destX = prevX + mapX;
            int destY = prevY + mapY;
            if (destX >= 0 && destX <= 7 && destY >= 0 && destY <= 7)
            {
                int target = destX + destY * 8;
                if (state[(int)teamAll].GetBit(target) == false)
                {
                    if (state[(int)enemyAll].GetBit(target))
                    {
                        AddMove(new HalfMove(MoveDetails.captures, origin, target, this), moves, Turn, isPinned);
                    }
                    else
                    {
                        AddMove(new HalfMove(MoveDetails.quiet_move, origin, target, this), moves, Turn, isPinned);
                    }
                }

                if (state[(int)PieceIndex.all].GetBit(target) == false)
                    LineSearch(origin, depth - 1, destX, destY, mapX, mapY, type, teamAll, enemyAll, moves, isPinned);
            }
        }

        private void CalculateBishopMoves(int origin, PieceTypes me, PieceIndex allyAll, PieceIndex enemyAll, List<HalfMove> moves, bool isPinned)
        {
            int x = origin % 8;
            int y = origin / 8;

            int[] xMap = new int[] { 1, 1, -1, -1 };
            int[] yMap = new int[] { 1, -1, -1, 1 };

            for (int i = 0; i < 4; ++i)
            {
                LineSearch(origin, 8, x, y, xMap[i], yMap[i], me, allyAll, enemyAll, moves, isPinned);
            }
        }

        private void CalculateKnightMoves(int origin, PieceTypes me, PieceIndex allyAll, PieceIndex enemyAll, List<HalfMove> moves, bool isPinned)
        {
            int x = origin % 8;
            int y = origin / 8;

            int[] xMap = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };
            int[] yMap = new int[] { 2, 1, -1, -2, -2, -1, 1, 2 };

            for (int i = 0; i < 8; ++i)
            {
                int x2 = x + xMap[i];
                int y2 = y + yMap[i];

                if (x2 >= 0 && x2 <= 7 && y2 >= 0 && y2 <= 7)
                {
                    int target = x2 + y2 * 8;
                    if (state[(int)allyAll].GetBit(target) == false)
                    {
                        if (state[(int)enemyAll].GetBit(target) == true)
                        {
                            AddMove(new HalfMove(MoveDetails.captures, origin, target, this), moves, Turn, isPinned);
                        }
                        else
                        {
                            AddMove(new HalfMove(MoveDetails.quiet_move, origin, target, this), moves, Turn, isPinned);
                        }
                    }
                }
            }
        }

        private void CalculateWhitePawnMoves(int i, List<HalfMove> moves, bool isPinned)
        {
            //pawns move up the board 1 square
            if (i < 56 && state[(int)PieceIndex.all].GetBit(i + 8) == false)
            {
                if (i < 48 && (i > 7 && i < 16) && state[(int)PieceIndex.all].GetBit(i + 16) == false)
                {
                    //can move forward 1 square
                    AddMove(new HalfMove(MoveDetails.quiet_move, i, i + 8, this), moves, Turn, isPinned);
                    //can move forward 2 squares
                    AddMove(new HalfMove(MoveDetails.double_pawn_push, i, i + 16, this), moves, Turn, isPinned);
                }
                else if ((i + 8) > 55 && (i + 8) < 64)
                {
                    //promotions
                    //TODO: Add promotions logic
                    AddMove(new HalfMove(MoveDetails.queen_promotion, i, i + 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promotion, i, i + 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promotion, i, i + 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promotion, i, i + 8, this), moves, Turn, isPinned);
                }
                else
                {
                    //can move forward 1 square
                    AddMove(new HalfMove(MoveDetails.quiet_move, i, i + 8, this), moves, Turn, isPinned);
                }
            }
            //pawns take diagonally
            if ((i % 8) != 0 && i < 56 && state[(int)PieceIndex.black_all].GetBit(i + 7) == true)
            {
                //can take diagonally \
                if ((i + 7) > 55 && (i + 7) < 64)
                {
                    AddMove(new HalfMove(MoveDetails.queen_promo_capture, i, i + 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promo_capture, i, i + 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promo_capture, i, i + 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promo_capture, i, i + 7, this), moves, Turn, isPinned);
                }
                else
                {
                    AddMove(new HalfMove(MoveDetails.captures, i, i + 7, this), moves, Turn, isPinned);
                }
            }
            if (((i + 1) % 8) != 0 && i < 56 && state[(int)PieceIndex.black_all].GetBit(i + 9) == true)
            {
                //can take diagonally /
                if ((i + 9) > 55 && (i + 9) < 64)
                {
                    AddMove(new HalfMove(MoveDetails.queen_promo_capture, i, i + 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promo_capture, i, i + 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promo_capture, i, i + 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promo_capture, i, i + 9, this), moves, Turn, isPinned);
                }
                else
                {
                    AddMove(new HalfMove(MoveDetails.captures, i, i + 9, this), moves, Turn, isPinned);
                }
            }
            //en passant
            if (i < 40 && i > 31)
            {
                if (i > 32 && i+7 == epTarg)
                {
                    //can enpassant diagonally \
                    AddMove(new HalfMove(MoveDetails.ep_capture, i, i + 7, this), moves, Turn, true);
                }
                if (i < 39 && i + 9 == epTarg)
                {
                    //can enpassant diagonally /
                    AddMove(new HalfMove(MoveDetails.ep_capture, i, i + 9, this), moves, Turn, true);
                }
            }
        }

        private void CalculateBlackPawnMoves(int i, List<HalfMove> moves, bool isPinned)
        {
            //pawns move up the board 1 square
            if (i < 56 && state[(int)PieceIndex.all].GetBit(i - 8) == false)
            {
                if ((i > 47 && i < 56) && state[(int)PieceIndex.all].GetBit(i - 16) == false)
                {
                    //can move forward 1 square
                    AddMove(new HalfMove(MoveDetails.quiet_move, i, i - 8, this), moves, Turn, isPinned);
                    //can move forward 2 squares
                    AddMove(new HalfMove(MoveDetails.double_pawn_push, i, i - 16, this), moves, Turn, isPinned);
                }
                else if ((i - 8) >= 0 && (i - 8) < 8)
                {
                    //promotions
                    AddMove(new HalfMove(MoveDetails.queen_promotion, i, i - 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promotion, i, i - 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promotion, i, i - 8, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promotion, i, i - 8, this), moves, Turn, isPinned);
                }
                else
                {
                    //can move forward 1 square
                    AddMove(new HalfMove(MoveDetails.quiet_move, i, i - 8, this), moves, Turn, isPinned);
                }
            }
            //pawns take diagonally
            if (((i + 1) % 8) != 0 && i > 7 && state[(int)PieceIndex.white_all].GetBit(i - 7) == true)
            {
                //can take diagonally \
                if ((i - 7) >= 0 && (i - 7) < 8)
                {
                    AddMove(new HalfMove(MoveDetails.queen_promo_capture, i, i - 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promo_capture, i, i - 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promo_capture, i, i - 7, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promo_capture, i, i - 7, this), moves, Turn, isPinned);
                }
                else
                {
                    AddMove(new HalfMove(MoveDetails.captures, i, i - 7, this), moves, Turn, isPinned);
                }
            }
            if ((i % 8) != 0 && i > 7 && state[(int)PieceIndex.white_all].GetBit(i - 9) == true)
            {
                //can take diagonally /
                if ((i - 9) >= 0 && (i - 9) < 8)
                {
                    AddMove(new HalfMove(MoveDetails.queen_promo_capture, i, i - 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.rook_promo_capture, i, i - 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.bishop_promo_capture, i, i - 9, this), moves, Turn, isPinned);
                    AddMove(new HalfMove(MoveDetails.knight_promo_capture, i, i - 9, this), moves, Turn, isPinned);
                }
                else
                {
                    AddMove(new HalfMove(MoveDetails.captures, i, i - 9, this), moves, Turn, isPinned);
                }
            }
            //en passant
            if (i < 32 && i > 23)
            {
                if (i < 31 && i - 7 == epTarg)
                {
                    //can enpassant diagonally \
                    AddMove(new HalfMove(MoveDetails.ep_capture, i, i - 7, this), moves, Turn, true);
                }
                if (i > 24 && i - 9 == epTarg)
                {
                    //can enpassant diagonally /
                    AddMove(new HalfMove(MoveDetails.ep_capture, i, i - 9, this), moves, Turn, true);
                }
            }
        }
    }
}
