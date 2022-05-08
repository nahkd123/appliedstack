using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects.Drawables
{
    public class DrawablePieceObject : CompositeDrawable
    {
        public PieceObject Piece { get; }
        public int HardDropTravelledLines { get; set; } = 1;
        private DrawableTileObject[] drawableTiles;

        public float XIfUpdate => Piece.X + 4.5f;
        public float YIfUpdate => AppliedStackRuleset.BOARD_HEIGHT - 1 - Piece.Y - Piece.Size + 2;

        public DrawablePieceObject(PieceObject obj)
        {
            Piece = obj;
            drawableTiles = new DrawableTileObject[obj.Tiles.Length];
            for (int i = 0; i < drawableTiles.Length; i++)
            {
                drawableTiles[i] = new DrawableTileObject(obj.Tiles[i]);
            }

            Width = Height = Piece.Size;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(drawableTiles);
        }

        public void UpdatePosition()
        {
            X = XIfUpdate;
            Y = YIfUpdate;
            foreach (DrawableTileObject tile in drawableTiles)
            {
                tile.X = tile.Tile.X;
                tile.Y = Piece.Size - 1 - tile.Tile.Y;
            }
        }

        public void PushAnimation(double delay, double pushTime)
        {
            float targetedY = Y;

            Alpha = 0;
            Position += new osuTK.Vector2(0, -0.5f);

            using (this.BeginDelayedSequence(delay))
            {
                this.FadeIn(pushTime - 80);
                this.MoveToY(targetedY + 0.2f, pushTime - 80, Easing.OutCubic);
            }
            using (this.BeginDelayedSequence(delay + pushTime - 80))
            {
                this.MoveToY(targetedY, 80, Easing.OutCubic);
            }
        }

        public void DropAnimation(double delay, double dropTime)
        {
            float targetedY = Y + HardDropTravelledLines / 2;

            using (this.BeginDelayedSequence(delay))
            {
                this.FadeOut(dropTime);
                this.MoveToY(targetedY, dropTime, Easing.OutCubic);
            }
        }

        public void NextQueueEnterAnimation(double delay, double animTime)
        {
            float targetedY = Y;
            Y = targetedY + 3;
            Alpha = 0;

            using (this.BeginDelayedSequence(delay))
            {
                this.FadeIn(animTime);
                this.MoveToY(targetedY, animTime, Easing.OutCubic);
            }
        }

        public void NextQueueLeaveAnimation(double delay, double animTime)
        {
            float targetedY = Y - 3;

            using (this.BeginDelayedSequence(delay))
            {
                this.FadeOut(animTime);
                this.MoveToY(targetedY, animTime, Easing.OutCubic);
            }
        }
    }
}