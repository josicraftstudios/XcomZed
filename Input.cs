namespace Input;

using entitys;

class Inputs
{
    public static int SelectedTroop = 0;

    public static void HandleInput()
    {
        ConsoleKey key = Console.ReadKey(true).Key;

        Operator troop = Xcom.PlayerTroops[SelectedTroop];

        int newX = troop.x;
        int newY = troop.y;

        switch (key)
        {
            case ConsoleKey.W:
            case ConsoleKey.UpArrow:
                newX--;
                break;

            case ConsoleKey.S:
            case ConsoleKey.DownArrow:
                newX++;
                break;

            case ConsoleKey.A:
            case ConsoleKey.LeftArrow:
                newY--;
                break;

            case ConsoleKey.D:
            case ConsoleKey.RightArrow:
                newY++;
                break;

            case ConsoleKey.Tab:
                SelectedTroop = (SelectedTroop + 1) % Xcom.PlayerTroops.Length;
                return;

            case ConsoleKey.Q:
                Environment.Exit(0);
                return;

            default:
                return;
        }

        // Keep movement inside the map.
        if (newX < 0 || newY < 0 ||
            newX >= Xcom.Map.Length ||
            newY >= Xcom.Map[newX].Length)
            return;

        // Walls block movement.
        if (Xcom.Map[newX][newY].Block)
            return;

        // Don't allow troops to occupy the same tile.
        foreach (Operator other in Xcom.PlayerTroops)
        {
            if (other.x == newX && other.y == newY)
                return;
        }

        troop.x = newX;
        troop.y = newY;
    }

}
