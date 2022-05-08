using System;
using osu.Game.Rulesets.AppliedStack.Objects;

namespace osu.Game.Rulesets.AppliedStack.Bags
{
    public class SevenBag : BaseBag
    {
        public int ShuffleRounds { get; set; } = 42;

        private PieceObject.PieceFactory[] factories;
        private int pointer = 0;

        public override void InitializeBag(PieceObject.PieceFactory[] factories, Random rng)
        {
            this.factories = factories;
            this.pointer = factories.Length;
        }

        private void Shuffle(Random rng)
        {
            for (int i = 0; i < ShuffleRounds; i++)
            {
                int a = rng.Next(factories.Length);
                int b = rng.Next(factories.Length);
                PieceObject.PieceFactory aux = factories[a];
                factories[a] = factories[b];
                factories[b] = aux;
            }
        }

        public override PieceObject.PieceFactory GetNextPiece(Random rng)
        {
            if (pointer >= factories.Length)
            {
                pointer = 0;
                Shuffle(rng);
            }

            return factories[pointer++];
        }
    }
}