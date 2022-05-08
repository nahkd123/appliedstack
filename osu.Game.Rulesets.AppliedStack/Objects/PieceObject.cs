using System;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects
{
    public class PieceObject
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Size { get; set; } = 3;
        public RotationOffsets RotationOffsets { get; set; }
        public TileObject[] Tiles { get; set; } = new TileObject[0];
        public int CurrentRotation { get; set; } = 0;

        public void Recolour(Color4 newColour)
        {
            foreach (TileObject tile in Tiles) tile.Colour = newColour;
        }

        public void Rotate(int direction)
        {
            double centerLoc = Size / 2.0;
            foreach (TileObject tile in Tiles)
            {
                double localX = (tile.X + .5) - centerLoc;
                double localY = (tile.Y + .5) - centerLoc;
                double aux = localX;

                switch (direction)
                {
                    case 1:
                        localX = localY;
                        localY = -aux;
                        break;
                    case -1:
                        localX = -localY;
                        localY = aux;
                        break;
                }

                tile.X = (int)Math.Round(localX + centerLoc - .5);
                tile.Y = (int)Math.Round(localY + centerLoc - .5);
            }

            CurrentRotation += direction;
            if (CurrentRotation >= 4) CurrentRotation %= 4;
            if (CurrentRotation < 0) CurrentRotation += 4;
        }

        public void ResetRotation()
        {
            while (CurrentRotation != 0) Rotate(CurrentRotation > 0? -1 : 1);
        }

        public PieceObject Clone()
        {
            TileObject[] tiles = new TileObject[Tiles.Length];
            for (int i = 0; i < Tiles.Length; i++) tiles[i] = new TileObject
            {
                X = Tiles[i].X,
                Y = Tiles[i].Y,
                Colour = Tiles[i].Colour
            };

            return new PieceObject
            {
                X = X,
                Y = Y,
                Size = Size,
                Tiles = tiles,
                RotationOffsets = RotationOffsets,
                CurrentRotation = CurrentRotation
            };
        }

        public static PieceObject CreateIPiece() => new PieceObject
        {
            Size = 4,
            Tiles = new[]
            {
                new TileObject { X = 0, Y = 2, Colour = AppliedStackColours.PIECE_I },
                new TileObject { X = 1, Y = 2, Colour = AppliedStackColours.PIECE_I },
                new TileObject { X = 2, Y = 2, Colour = AppliedStackColours.PIECE_I },
                new TileObject { X = 3, Y = 2, Colour = AppliedStackColours.PIECE_I }
            },
            RotationOffsets = RotationOffsets.I
        };
        public static PieceObject CreateJPiece() => new PieceObject
        {
            Size = 3,
            Tiles = new[]
            {
                new TileObject { X = 0, Y = 2, Colour = AppliedStackColours.PIECE_J },
                new TileObject { X = 0, Y = 1, Colour = AppliedStackColours.PIECE_J },
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_J },
                new TileObject { X = 2, Y = 1, Colour = AppliedStackColours.PIECE_J }
            },
            RotationOffsets = RotationOffsets.JLSTZ
        };
        public static PieceObject CreateLPiece() => new PieceObject
        {
            Size = 3,
            Tiles = new[]
            {
                new TileObject { X = 2, Y = 2, Colour = AppliedStackColours.PIECE_L },
                new TileObject { X = 0, Y = 1, Colour = AppliedStackColours.PIECE_L },
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_L },
                new TileObject { X = 2, Y = 1, Colour = AppliedStackColours.PIECE_L }
            },
            RotationOffsets = RotationOffsets.JLSTZ
        };
        public static PieceObject CreateOPiece() => new PieceObject
        {
            Size = 4,
            Tiles = new[]
            {
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_O },
                new TileObject { X = 2, Y = 1, Colour = AppliedStackColours.PIECE_O },
                new TileObject { X = 1, Y = 2, Colour = AppliedStackColours.PIECE_O },
                new TileObject { X = 2, Y = 2, Colour = AppliedStackColours.PIECE_O }
            },
            RotationOffsets = RotationOffsets.O
        };
        public static PieceObject CreateSPiece() => new PieceObject
        {
            Size = 3,
            Tiles = new[]
            {
                new TileObject { X = 0, Y = 1, Colour = AppliedStackColours.PIECE_S },
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_S },
                new TileObject { X = 1, Y = 2, Colour = AppliedStackColours.PIECE_S },
                new TileObject { X = 2, Y = 2, Colour = AppliedStackColours.PIECE_S }
            },
            RotationOffsets = RotationOffsets.JLSTZ
        };
        public static PieceObject CreateTPiece() => new PieceObject
        {
            Size = 3,
            Tiles = new[]
            {
                new TileObject { X = 0, Y = 1, Colour = AppliedStackColours.PIECE_T },
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_T },
                new TileObject { X = 2, Y = 1, Colour = AppliedStackColours.PIECE_T },
                new TileObject { X = 1, Y = 2, Colour = AppliedStackColours.PIECE_T }
            },
            RotationOffsets = RotationOffsets.JLSTZ
        };
        public static PieceObject CreateZPiece() => new PieceObject
        {
            Size = 3,
            Tiles = new[]
            {
                new TileObject { X = 1, Y = 1, Colour = AppliedStackColours.PIECE_Z },
                new TileObject { X = 2, Y = 1, Colour = AppliedStackColours.PIECE_Z },
                new TileObject { X = 0, Y = 2, Colour = AppliedStackColours.PIECE_Z },
                new TileObject { X = 1, Y = 2, Colour = AppliedStackColours.PIECE_Z }
            },
            RotationOffsets = RotationOffsets.JLSTZ
        };

        public delegate PieceObject PieceFactory();
        public static PieceFactory[] Factories() => new PieceFactory[]
        {
            CreateIPiece,
            CreateJPiece,
            CreateLPiece,
            CreateOPiece,
            CreateSPiece,
            CreateTPiece,
            CreateZPiece
        };
    }

    public class RotationOffsets
    {
        public Vector2[][] Clockwise { get; set; }
        public Vector2[][] CounterClockwise { get; set; }

        public Vector2[] GetOffsetSets(int fromRot, int direction)
        {
            return direction switch
            {
                1 => Clockwise[fromRot],
                -1 => CounterClockwise[fromRot],
                _ => null
            };
        }

        private static Vector2 V2(float x, float y) => new Vector2(x, y);

        public static readonly RotationOffsets JLSTZ = new RotationOffsets
        {
            Clockwise = new[]
            {
                new[] { V2(  0,  0), V2(-1,  0), V2(-1, +1), V2(  0, -2), V2(-1, -2) }, // 0 -> R
                new[] { V2(  0,  0), V2(+1,  0), V2(+1, -1), V2(  0, +2), V2(+1, +2) }, // R -> 2
                new[] { V2(  0,  0), V2(+1,  0), V2(+1, +1), V2(  0, -2), V2(+1, -2) }, // 2 -> L
                new[] { V2(  0,  0), V2(-1,  0), V2(-1, -1), V2(  0, +2), V2(-1, +2) }  // L -> 0
            },
            CounterClockwise = new[]
            {
                new[] { V2(  0,  0), V2(+1,  0), V2(+1, +1), V2(  0, -2), V2(+1, -2) }, // 0 -> L
                new[] { V2(  0,  0), V2(+1,  0), V2(+1, -1), V2(  0, +2), V2(+1, +2) }, // R -> 0
                new[] { V2(  0,  0), V2(-1,  0), V2(-1, +1), V2(  0, -2), V2(-1, -2) }, // 2 -> R
                new[] { V2(  0,  0), V2(-1,  0), V2(-1, -1), V2(  0, +2), V2(-1, +2) }  // L -> 2
            }
        };

        public static readonly RotationOffsets I = new RotationOffsets
        {
            Clockwise = new[]
            {
                new[] { V2(  0,  0), V2(-2,  0), V2(+1,  0), V2( -2, -1), V2(+1, +2) }, // 0 -> R
                new[] { V2(  0,  0), V2(-1,  0), V2(+2,  0), V2( -1, +2), V2(+2, -1) }, // R -> 2
                new[] { V2(  0,  0), V2(+2,  0), V2(-1,  0), V2( +2, +1), V2(-1, -2) }, // 2 -> L
                new[] { V2(  0,  0), V2(+1,  0), V2(-2,  0), V2( +1, -2), V2(-2, +1) }  // L -> 0
            },
            CounterClockwise = new[]
            {
                new[] { V2(  0,  0), V2(-1,  0), V2(+2,  0), V2( -1, +2), V2(+2, -1) }, // 0 -> L
                new[] { V2(  0,  0), V2(+2,  0), V2(-1,  0), V2( +2, +1), V2(-1, -2) }, // R -> 0
                new[] { V2(  0,  0), V2(+1,  0), V2(-2,  0), V2( +1, -2), V2(-2, +1) }, // 2 -> R
                new[] { V2(  0,  0), V2(-2,  0), V2(+1,  0), V2( -2, -1), V2(+1, +2) }  // L -> 2
            }
        };

        public static readonly RotationOffsets O = new RotationOffsets
        {
            Clockwise = new[]
            {
                new[] { V2(  0,  0) }, // 0 -> R
                new[] { V2(  0,  0) }, // R -> 2
                new[] { V2(  0,  0) }, // 2 -> L
                new[] { V2(  0,  0) }  // L -> 0
            },
            CounterClockwise = new[]
            {
                new[] { V2(  0,  0) }, // 0 -> L
                new[] { V2(  0,  0) }, // R -> 0
                new[] { V2(  0,  0) }, // 2 -> R
                new[] { V2(  0,  0) }  // L -> 2
            }
        };
    }
}