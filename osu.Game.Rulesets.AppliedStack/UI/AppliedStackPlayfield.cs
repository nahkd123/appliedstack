using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.AppliedStack.Bags;
using osu.Game.Rulesets.AppliedStack.Configuration;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.UI
{
    [Cached]
    public class AppliedStackPlayfield : ScrollingPlayfield, IKeyBindingHandler<AppliedStackAction>
    {
        public Container ScaledContainer { get; private set; }

        public Board Board { get; } = new Board();
        public DrawableBoard DrawableBoard { get; private set; }
        private BoardSidebar[] sideBars;
        public PieceObject Current { get; private set; }
        public PieceObject Hold { get; private set; }
        public bool HoldSwapped { get; set; } = false;
        private DrawablePieceObject drawableCurrent;
        private DrawablePieceObject drawableGhost;
        private DrawablePieceObject[] drawableNext;
        private DrawablePieceObject drawableHold;
        private Random rng;
        private BufferedBag bufferedBag;

        private Vector2 judgementDisplayCenter;

        public double arr = 150;
        public double das = 500;
        public double sdr = 150;
        public AppliedStackAction[] HandlingActions = new[]
        {
            AppliedStackAction.Hold,
            AppliedStackAction.MoveLeft,
            AppliedStackAction.MoveRight,
            AppliedStackAction.RotateLeft,
            AppliedStackAction.RotateRight,
            AppliedStackAction.SoftDrop,
        };
        private Bindable<bool> FancyAnimation;

        public AppliedStackPlayfield()
        {
            sideBars = new BoardSidebar[2];
            drawableNext = new DrawablePieceObject[5];

            ScaledContainer = new Container
            {
                Scale = new osuTK.Vector2(25, 25),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = AppliedStackRuleset.BOARD_WIDTH + 9,
                Height = AppliedStackRuleset.BOARD_HEIGHT + 2,
                Children = new Drawable[]
                {
                    sideBars[0] = new BoardSidebar
                    {
                        X = 0, Y = 1
                    },
                    DrawableBoard = new DrawableBoard(Board)
                    {
                        X = 4.5f, Y = 1,
                    },
                    sideBars[1] = new BoardSidebar
                    {
                        X = AppliedStackRuleset.BOARD_WIDTH + 5, Y = 1
                    },
                }
            };

            sideBars[0].AddRange(new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "HOLD",
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    X = 0, Y = 0,
                    Colour = OsuColour.Gray(1.0f),
                    Scale = new osuTK.Vector2(
                        1f / ScaledContainer.Scale.X,
                        1f / ScaledContainer.Scale.Y
                    )
                }
            });

            sideBars[1].AddRange(new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "NEXT",
                    X = 0, Y = 0,
                    Colour = OsuColour.Gray(1.0f),
                    Scale = new osuTK.Vector2(
                        1f / ScaledContainer.Scale.X,
                        1f / ScaledContainer.Scale.Y
                    )
                }
            });
        }

        public void SetNewCurrentPiece(PieceObject piece)
        {
            bool firstPiece = true;

            if (Current != null)
            {
                if (FancyAnimation.Value)
                {
                    DrawablePieceObject aux = drawableCurrent;
                    aux.DropAnimation(0, 150);
                    Scheduler.AddDelayed(() =>
                    {
                        ScaledContainer.Remove(aux);
                    }, 150);
                }
                else
                {
                    ScaledContainer.Remove(drawableCurrent);
                }

                drawableCurrent = null;
                firstPiece = false;
            }

            piece.ResetRotation();
            Current = piece;
            
            if (piece != null)
            {
                piece.X = (AppliedStackRuleset.BOARD_WIDTH - piece.Size) / 2;
                piece.Y = AppliedStackRuleset.BOARD_HEIGHT;
                ScaledContainer.Add(drawableCurrent = new DrawablePieceObject(Current));
                drawableCurrent.UpdatePosition();
                if (FancyAnimation.Value && !firstPiece) drawableCurrent.PushAnimation(0, 150);
                UpdateGhostPiece();
            }
        }

        public void HoldSwap()
        {
            if (HoldSwapped) return;

            if (Hold == null)
            {
                Hold = Current;
                PushNext();
            }
            else
            {
                PieceObject aux = Hold;
                Hold = Current;
                SetNewCurrentPiece(aux);
            }

            Hold.ResetRotation();
            HoldSwapped = true;

            if (drawableHold != null) sideBars[0].Remove(drawableHold);
            sideBars[0].Add(drawableHold = new DrawablePieceObject(Hold)
            {
                Colour = OsuColour.Gray(0.7f)
            });
            drawableHold.UpdatePosition();
            drawableHold.X = 0;
            drawableHold.Y = Hold.Size == 3? 1.5f : 0.5f;
        }

        private void UpdateGhostPiece()
        {
            if (drawableGhost != null) ScaledContainer.Remove(drawableGhost);

            PieceObject ghostPiece = Current.Clone();
            ghostPiece.Recolour(new Color4(
                Current.Tiles[0].Colour.R,
                Current.Tiles[0].Colour.G,
                Current.Tiles[0].Colour.B,
                Current.Tiles[0].Colour.A / 4f
            ));
            ScaledContainer.Add(drawableGhost = new DrawablePieceObject(ghostPiece));
            Board.SoftDrop(AppliedStackRuleset.BOARD_HEIGHT * 2, ghostPiece);
            drawableGhost.UpdatePosition();
        }

        private void UpdateNextQueue()
        {
            for (int i = 0; i < drawableNext.Length; i++)
            {
                float pieceDisplayY = 0.5f + i * 3f;
                bool isNullBefore = false;

                DrawablePieceObject currentNext = drawableNext[i];
                PieceObject.PieceFactory nextFactory = bufferedBag.Buffer[i];
                if (currentNext != null)
                {
                    if (FancyAnimation.Value)
                    {
                        DrawablePieceObject aux = currentNext;
                        aux.NextQueueLeaveAnimation(0, 150);
                        Scheduler.AddDelayed(() =>
                        {
                            sideBars[1].Remove(aux);
                        }, 150);
                    }
                    else sideBars[1].Remove(currentNext);
                }
                else isNullBefore = true;

                sideBars[1].Add(drawableNext[i] = currentNext = new DrawablePieceObject(nextFactory()));
                currentNext.UpdatePosition();

                currentNext.X = 0;
                if (currentNext.Piece.Size == 4) currentNext.Y = pieceDisplayY - 0.5f;
                if (currentNext.Piece.Size == 3) currentNext.Y = pieceDisplayY + 0.5f;

                if (!isNullBefore && FancyAnimation.Value) currentNext.NextQueueEnterAnimation(0, 150);
                else currentNext.Alpha = 1;
            }
        }

        private void PushNext()
        {
            SetNewCurrentPiece(bufferedBag.GetNextPiece(rng)());
            UpdateNextQueue();
        }

        [BackgroundDependencyLoader]
        private void load(AppliedStackConfigManager config)
        {
            AddInternal(HitObjectContainer);
            AddInternal(ScaledContainer);

            arr = config.Get<double>(AppliedStackSetting.AutoRepeatRate);
            das = config.Get<double>(AppliedStackSetting.DelayedAutoShift);
            sdr = config.Get<double>(AppliedStackSetting.SoftDropRate);
            FancyAnimation = config.GetBindable<bool>(AppliedStackSetting.FancyAnimations);

            RegisterRepeatingInputsHandler();

            RegisterPool<HardAction, DrawableHardAction>(12, 50);
            RegisterPool<SoftAction, DrawableSoftAction>(12, 50);
        }

        public override void PostProcess()
        {
            int bagSeed = 1337;
            Random processorRng = new Random(129481581);

            foreach (DrawableHitObject hitObj in AllHitObjects)
            {
                if (!(hitObj is DrawableAppliedStackHitObject asho)) continue;
                bagSeed ^= processorRng.Next(asho.HitObject.ObjectSeed);
            }

            rng = new Random(bagSeed);
            bufferedBag = new BufferedBag(new SevenBag());
            bufferedBag.InitializeBag(PieceObject.Factories(), rng);
            PushNext();

            base.PostProcess();
        }

        #region Input handling
        private int pieceDirection = 0;
        private double msUntilAutoRepeat = 0;
        private bool softDropping = false;
        private double msUntilSoftDrop = 0;

        private void RegisterRepeatingInputsHandler()
        {
            NewResult += OnNewResult;
            OnUpdate += OnUpdateRepeatingInputs;
        }

        private void OnUpdateRepeatingInputs(Drawable _)
        {
            double elasped = Time.Elapsed;
            if (pieceDirection != 0)
            {
                msUntilAutoRepeat -= elasped;
                while (msUntilAutoRepeat <= 0 && MoveToDirection(pieceDirection))
                {
                    msUntilAutoRepeat =+ arr;
                }
            }

            if (softDropping)
            {
                msUntilSoftDrop -= elasped;
                while (msUntilSoftDrop <= 0 && Board.SoftDrop(1, Current) != 1)
                {
                    msUntilSoftDrop =+ sdr;
                }
                drawableCurrent.UpdatePosition();
                UpdateGhostPiece();
            }
        }

        private void OnNewResult(DrawableHitObject ho, JudgementResult result)
        {
            if (!(ho is DrawableAppliedStackHitObject daho)) return;
            if (result.Type == HitResult.Miss || result.Type == HitResult.IgnoreMiss) return;

            AppliedStackAction lastAction = daho.LastAction;
            int linesCleared = HandleActions(lastAction);

            if (!daho.DisplayResult || !DisplayJudgements.Value) return;

            if (FancyAnimation.Value)
            {
                DrawableStackingResult stackResult = new DrawableStackingResult
                {
                    HitResult = result.Type <= HitResult.Great? result.Type : HitResult.None,
                    Scale = new Vector2(1f / ScaledContainer.Scale.X, 1f / ScaledContainer.Scale.Y),
                    Position = judgementDisplayCenter - ScaledContainer.Size / 2f,
                    StackType = (StackingResultType)linesCleared // TODO: Add T-spin thing
                };
                ScaledContainer.Add(stackResult);
                stackResult.PlayAnimation();
                Scheduler.AddDelayed(() =>
                {
                    ScaledContainer.Remove(stackResult);
                }, 1500);
            }
        }

        private bool MoveToDirection(int direction)
        {
            Current.X += direction;
            if (Board.IsOccupied(Current))
            {
                Current.X -= direction;
                return false;
            }

            drawableCurrent.UpdatePosition();
            UpdateGhostPiece();
            return true;
        }

        public int HandleActions(AppliedStackAction action)
        {
            int linesCleared = 0;
            DrawablePieceObject target = drawableCurrent;
            switch (action)
            {
                case AppliedStackAction.RotateLeft:
                    Board.Rotate(Current, -1);
                    drawableCurrent.UpdatePosition();
                    UpdateGhostPiece();
                    break;
                case AppliedStackAction.RotateRight:
                    Board.Rotate(Current, 1);
                    drawableCurrent.UpdatePosition();
                    UpdateGhostPiece();
                    break;
                case AppliedStackAction.MoveLeft:
                    pieceDirection = -1;
                    msUntilAutoRepeat = das;
                    MoveToDirection(-1);
                    drawableCurrent.UpdatePosition();
                    break;
                case AppliedStackAction.MoveRight:
                    pieceDirection = 1;
                    msUntilAutoRepeat = das;
                    MoveToDirection(1);
                    drawableCurrent.UpdatePosition();
                    break;
                case AppliedStackAction.SoftDrop:
                    softDropping = true;
                    msUntilSoftDrop = sdr;
                    Board.SoftDrop(1, Current);
                    drawableCurrent.UpdatePosition();
                    UpdateGhostPiece();
                    break;
                case AppliedStackAction.HardDrop:
                    drawableCurrent.HardDropTravelledLines = Board.HardDrop(Current);
                    linesCleared = Board.RemoveFilledLines();
                    DrawableBoard.RefreshTiles();
                    PushNext();
                    HoldSwapped = false;
                    if (drawableHold != null) drawableHold.Colour = OsuColour.Gray(1.0f);
                    if (FancyAnimation.Value) DrawableBoard.HardDropAnimation(0, 500);
                    break;
                case AppliedStackAction.Hold:
                    HoldSwap();
                    break;
            }
            
            judgementDisplayCenter = new Vector2(
                target.XIfUpdate + target.Piece.Size / 2f,
                target.YIfUpdate + target.Piece.Size / 2f
            );

            return linesCleared;
        }

        public bool OnPressed(KeyBindingPressEvent<AppliedStackAction> e)
        {
            if (!HandlingActions.Where(v => v == e.Action).Any()) return false;
            HandleActions(e.Action);
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<AppliedStackAction> e)
        {
            switch (e.Action)
            {
                case AppliedStackAction.MoveLeft:
                    if (pieceDirection == -1) pieceDirection = 0;
                    break;
                case AppliedStackAction.MoveRight:
                    if (pieceDirection == 1) pieceDirection = 0;
                    break;
                case AppliedStackAction.SoftDrop:
                    softDropping = false;
                    break;
            }
        }
        #endregion
    }
}
