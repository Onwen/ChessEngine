using NUnit.Framework;
using System;

namespace ChessEngine.Tests
{
    public class FEN_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("r3k2r/pp1pp1pp/8/2pPPp2/2P2P2/1P4P1/P6P/R3K2R w k f6")]
        public void FEN_Position1_Tests(string FEN)
        {
            BoardState state;
            state = new BoardState(FEN);

            Assert.That(state.BoardFEN, Is.EqualTo(FEN));
        }


        [Test]
        [TestCase("rnb1kb1r/pppp1ppp/5q1n/4p3/2B1P3/5NP1/PPPP1P1P/RNBQK2R w KQkq - 3 5", "rnb1kb1r/pppp1ppp/5q1n/4N3/2B1P3/6P1/PPPP1P1P/RNBQK2R b KQkq -")]
        public void FEN_MoveTests_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //knight f3=>e5 takes pawn
            var m = new HalfMove(MoveDetails.captures, 21, 36, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }
        [Test]

        [TestCase("r3k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R b KQkq -", "1r2k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R w KQk -")]
        public void FEN_MoveTest_Pos10_a8_b8_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //rook a8=>b8 takes pawn
            var m = new HalfMove(MoveDetails.quiet_move, 56, 57, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }

        [TestCase("r3k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R b KQkq -", "2r1k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R w KQk -")]
        public void FEN_MoveTest_Pos10_a8_c8_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //rook a8=>b8 takes pawn
            var m = new HalfMove(MoveDetails.quiet_move, 56, 58, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }

        [TestCase("r3k2r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R b KQkq -", "2kr3r/pb3p2/5npp/n2p4/1p1PPB2/6P1/P2N1PBP/R3K2R w KQ -")]
        public void FEN_MoveTest_Pos10_e8_c8_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //king e8=>c8 black queenside castle
            var m = new HalfMove(MoveDetails.queen_castle, 60, 58, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }

        [TestCase("8/5p2/8/2k3P1/p3K3/8/1P6/8 b - -", "8/8/8/2k2pP1/p3K3/8/1P6/8 w - f6")]
        public void FEN_MoveTest_Pos9_f7_f5_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //pawn f7=>f5 double pawn push
            var m = new HalfMove(MoveDetails.double_pawn_push, 53, 37, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }

        [TestCase("8/8/8/2k2pP1/p3K3/8/1P6/8 w - f6", "8/8/5P2/2k5/p3K3/8/1P6/8 b - -")]
        public void FEN_MoveTest_Pos9_g5_f6_Tests(string startFEN, string endFEN)
        {
            BoardState state;
            state = new BoardState(startFEN);

            //pawn g5=>f6 en passant
            var m = new HalfMove(MoveDetails.ep_capture, 38, 45, state);

            Assert.That(m.resultState.BoardFEN, Is.EqualTo(endFEN));
        }
    }
}