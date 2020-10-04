using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisual
{
    public class Actor
    {
        public string Path { get; private set; }
        public string Type { get; private set; }

        public Actor(string path, string type)
        {
            Path = path;
            Type = type ?? "Unknown";
        }
    }
}
