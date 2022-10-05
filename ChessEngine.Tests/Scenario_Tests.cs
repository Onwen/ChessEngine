using NUnit.Framework;
using System;

namespace ChessEngine.Tests
{
    public class Scenario_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("r3k2r/pp1pp1pp/8/2pPPp2/2P2P2/1P4P1/P6P/R3K2R w k f6 0 1 5", 1, 19, 1, 1, 0, 0, 0, 0)]
        [TestCase("r3k1r1/pp1pp2p/8/2pPPp2/2P2P2/1P6/P6P/R3K2R w KQ f6 0 1", 1, 19, 1, 1, 1, 0, 0, 0)]
        [TestCase("r3k2r/pb3p2/5npp/3p4/1p1PPB2/1n4P1/P2N1PBP/R3K2R w KQkq - 1 2", 1, 33, 4, 0, 1, 0, 0, 0)]
        [TestCase("1r2k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R w KQk - 1 2", 1, 33, 3, 0, 2, 0, 0, 0)]
        [TestCase("2kr3r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R w KQ - 1 2", 1, 33, 2, 0, 2, 0, 0, 0)]
        [TestCase("8/8/8/2k2pP1/p3K3/8/1P6/8 w - f6", 1, 7, 2, 1, 0, 0, 0, 0)]
        [TestCase("r2Bk2r/p6p/8/8/1pp1p3/3b4/P6P/R3K2R b KQkq -", 1, 21, 2, 0, 1, 0, 0, 0)]
        [TestCase("8/2p5/3p4/KP5r/1R3p1k/4P3/6P1/8 b - -", 1, 15, 1, 0, 0, 0, 0, 0)]
        [TestCase("8/2p5/3p4/KP5r/1R2Pp1k/8/6P1/8 b - e3", 1, 16, 1, 0, 0, 0, 0, 0)]
        [TestCase("8/2p5/3p4/KP5r/2R2p1k/8/4P1P1/8 b - -", 1, 15, 1, 0, 0, 0, 0, 0)]
        [TestCase("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2BNP1b1/P1NP4/1PP1QPPP/R4RK1 b - -", 1, 46, 6, 0, 0, 0, 0, 0)]
        [TestCase("6k1/R7/6N1/6B1/3NP1b1/P1NP4/1PP1QPPP/R4RK1 b - - 0 1", 1, 8, 1, 0, 0, 0, 0, 0)]
        [TestCase("r3k2r/pb3p2/5npp/n2p4/3PPB2/1p4P1/P2N1PBP/R3K2R w KQkq -", 2, 970, 104, 0, 62, 0, 0, 0)]
        [TestCase("rB2k2r/pb3p2/5npp/n2p4/3PP3/1p4P1/P2N1PBP/R3K2R b KQkq -", 1, 26, 4, 0, 1, 0, 0, 0)]
        public void Scenario_Position1_Tests(string FEN, int depth, long expMove, long expCapt, long expEP, long expCastle, long expPromo, long expCheck, long expCheckMate)
        {
            BoardState state;
            state = new BoardState(FEN);

            Perft result = state.Perft(depth);

            Assert.That(result.moveCount, Is.EqualTo(expMove));
            Assert.That(result.captures, Is.EqualTo(expCapt));
            Assert.That(result.enpassant, Is.EqualTo(expEP));
            Assert.That(result.castles, Is.EqualTo(expCastle));
            Assert.That(result.promotions, Is.EqualTo(expPromo));
            //Assert.That(result.checks, Is.EqualTo(expCheck));
        }
    }
}