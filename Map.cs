namespace World;

using Renderring;
using System.Collections.Generic;

class world()
{
    public static Tile[][] CreateGrid(int width, int height)
    {
        Tile[][] outcome = new Tile[width][];

        for (int x = 0; x < width; x++)
        {
            outcome[x] = new Tile[height];

            for (int y = 0; y < height; y++)
            {
                outcome[x][y] = new Tile();
                outcome[x][y].Block = true;
            }
        }

        return outcome;
    }

    public static void Render(int width, int height)
    {
        Rendere.RenderWorld(width, height);
    }

    public static void MakeWall(int x1, int y1, int x2, int y2)
    {
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);

        int sx = x1 < x2 ? 1 : -1;
        int sy = y1 < y2 ? 1 : -1;

        int err = dx - dy;

        while (true)
        {
            Xcom.Map[x1][y1].Block = true;

            if (x1 == x2 && y1 == y2)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }

    private static readonly Random rng = new Random();

    public static void GenerateMap()
    {
        int width = Xcom.Map.Length;
        int height = Xcom.Map[0].Length;

        // Start with a small room.
        int x = width / 2;
        int y = height / 2;

        CarveRoom(x - 2, y - 2, 5, 5);

        // Grow hallways in random directions.
        int hallwayCount = rng.Next(5, 9);

        for (int i = 0; i < hallwayCount; i++)
        {
            int direction = rng.Next(4);
            int length = rng.Next(4, 9);

            int hx = x;
            int hy = y;

            for (int step = 0; step < length; step++)
            {
                switch (direction)
                {
                    case 0: hx++; break; // East
                    case 1: hx--; break; // West
                    case 2: hy++; break; // South
                    case 3: hy--; break; // North
                }

                if (!InsideMap(hx, hy, width, height))
                    break;

                // Hallway is 3 tiles wide.
                CarveRoom(hx - 1, hy - 1, 3, 3);

                // Occasionally branch into a room.
                if (rng.Next(100) < 35)
                {
                    int rw = rng.Next(4, 8);
                    int rh = rng.Next(4, 7);

                    int rx = hx;
                    int ry = hy;

                    switch (direction)
                    {
                        case 0: rx += 2; break;
                        case 1: rx -= rw + 1; break;
                        case 2: ry += 2; break;
                        case 3: ry -= rh + 1; break;
                    }

                    if (InsideMap(rx, ry, width, height))
                        CarveRoom(rx, ry, rw, rh);
                }
            }
        }
        GenerateCover(12);
        ClearOutside();
        GenerateDoors(4);
        GenerateCover(12);
    }


    private static bool InsideMap(int x, int y, int width, int height)
    {
        return x >= 1 && y >= 1 &&
               x < width - 1 && y < height - 1;
    }

    private static void CarveRoom(int x, int y, int roomWidth, int roomHeight)
    {
        int mapWidth = Xcom.Map.Length;
        int mapHeight = Xcom.Map[0].Length;

        for (int rx = x; rx < x + roomWidth; rx++)
        {
            for (int ry = y; ry < y + roomHeight; ry++)
            {
                if (!InsideMap(rx, ry, mapWidth, mapHeight))
                    continue;

                Xcom.Map[rx][ry].Block = false;
            }
        }
    }

    public static void GenerateCover(int count)
    {
        int width = Xcom.Map.Length;
        int height = Xcom.Map[0].Length;

        Random rng = new Random();
        int placed = 0;
        int attempts = 0;

        while (placed < count && attempts < count * 20)
        {
            attempts++;

            int x = rng.Next(1, width - 1);
            int y = rng.Next(1, height - 1);

            // Only place cover on walkable tiles.
            if (Xcom.Map[x][y].Block)
                continue;

            Xcom.Map[x][y].Cover = true;
            placed++;
        }
    }

    public static void ClearOutside()
    {
        int width = Xcom.Map.Length;
        int height = Xcom.Map[0].Length;

        bool[,] outside = new bool[width, height];
        Queue<(int x, int y)> queue = new();

        queue.Enqueue((0, 0));
        outside[0, 0] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        // Flood-fill through outside tiles and wall tiles
        // that do NOT touch the walkable house interior.
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx < 0 || ny < 0 ||
                    nx >= width || ny >= height ||
                    outside[nx, ny])
                    continue;

                // Never flood into walkable floor.
                if (!Xcom.Map[nx][ny].Block)
                    continue;

                // Preserve walls touching walkable tiles.
                bool touchesFloor = false;

                for (int j = 0; j < 4; j++)
                {
                    int fx = nx + dx[j];
                    int fy = ny + dy[j];

                    if (fx >= 0 && fy >= 0 &&
                        fx < width && fy < height &&
                        !Xcom.Map[fx][fy].Block)
                    {
                        touchesFloor = true;
                        break;
                    }
                }

                if (touchesFloor)
                    continue;

                outside[nx, ny] = true;
                queue.Enqueue((nx, ny));
            }
        }

        // Remove every wall classified as outside.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (outside[x, y])
                    Xcom.Map[x][y].Block = false;
            }
        }
    }

    public static void GenerateDoors(int doorCount)
    {
        int width = Xcom.Map.Length;
        int height = Xcom.Map[0].Length;

        Random rng = new Random();
        var candidates = new List<(int x, int y)>();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (!Xcom.Map[x][y].Block)
                    continue;

                bool horizontalOpening =
                    !Xcom.Map[x - 1][y].Block &&
                    !Xcom.Map[x + 1][y].Block &&
                    Xcom.Map[x][y - 1].Block &&
                    Xcom.Map[x][y + 1].Block;

                bool verticalOpening =
                    !Xcom.Map[x][y - 1].Block &&
                    !Xcom.Map[x][y + 1].Block &&
                    Xcom.Map[x - 1][y].Block &&
                    Xcom.Map[x + 1][y].Block;

                if (horizontalOpening || verticalOpening)
                    candidates.Add((x, y));
            }
        }

        // Shuffle candidates so door locations are random.
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (candidates[i], candidates[j]) =
                (candidates[j], candidates[i]);
        }

        int placed = 0;

        foreach (var (x, y) in candidates)
        {
            if (placed >= doorCount)
                break;

            Xcom.Map[x][y].Door = true;
            Xcom.Map[x][y].Block = false;

            placed++;
        }
    }

    private static bool HasOpenNeighbor(int x, int y, int width, int height)
    {
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (nx >= 0 && ny >= 0 &&
                nx < width && ny < height &&
                !Xcom.Map[nx][ny].Block)
            {
                return true;
            }
        }

        return false;
    }
}

public class Tile{
    public bool Block;
    public bool Cover;
    bool Destructible;
    public bool Door;

    public bool getDestructible(){
        return Destructible;
    }
}
