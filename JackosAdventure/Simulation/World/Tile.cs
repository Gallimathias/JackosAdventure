using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JackosAdventure.Simulation.World
{
    public class Tile
    {
        public string Name { get; }

        public Tile(string name)
        {
            Name = name;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Name);
        }

        public static Tile Deserialize(BinaryReader reader)
        {
            var name = reader.ReadString();
            return new Tile(name);
        }
    }
}
