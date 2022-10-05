using NUnit.Framework;
using System;

namespace ChessEngine.Tests
{
    public class Perft_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(1, 20, 0, 0, 0, 0, 0, 0)]
        [TestCase(2, 400, 0, 0, 0, 0, 0, 0)]
        [TestCase(3, 8902, 34, 0, 0, 0, 0, 0)]
        [TestCase(4, 197281, 1576, 0, 0, 0, 0, 0)]
        [TestCase(5, 4865609, 82719, 258, 0, 0, 0, 0)]
        [TestCase(6, 119060324, 2812008, 5248, 0, 0, 0, 0)]
        [TestCase(7, 3195901860, 108329926, 319617, 883453, 0, 0, 0)]
        //[TestCase(8, 84998978956, 3523740106, 7187977, 23605205, 0, 0, 0)]
        //[TestCase(9, 2439530234167, 125208536153, 319496827, 1784356000, 17334376, 0, 0)]
        public void Perft_Position1_Tests(int depth, long expMove, long expCapt, long expEP, long expCastle, long expPromo, long expCheck, long expCheckMate)
        {
            BoardState state;
            state = new BoardState("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
            Assert.That(result.captures, Is.EqualTo(expCapt));
            Assert.That(result.enpassant, Is.EqualTo(expEP));
            Assert.That(result.castles, Is.EqualTo(expCastle));
            Assert.That(result.promotions, Is.EqualTo(expPromo));
            //Assert.That(result.checks, Is.EqualTo(expCheck));
        }

        [Test]
        [TestCase(1, 48, 8, 0, 2, 0, 0, 0)]
        [TestCase(2, 2039, 351, 1, 91, 0, 3, 0)]
        [TestCase(3, 97862, 17102, 45, 3162, 0, 993, 1)]
        public void Perft_Position2_Tests(int depth, long expMove, long expCapt, long expEP, long expCastle, long expPromo, long expCheck, long expCheckMate)
        {
            BoardState state;
            state = new BoardState("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
            Assert.That(result.captures, Is.EqualTo(expCapt));
            Assert.That(result.enpassant, Is.EqualTo(expEP));
            Assert.That(result.castles, Is.EqualTo(expCastle));
            Assert.That(result.promotions, Is.EqualTo(expPromo));
            //Assert.That(result.checks, Is.EqualTo(expCheck));
        }

        [Test]
        [TestCase(1, 14, 1, 0, 0, 0, 2, 0)]
        [TestCase(2, 191, 14, 0, 0, 0, 10, 0)]
        [TestCase(3, 2812, 209, 2, 0, 0, 267, 0)]
        public void Perft_Position3_Tests(int depth, long expMove, long expCapt, long expEP, long expCastle, long expPromo, long expCheck, long expCheckMate)
        {
            BoardState state;
            state = new BoardState("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
            Assert.That(result.captures, Is.EqualTo(expCapt));
            Assert.That(result.enpassant, Is.EqualTo(expEP));
            Assert.That(result.castles, Is.EqualTo(expCastle));
            Assert.That(result.promotions, Is.EqualTo(expPromo));
            //Assert.That(result.checks, Is.EqualTo(expCheck));
        }

        [Test]
        [TestCase(1, 6, 0, 0, 0, 0, 0, 0)]
        [TestCase(2, 264, 87, 0, 6, 48, 10, 0)]
        [TestCase(3, 9467, 1021, 4, 0, 120, 38, 22)]
        public void Perft_Position4_Tests(int depth, long expMove, long expCapt, long expEP, long expCastle, long expPromo, long expCheck, long expCheckMate)
        {
            BoardState state;
            state = new BoardState("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
            Assert.That(result.captures, Is.EqualTo(expCapt));
            Assert.That(result.enpassant, Is.EqualTo(expEP));
            Assert.That(result.castles, Is.EqualTo(expCastle));
            Assert.That(result.promotions, Is.EqualTo(expPromo));
            //Assert.That(result.checks, Is.EqualTo(expCheck));
        }

        [Test]
        [TestCase(1, 44)]
        [TestCase(2, 1486)]
        [TestCase(3, 62379)]
        public void Perft_Position5_Tests(int depth, long expMove)
        {
            BoardState state;
            state = new BoardState("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8  ");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }

        [Test]
        [TestCase(1, 46)]
        [TestCase(2, 2079)]
        [TestCase(3, 89890)]
        public void Perft_Position6_Tests(int depth, long expMove)
        {
            BoardState state;
            state = new BoardState("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }

        [Test]
        [TestCase(1, 5)]
        [TestCase(2, 39)]
        [TestCase(3, 237)]
        public void Perft_Position7_Tests(int depth, long expMove)
        {
            // https://sites.google.com/site/numptychess/perft/position-2
            BoardState state;
            state = new BoardState("8/p7/8/1P6/K1k3p1/6P1/7P/8 w - -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }

        [Test]
        [TestCase(1, 17)]
        [TestCase(2, 341)]
        [TestCase(3, 6666)]
        public void Perft_Position8_Tests(int depth, long expMove)
        {
            // https://sites.google.com/site/numptychess/perft/position-3
            BoardState state;
            state = new BoardState("r3k2r/p6p/8/B7/1pp1p3/3b4/P6P/R3K2R w KQkq -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }

        [Test]
        [TestCase(1, 9)]
        [TestCase(2, 85)]
        [TestCase(3, 795)]
        public void Perft_Position9_Tests(int depth, long expMove)
        {
            // https://sites.google.com/site/numptychess/perft/position-4
            BoardState state;
            state = new BoardState("8/5p2/8/2k3P1/p3K3/8/1P6/8 b - -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }

        [Test]
        [TestCase(1, 29)]
        [TestCase(2, 953)]
        [TestCase(3, 27990)]
        public void Perft_Position10_Tests(int depth, long expMove)
        {
            // https://sites.google.com/site/numptychess/perft/position-5
            BoardState state;
            state = new BoardState("r3k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R b KQkq -");

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
        }
    }
}