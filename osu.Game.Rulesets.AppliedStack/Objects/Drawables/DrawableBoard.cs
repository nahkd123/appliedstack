using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects.Drawables
{
    public class DrawableBoard : CompositeDrawable
    {
        public Board Board { get; }
        public DrawableTileObject[] Tiles { get; } = new DrawableTileObject[AppliedStackRuleset.BOARD_WIDTH * AppliedStackRuleset.BOARD_HEIGHT];
        public bool TrackNewTiles { get; set; } = false;
        public List<DrawableTileObject> NewTiles { get; } = new List<DrawableTileObject>();

        public DrawableBoard(Board board)
        {
            Board = board;

            AutoSizeAxes = Axes.Both;
        }
        
        public void RefreshTiles()
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                int tileInternalX = i % AppliedStackRuleset.BOARD_WIDTH;
                int tileInternalY = i / AppliedStackRuleset.BOARD_WIDTH;

                DrawableTileObject drawableTile = Tiles[i];
                TileObject tile = Board.Lines[tileInternalY][tileInternalX];
                if (tile == null && drawableTile != null)
                {
                    Tiles[i] = null;
                    RemoveInternal(drawableTile);
                    NewTiles.Remove(drawableTile);
                }
                else if (tile != null && drawableTile == null)
                {
                    AddInternal(Tiles[i] = new DrawableTileObject(tile)
                    {
                        X = tileInternalX,
                        Y = AppliedStackRuleset.BOARD_HEIGHT - tileInternalY - 1,
                    });
                    NewTiles.Add(Tiles[i]);
                }
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new[]
            {
                new Box
                {
                    Colour = new Color4(0f, 0f, 0f, 0.5f),
                    X = 0, Y = 0,
                    Width = AppliedStackRuleset.BOARD_WIDTH,
                    Height = AppliedStackRuleset.BOARD_HEIGHT,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            });
        }

        public void HardDropAnimation(double delay, double duration)
        {
            double moveDownDuration = 80;
            double moveUpDuration = duration - moveDownDuration;

            using (this.BeginDelayedSequence(delay))
            {
                this.MoveToY(1.12f, moveDownDuration, Easing.OutCubic);
            }
            using (this.BeginDelayedSequence(delay + moveDownDuration))
            {
                this.MoveToY(1, moveUpDuration, Easing.OutCubic);
            }
        }
    }
}