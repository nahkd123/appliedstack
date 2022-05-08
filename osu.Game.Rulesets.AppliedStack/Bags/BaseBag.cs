using System;
using osu.Game.Rulesets.AppliedStack.Objects;

namespace osu.Game.Rulesets.AppliedStack.Bags
{
    public abstract class BaseBag
    {
        public abstract void InitializeBag(PieceObject.PieceFactory[] factories, Random rng);
        public abstract PieceObject.PieceFactory GetNextPiece(Random rng);
    }
}