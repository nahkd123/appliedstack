using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects
{
    public class Board
    {
        //public TileObject[] Tiles { get; } = new TileObject[AppliedStackRuleset.BOARD_WIDTH * AppliedStackRuleset.BOARD_HEIGHT];
        public List<TileObject[]> Lines { get; } = new List<TileObject[]>();

        public Board()
        {
            while (Lines.Count < AppliedStackRuleset.BOARD_HEIGHT)
            {
                Lines.Add(new TileObject[AppliedStackRuleset.BOARD_WIDTH]);
            }
        }

        public TileObject GetTileAt(int x, int y)
        {
            if (y < 0 || y >= AppliedStackRuleset.BOARD_HEIGHT || x < 0 || x >= AppliedStackRuleset.BOARD_WIDTH) return null;
            return Lines[y][x] != null? new TileObject
            {
                X = x,
                Y = y,
                Colour = Lines[y][x].Colour
            } : null;
        }

        public void SetTile(TileObject newTile)
        {
            int x = newTile.X, y = newTile.Y;
            if (y < 0 || y >= AppliedStackRuleset.BOARD_HEIGHT || x < 0 || x >= AppliedStackRuleset.BOARD_WIDTH) return;

            Lines[y][x] = new TileObject
            {
                X = x,
                Y = y,
                Colour = newTile.Colour
            };
        }

        public void RemoveTile(int x, int y)
        {
            if (y < 0 || y >= AppliedStackRuleset.BOARD_HEIGHT || x < 0 || x >= AppliedStackRuleset.BOARD_WIDTH) return;
            Lines[y][x] = null;
        }

        public void SetTilesFromPiece(PieceObject piece)
        {
            foreach (TileObject tile in piece.Tiles)
            {
                SetTile(new TileObject
                {
                    X = piece.X + tile.X,
                    Y = piece.Y + tile.Y,
                    Colour = tile.Colour
                });
            }
            return;
        }

        public bool IsOccupied(PieceObject piece)
        {
            foreach (TileObject tile in piece.Tiles)
            {
                int tileX = piece.X + tile.X;
                int tileY = piece.Y + tile.Y;
                if (tileX < 0 || tileX >= AppliedStackRuleset.BOARD_WIDTH || tileY < 0) return true;
                if (GetTileAt(tileX, tileY) != null) return true;
            }

            return false;
        }

        public int SoftDrop(int amount, PieceObject piece)
        {
            while (amount > 0)
            {
                piece.Y -= 1;
                if (IsOccupied(piece))
                {
                    piece.Y += 1;
                    return amount;
                }
                amount--;
            }
            return 0;
        }

        public int HardDrop(PieceObject piece)
        {
            int travelledLines = AppliedStackRuleset.BOARD_HEIGHT * 2 - SoftDrop(AppliedStackRuleset.BOARD_HEIGHT * 2, piece);
            SetTilesFromPiece(piece);
            return travelledLines;
        }

        public bool Rotate(PieceObject piece, int direction)
        {
            Vector2[] offsetsSet = piece.RotationOffsets.GetOffsetSets(piece.CurrentRotation, direction);
            piece.Rotate(direction);

            for (int i = 0; i < offsetsSet.Length; i++)
            {
                Vector2 offset = offsetsSet[i];
                piece.X += (int)offset.X;
                piece.Y += (int)offset.Y;

                if (IsOccupied(piece))
                {
                    piece.X -= (int)offset.X;
                    piece.Y -= (int)offset.Y;
                    continue;
                }
                else
                {
                    return true;
                }
            }

            piece.Rotate(-direction);
            return false;
        }

        private bool IsLineFilled(int line)
        {
            TileObject[] tiles = Lines[line];
            for (int i = 0; i < AppliedStackRuleset.BOARD_WIDTH; i++)
            {
                if (tiles[i] == null) return false;
            }

            return true;
        }

        private bool IsLineFilledWithNonBoardPiece(int line, PieceObject piece)
        {
            TileObject[] tiles = Lines[line];
            for (int i = 0; i < AppliedStackRuleset.BOARD_WIDTH; i++)
            {
                if (tiles[i] == null && !piece.Tiles.Where(v => (v.Y + piece.Y) == line && (v.X + piece.X) == i).Any()) return false;
            }

            return true;
        }

        private void RemoveLine(int line)
        {
            Lines.RemoveAt(line);
            Lines.Add(new TileObject[AppliedStackRuleset.BOARD_WIDTH]);
        }

        public int CountFilledLinesWithNonBoardPiece(PieceObject piece)
        {
            int filled = 0;
            for (int i = 0; i < AppliedStackRuleset.BOARD_HEIGHT; i++)
            {
                if (IsLineFilledWithNonBoardPiece(i, piece)) filled++;
            }
            return filled;
        }

        public int RemoveFilledLines()
        {
            int filled = 0, line = 0;
            while (line < AppliedStackRuleset.BOARD_HEIGHT)
            {
                if (IsLineFilled(line))
                {
                    RemoveLine(line);
                    filled++;
                }
                else line++;
            }
            return filled;
        }

        public void LogBoard()
        {
            for (int i = AppliedStackRuleset.BOARD_HEIGHT - 1; i >= 0; i--)
            {
                TileObject[] line = Lines[i];
                string logLine = "";
                for (int j = 0; j < line.Length; j++)
                {
                    logLine += line[j] == null? ".." : "##";
                }

                Logger.Log($"{i}: {logLine}");
            }
        }
    }
}