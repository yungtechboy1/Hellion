﻿namespace Hellion.World
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new WorldServer())
                server.Start();
        }
    }
}
