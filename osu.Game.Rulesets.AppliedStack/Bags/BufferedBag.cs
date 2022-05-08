using System;
using osu.Game.Rulesets.AppliedStack.Objects;

namespace osu.Game.Rulesets.AppliedStack.Bags
{
    public class BufferedBag : BaseBag
    {
        public int BufferSize { get; set; } = 5;
        public BaseBag RealBag { get; private set; }
        public PieceObject.PieceFactory[] Buffer { get; private set; }
        public PieceObject.PieceFactory Next => Buffer[0];

        public BufferedBag(BaseBag bag)
        {
            RealBag = bag;
        }

        public override PieceObject.PieceFactory GetNextPiece(Random rng)
        {
            PieceObject.PieceFactory next = Buffer[0];
            Array.Copy(Buffer, 1, Buffer, 0, Buffer.Length - 1);
            Buffer[Buffer.Length - 1] = RealBag.GetNextPiece(rng);
            return next;
        }

        public override void InitializeBag(PieceObject.PieceFactory[] factories, Random rng)
        {
            RealBag.InitializeBag(factories, rng);
            Buffer = new PieceObject.PieceFactory[BufferSize];
            for (int i = 0; i < BufferSize; i++) Buffer[i] = RealBag.GetNextPiece(rng);
        }
    }
}