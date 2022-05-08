using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects.Drawables
{
    public class DrawableTileObject : CompositeDrawable
    {
        public TileObject Tile { get; }

        public DrawableTileObject(TileObject tile)
        {
            Tile = tile;
            Width = 1;
            Height = 1;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new[]
            {
                new Box
                {
                    X = 0, Y = 0,
                    Width = 1, Height = 1,
                    Colour = new Color4(0.7f, 0.7f, 0.7f, 1f)
                },
                new Box
                {
                    X = 0.2f, Y = 0.2f,
                    Width = 0.8f, Height = 0.8f,
                    Colour = new Color4(0.8f, 0.8f, 0.8f, 1f)
                },
                new Box
                {
                    X = 0, Y = 0,
                    Width = 0.8f, Height = 0.8f,
                    Colour = new Color4(1f, 1f, 1f, 1f)
                },
            });
        }

        protected override void Update()
        {
            base.Update();
            Colour = Tile.Colour;
        }
    }
}