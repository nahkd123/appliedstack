using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.UI
{
    public class AppliedStackIcon : CompositeDrawable
    {
        public AppliedStackIcon()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Icon = FontAwesome.Regular.Circle
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new osuTK.Vector2(0.15f),
                    Rotation = -30f,
                    X = 0.01f, Y = -0.02f,
                    AutoSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        new Box
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Colour = new Color4(1f, 1f, 1f, 1f),
                            Width = 1.0f, Height = 1.0f,
                        },
                        new Box
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Colour = new Color4(1f, 1f, 1f, 1f),
                            Width = 1.0f, Height = 1.0f, Y = 1,
                        },
                        new Box
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Colour = new Color4(1f, 1f, 1f, 1f),
                            Width = 1.0f, Height = 1.0f, Y = 2,
                        },
                        new Box
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Colour = new Color4(1f, 1f, 1f, 1f),
                            Width = 1.0f, Height = 1.0f, X = 1, Y = 1,
                        }
                    }
                }
            };
        }
    }
}