using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Logging;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Bags;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.AppliedStack.Replays
{
    public class AppliedStackAutoGenerator : AutoGenerator<AppliedStackReplayFrame>
    {
        // TODO: need improvement

        public const int FRAME_DURATION = 10;
        public const int KEYPRESS_DURATION = 8;

        public new AppliedStackBeatmap Beatmap => (AppliedStackBeatmap)base.Beatmap;
        private Board board;
        private Random rng;
        private BufferedBag bag;
        private PieceObject current;
        private PieceObject hold;

        // """Machine Learning"""
        public static bool IsLearning = false;
        public static int Generation = 0;
        public static float LandingHeightBias = 0.65f;
        public static float EmptyTileSideBias = 0.30f;
        public static float EmptyTileTopBias = 0.05f;
        public static float EmptyTileBottomBias = 1.60f;
        public static float RowsEliminatedBias = 1.00f;
        public static int PreviousScore = 0;
        public static float[] RandomizedChanges = new float[5];
        public static void RandomizeParametersChanges()
        {
            Random rng = new Random();
            for (int i = 0; i < 5; i++) RandomizedChanges[i] = (float)(rng.NextDouble() * 2.0 - 1.0) * 0.2f;
        }
        public static void ApplyParametersChanges()
        {
            LandingHeightBias += RandomizedChanges[0];
            EmptyTileSideBias += RandomizedChanges[1];
            EmptyTileTopBias += RandomizedChanges[2];
            EmptyTileBottomBias += RandomizedChanges[3];
            RowsEliminatedBias += RandomizedChanges[4];
        }
        public static void RevertParametersChanges()
        {
            LandingHeightBias -= RandomizedChanges[0];
            EmptyTileSideBias -= RandomizedChanges[1];
            EmptyTileTopBias -= RandomizedChanges[2];
            EmptyTileBottomBias -= RandomizedChanges[3];
            RowsEliminatedBias -= RandomizedChanges[4];
        }

        public int TotalScore { get; private set; } = 0;

        public PieceObject NextPieceOnSwap {
            get {
                PieceObject next = hold != null? hold.Clone() : bag.Next();
                next.ResetRotation();
                next.X = (AppliedStackRuleset.BOARD_WIDTH - next.Size) / 2;
                next.Y = AppliedStackRuleset.BOARD_HEIGHT;
                return next;
            }
        }

        public AppliedStackAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        private AppliedStackReplayFrame HoldSwap()
        {
            PieceObject aux = current;
            current = hold;
            hold = aux;
            hold.ResetRotation();
            if (current == null) current = bag.GetNextPiece(rng)();
            return new AppliedStackReplayFrame(AppliedStackAction.Hold);
        }

        private AppliedStackReplayFrame Move(double time, int dir)
        {
            current.X += dir;
            return new AppliedStackReplayFrame(dir < 0? AppliedStackAction.MoveLeft : AppliedStackAction.MoveRight) { Time = time };
        }

        private bool HasTileAt(PieceObject piece, int boardX, int boardY)
        {
            if (piece.Tiles.Where(v => v.X + piece.X == boardX && v.Y + piece.Y == boardY).Any()) return true;
            if (board.GetTileAt(boardX, boardY) != null) return true;
            return false;
        }

        private float CalculateScore(PieceObject piece)
        {
            piece = piece.Clone();
            int travelledLines = AppliedStackRuleset.BOARD_HEIGHT * 2 - board.SoftDrop(AppliedStackRuleset.BOARD_HEIGHT * 2, piece);
            float pieceHeight = 0f;
            {
                int yMin = piece.Tiles[0].Y;
                int yMax = piece.Tiles[0].Y;
                foreach (TileObject tile in piece.Tiles)
                {
                    if (tile.Y < yMin) yMin = tile.Y;
                    else if (tile.Y > yMax) yMax = tile.Y;
                }
                pieceHeight = yMax - yMin + 1;
            }
            float landingHeight = travelledLines + pieceHeight / 2f;

            float holesBias = 0;
            foreach (TileObject tile in piece.Tiles)
            {
                int boardX = tile.X + piece.X;
                int boardY = tile.Y + piece.Y;
                if (boardX < 0 || boardX >= AppliedStackRuleset.BOARD_WIDTH) continue;
                if (boardY < 0) continue;
                holesBias += HasTileAt(piece, boardX - 1, boardY)? EmptyTileSideBias : 0;
                holesBias += HasTileAt(piece, boardX + 1, boardY)? EmptyTileSideBias : 0;
                holesBias += HasTileAt(piece, boardX, boardY + 1)? EmptyTileTopBias : 0;
                holesBias += HasTileAt(piece, boardX, boardY - 1)? EmptyTileBottomBias : 0;
            }

            int rowsEliminated = board.CountFilledLinesWithNonBoardPiece(piece);

            return
                landingHeight * LandingHeightBias +
                holesBias +
                rowsEliminated * RowsEliminatedBias;
        }

        private AppliedStackBotScore FindBest(PieceObject piece, bool isHoldPiece)
        {
            piece = piece.Clone();
            piece.X = -piece.Size;

            while (board.IsOccupied(piece) && piece.X < AppliedStackRuleset.BOARD_WIDTH) piece.X++;

            int bestX = piece.X;
            float bestScore = CalculateScore(piece);
            piece.X++;

            while (!board.IsOccupied(piece) && piece.X < AppliedStackRuleset.BOARD_WIDTH)
            {
                float score = CalculateScore(piece);
                if (score > bestScore)
                {
                    bestX = piece.X;
                    bestScore = score;
                }
                piece.X++;
            }

            return new AppliedStackBotScore
            {
                X = bestX,
                HoldTrigger = isHoldPiece,
                Rotation = piece.CurrentRotation,
                Score = bestScore
            };
        }

        private AppliedStackBotScore ScanAllBestWithRotations(PieceObject piece, bool isHoldPiece)
        {
            piece = piece.Clone();
            piece.ResetRotation();

            AppliedStackBotScore best = FindBest(piece, isHoldPiece);
            float bestScore = best.Score;

            while (piece.CurrentRotation != 3)
            {
                piece.Rotate(1);
                AppliedStackBotScore scoreInfo = FindBest(piece, isHoldPiece);
                if (scoreInfo.Score > bestScore)
                {
                    bestScore = scoreInfo.Score;
                    best = scoreInfo;
                }
            }

            return best;
        }

        private AppliedStackBotScore ScanAllBestWithAllStates()
        {
            /*AppliedStackBotScore noSwap = ScanAllBestWithRotations(current, false);
            AppliedStackBotScore withSwap = ScanAllBestWithRotations(NextPieceOnSwap, true);
            return noSwap.Score > withSwap.Score? noSwap : withSwap;*/ // too stupid
            return ScanAllBestWithRotations(current, false);
        }

        private void PushNewPiece()
        {
            current = bag.GetNextPiece(rng)();
            current.X = (AppliedStackRuleset.BOARD_WIDTH - current.Size) / 2;
            current.Y = AppliedStackRuleset.BOARD_HEIGHT;
        }

        protected override void GenerateFrames()
        {
            for (int i = 0; i < (IsLearning? 10 : 1); i++)
            {
                board = new Board();

                int bagSeed = 1337;
                Random processorRng = new Random(129481581);
                foreach (AppliedStackHitObject hitObj in Beatmap.HitObjects) bagSeed ^= processorRng.Next(hitObj.ObjectSeed);

                rng = new Random(bagSeed);
                bag = new BufferedBag(new SevenBag());
                bag.InitializeBag(PieceObject.Factories(), rng);

                hold = null;
                current = null;

                Frames.Clear();
                PerformSingleGeneration();
            }
        }

        public void PerformSingleGeneration()
        {
            TotalScore = 0;
            if (IsLearning && Generation > 0)
            {
                RandomizeParametersChanges();
                ApplyParametersChanges();
            }

            PushNewPiece();
            Frames.Add(new AppliedStackReplayFrame());
            double lastTime = 0;

            foreach (AppliedStackHitObject hitObject in Beatmap.HitObjects)
            {
                AppliedStackBotScore score = ScanAllBestWithAllStates();
                List<AppliedStackReplayFrame> moveFrames = new List<AppliedStackReplayFrame>();

                if (score.HoldTrigger)
                {
                    moveFrames.Add(HoldSwap());
                }

                while (current.CurrentRotation != score.Rotation)
                {
                    moveFrames.Add(new AppliedStackReplayFrame(AppliedStackAction.RotateRight));
                    board.Rotate(current, 1);
                }

                while (current.X != score.X)
                {
                    moveFrames.Add(new AppliedStackReplayFrame(current.X > score.X? AppliedStackAction.MoveLeft : AppliedStackAction.MoveRight));
                    current.X += current.X > score.X? -1 : 1;
                }

                for (int i = 0; i < moveFrames.Count; i++)
                {
                    double globalFrameTime = lastTime + i * FRAME_DURATION;
                    moveFrames[i].Time = globalFrameTime;

                    Frames.Add(moveFrames[i]);
                    Frames.Add(new AppliedStackReplayFrame()
                    {
                        Time = globalFrameTime + KEYPRESS_DURATION
                    });
                }

                Frames.Add(new AppliedStackReplayFrame(AppliedStackAction.HardDrop)
                {
                    Time = hitObject.StartTime
                });
                Frames.Add(new AppliedStackReplayFrame()
                {
                    Time = hitObject.StartTime + KEYPRESS_DURATION
                });

                board.HardDrop(current);
                TotalScore += board.RemoveFilledLines();
                PushNewPiece();

                lastTime = hitObject.StartTime + FRAME_DURATION;
            }

            if (IsLearning)
            {
                Logger.Log($"| Generation #{Generation}");
                Logger.Log($"| Score = {TotalScore}/{PreviousScore}");
                Logger.Log($"| Parameters = {LandingHeightBias} | {EmptyTileSideBias} | {EmptyTileTopBias} | {EmptyTileBottomBias} | {RowsEliminatedBias}");
                if (Generation == 0) PreviousScore = TotalScore;
                else
                {
                    if (PreviousScore > TotalScore) RevertParametersChanges();
                    else
                    {
                        PreviousScore = TotalScore;
                        Logger.Log($"| This generation is better than previous, score set to {PreviousScore}");
                    }
                }

                Generation += 1;
            }
        }
    }

    public class AppliedStackBotScore
    {
        public int X { get; set; } = 0;
        public int Rotation { get; set; } = 0;
        public float Score { get; set; } = 0;
        public bool HoldTrigger { get; set; } = false;
    }
}
