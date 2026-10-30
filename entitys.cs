namespace entitys;
using World;

public abstract class Entity()
{
    public int Hp;
    public bool Player;
    public int x;
    public int y;
    public int[] inventory;
    public int Spd;
    public string Name;
}


public static class LineOfSight
{
    private static readonly Random rng = new Random();

    public static bool CanSee(
        Tile[][] map,
        int x0, int y0,
        int x1, int y1)
    {
        foreach (var (x, y) in GetLine(x0, y0, x1, y1))
        {
            // Don't let the shooter's or target's tile
            // block the line of sight.
            if ((x == x0 && y == y0) ||
                (x == x1 && y == y1))
                continue;

            if (map[x][y].Block)
                return false;
        }

        return true;
    }

    public static double GetCoverHitChance(
        Tile[][] map,
        int x0, int y0,
        int x1, int y1)
    {
        if (!CanSee(map, x0, y0, x1, y1))
            return 0.0;

        // Count cover only on tiles immediately
        // adjacent to the target along the line.
        int dx = Math.Sign(x1 - x0);
        int dy = Math.Sign(y1 - y0);

        int x = x1 - dx;
        int y = y1 - dy;

        // The tile immediately before the target
        // is the only tile that can provide cover.
        if (x == x0 && y == y0)
            return 1.0;

        if (map[x][y].Cover)
            return 0.5;

        return 1.0;
    }

    private static System.Collections.Generic.List<(int x, int y)>
        GetLine(int x0, int y0, int x1, int y1)
    {
        var points =
            new System.Collections.Generic.List<(int x, int y)>();

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            points.Add((x0, y0));

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return points;
    }
}
