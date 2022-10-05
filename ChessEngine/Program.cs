using System;
using System.Diagnostics;

namespace ChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardState state;
            state = new BoardState("rB2k2r/pb3p2/5npp/n2p4/3PP3/1p4P1/P2N1PBP/R3K2R b KQkq -");
            //its castling incorrectly

            foreach (var m in state.FindValidMoves())
            {
                var perft = m.resultState.Perft(1);

                Console.WriteLine($"{boardString(m.From)} => {boardString(m.To)} = {perft.moveCount} moves --- {m.resultState.BoardFEN}");
            }

            Console.ReadLine();
        }

        static string boardString(int i)
        {
            int x = i % 8;
            int y = i / 8;

            return $"{(char)('a' + x)}{y + 1}";
        }
    }
}
