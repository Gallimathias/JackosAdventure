using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JackosAdventure.Simulation.World
{
    public class Map
    {
        public Tile[,] Tiles { get; }

        public int Width { get; set; }
        public int Height { get; set; }

        public List<string> TileTypes { get; }

        public Map(int width, int height)
        {
            TileTypes = new List<string>();
            Tiles = new Tile[width, height];
            Width = width;
            Height = height;
        }

        public void Init()
        {
            TileTypes.Add("grass");
            TileTypes.Add("grass_blue");

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y] = new Tile("grass");
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Width);
            writer.Write(Height);

            writer.Write(TileTypes.Count);

            foreach (var tileType in TileTypes)
            {
                writer.Write(tileType);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y].Serialize(writer);
                }
            }
        }

        public static Map Deserialize(BinaryReader reader)
        {
            var width = reader.ReadInt32();
            var height = reader.ReadInt32();

            var map = new Map(width, height);
            var count = reader.ReadInt32();


            for (int i = 0; i < count; i++)
            {
                map.TileTypes.Add(reader.ReadString());
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map.Tiles[x, y] = Tile.Deserialize(reader);
                }
            }

            return map;
        }
    }
}
