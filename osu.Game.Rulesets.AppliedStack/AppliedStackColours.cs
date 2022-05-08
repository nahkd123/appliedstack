using osu.Game.Rulesets.AppliedStack.Objects.Drawables;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack
{
    public static class AppliedStackColours
    {
        public static Color4 PIECE_I = new Color4(0f, 1f, 1f, 1f);
        public static Color4 PIECE_J = new Color4(0f, 0f, 1f, 1f);
        public static Color4 PIECE_L = new Color4(1f, 0.5f, 0f, 1f);
        public static Color4 PIECE_O = new Color4(1f, 1f, 0f, 1f);
        public static Color4 PIECE_S = new Color4(0f, 1f, 0f, 1f);
        public static Color4 PIECE_T = new Color4(0.8f, 0f, 1f, 1f);
        public static Color4 PIECE_Z = new Color4(1f, 0f, 0f, 1f);
        public static Color4 PIECE_GARBAGE = new Color4(.5f, .5f, .5f, 1f);

        public static Color4 GetFromStateValue(int stateValue)
        {
            return stateValue switch
            {
                1 => PIECE_I,
                2 => PIECE_J,
                3 => PIECE_L,
                4 => PIECE_O,
                5 => PIECE_S,
                6 => PIECE_T,
                7 => PIECE_Z,
                _ => PIECE_GARBAGE
            };
        }

        public static Color4 ForStackingResult(StackingResultType type)
        {
            return type switch
            {
                StackingResultType.Single => new Color4(1f, 1f, 1f, 1f),
                StackingResultType.Double => new Color4(1f, 0.85f, 0.85f, 1f),
                StackingResultType.Triple => new Color4(1f, 0.7f, 0.7f, 1f),
                StackingResultType.Quadruple => new Color4(1f, 0.6f, 0.6f, 1f),
                StackingResultType.TSpinSingle => PIECE_T,
                StackingResultType.TSpinDouble => PIECE_T,
                StackingResultType.TSpinTriple => PIECE_T,
                _ => new Color4(1f, 1f, 1f, 1f)
            };
        }
    }
}