﻿namespace codeshaper.util {

    public static class Layers {

        // Built in.
        public const int DEFAULT = 0;
        public const int TRANSPARENT_FX = 1;
        public const int IGNORE_RAYCAST = (1 << 2);
        public const int WATER = (1 << 4);
        public const int UI = (1 << 5);

        // Defined
        /// <summary> Layer for all objects that should block the placement of a building. </summary>
        public const int GEOMETRY = (1 << 9);
        public const int WALL = (1 << 9);
    }
}
