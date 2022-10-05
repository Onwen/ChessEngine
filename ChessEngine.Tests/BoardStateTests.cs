using NUnit.Framework;
using System;

namespace ChessEngine.Tests
{
    public class Piece_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("7k/8/8/8/8/8/P7/K7 w - - 0 0", 4)]
        [TestCase("7k/8/8/8/8/P7/8/K7 w - - 0 0", 4)]
        [TestCase("7k/8/8/8/P7/8/8/K7 w - - 0 0", 4)]
        [TestCase("7k/8/8/P7/8/8/8/K7 w - - 0 0", 4)]
        [TestCase("7k/8/P7/8/8/8/8/K7 w - - 0 0", 4)]
        [TestCase("7k/P7/8/8/8/8/8/K7 w - - 0 0", 7)]
        [TestCase("p6k/P7/8/8/8/8/8/K7 w - - 0 0", 3)]
        [TestCase("1p5k/P7/8/8/8/8/8/K7 w - - 0 0", 11)]
        [TestCase("p6k/1P6/8/8/8/8/8/K7 w - - 0 0", 11)]
        [TestCase("p1p4k/1P6/8/8/8/8/8/K7 w - - 0 0", 15)]
        [TestCase("ppp4k/1P6/8/8/8/8/8/K7 w - - 0 0", 11)]
        [TestCase("7k/8/8/8/8/8/PPPPPPPP/K7 w - - 0 0", 17)]
        public void WhitePawnMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("k7/p7/8/8/8/8/8/7K b - - 0 0", 4)]
        [TestCase("k7/8/p7/8/8/8/8/7K b - - 0 0", 4)]
        [TestCase("k7/8/8/p7/8/8/8/7K b - - 0 0", 4)]
        [TestCase("k7/8/8/8/p7/8/8/7K b - - 0 0", 4)]
        [TestCase("k7/8/8/8/8/p7/8/7K b - - 0 0", 4)]
        [TestCase("k7/8/8/8/8/8/p7/7K b - - 0 0", 7)]
        [TestCase("k7/8/8/8/8/8/p7/P6K b - - 0 0", 3)]
        [TestCase("k7/8/8/8/8/8/p7/1P5K b - - 0 0", 11)]
        [TestCase("k7/8/8/8/8/8/1p6/P6K b - - 0 0", 11)]
        [TestCase("k7/8/8/8/8/8/1p6/P1P4K b - - 0 0", 15)]
        [TestCase("k7/8/8/8/8/8/1p6/PPP4K b - - 0 0", 11)]
        [TestCase("k7/pppppppp/8/8/8/8/8/7K b - - 0 0", 17)]
        public void BlackPawnMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("k7/8/8/8/8/8/8/N6K w - - 0 0", 5)]
        [TestCase("k7/8/8/8/8/8/8/K6N w - - 0 0", 5)]
        [TestCase("N6K/8/8/8/8/8/8/k7 w - - 0 0", 5)]
        [TestCase("K6N/8/8/8/8/8/8/k7 w - - 0 0", 5)]
        [TestCase("k7/8/8/8/8/8/8/3N3K w - - 0 0", 7)]
        [TestCase("K2N4/8/8/8/8/8/8/k7 w - - 0 0", 7)]
        [TestCase("k7/8/8/N7/8/8/8/K7 w - - 0 0", 7)]
        [TestCase("k7/8/8/7N/8/8/8/K7 w - - 0 0", 7)]
        [TestCase("k7/8/8/3N4/8/8/8/K7 w - - 0 0", 11)]
        [TestCase("1n2k1n1/8/8/8/8/8/8/1N2K1N1 w - - 0 0", 11)]
        [TestCase("1n2k1n1/8/8/8/8/8/8/1N2K1N1 b - - 0 0", 11)]
        public void KnightMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("k7/8/8/8/8/8/8/B6K w - - 0 0", 10)]
        [TestCase("7k/8/8/8/8/8/8/K6B w - - 0 0", 10)]
        [TestCase("B6K/8/8/8/8/8/8/k7 w - - 0 0", 10)]
        [TestCase("K6B/8/8/8/8/8/8/7k w - - 0 0", 10)]
        [TestCase("k7/8/8/8/8/8/8/3BK3 w - - 0 0", 11)]
        [TestCase("3Bk3/8/8/8/8/8/8/7K w - - 0 0", 10)]
        [TestCase("8/8/8/Bk6/8/8/8/K7 w - - 0 0", 10)]
        [TestCase("8/8/8/6kB/8/8/8/7K w - - 0 0", 10)]
        [TestCase("8/8/8/2KBk3/8/8/8/8 w - - 0 0", 18)]
        [TestCase("8/8/8/8/2KBk3/8/8/8 w - - 0 0", 18)]
        public void BishopMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("7k/8/8/8/8/8/7K/R7 w - - 0 0", 19)]
        [TestCase("k7/8/8/8/8/8/K7/7R w - - 0 0", 19)]
        [TestCase("R7/7K/8/8/8/8/8/7k w - - 0 0", 19)]
        [TestCase("7R/K7/8/8/8/8/8/k7 w - - 0 0", 19)]
        [TestCase("k6K/8/8/8/8/8/8/3R4 w - - 0 0", 17)]
        [TestCase("3R4/8/8/8/8/8/8/k6K w - - 0 0", 17)]
        [TestCase("7k/8/8/R7/8/8/8/7K w - - 0 0", 17)]
        [TestCase("k7/8/8/7R/8/8/8/K7 w - - 0 0", 17)]
        [TestCase("k7/8/8/3R4/8/8/8/K7 w - - 0 0", 17)]
        [TestCase("k7/8/8/8/3R4/8/8/K7 w - - 0 0", 17)]
        public void RookMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1k6/8/8/8/8/8/7K/Q7 w - - 0 0", 26)]
        [TestCase("6k1/8/8/8/8/8/K7/7Q w - - 0 0", 26)]
        [TestCase("Q7/7k/8/8/8/8/8/1K6 w - - 0 0", 26)]
        [TestCase("7Q/k7/8/8/8/8/8/6K1 w - - 0 0", 26)]
        [TestCase("k7/8/8/8/8/8/1K6/3Q4 w - - 0 0", 29)]
        [TestCase("3Q4/k7/8/8/8/8/8/1K6 w - - 0 0", 26)]
        [TestCase("1k6/8/8/Q7/8/8/8/1K6 w - - 0 0", 26)]
        [TestCase("6k1/8/8/7Q/8/8/8/K7 w - - 0 0", 24)]
        [TestCase("2k5/8/8/3Q4/8/8/8/K7 w - - 0 0", 30)]
        [TestCase("2k5/8/8/8/3Q4/8/8/1K6 w - - 0 0", 32)]
        public void QueenMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("3K4/8/7Q/8/8/8/2rrr3/3k4 w - - 0 0", 2)]
        [TestCase("3K4/r7/8/8/8/8/3rr3/3k4 w - - 0 0", 1)]
        [TestCase("3K4/7r/8/8/8/8/2rr4/3k4 w - - 0 0", 1)]
        [TestCase("3K4/4r3/8/8/8/8/2rr4/3k4 w - - 0 0", 1)]
        public void KingCheckTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("k7/8/8/8/8/8/8/K7 w - - 0 0", 3)]
        [TestCase("7k/8/8/8/8/8/8/7K w - - 0 0", 3)]
        [TestCase("K7/8/8/8/8/8/8/k7 w - - 0 0", 3)]
        [TestCase("7K/8/8/8/8/8/8/7k w - - 0 0", 3)]
        [TestCase("3k4/8/8/8/8/8/8/3K4 w - - 0 0", 5)]
        [TestCase("3K4/8/8/8/8/8/8/3k4 w - - 0 0", 5)]
        [TestCase("k7/8/8/K7/8/8/8/8 w - - 0 0", 5)]
        [TestCase("7k/8/8/7K/8/8/8/8 w - - 0 0", 5)]
        [TestCase("3k4/8/8/3K4/8/8/8/8 w - - 0 0", 8)]
        [TestCase("3k4/8/8/8/3K4/8/8/8 w - - 0 0", 8)]
        [TestCase("4k3/8/8/8/8/8/8/R3K2R w KQkq - 0 0", 26)]
        public void KingMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1n4n1/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 0", 20)]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/8/8 b KQkq - 0 0", 20)]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0", 20)]
        public void GeneralMovementTests(string fen, int expected)
        {
            BoardState state;
            state = new BoardState(fen);

            Perft result = state.Perft(1);

            Assert.That(result.moveCount, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -")]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq -")]
        public void FEN_Tests(string fen)
        {
            BoardState state;
            state = new BoardState(fen);
            string newFen = state.BoardFEN;

            Assert.That(newFen, Is.EqualTo(fen));
        }
    }
}