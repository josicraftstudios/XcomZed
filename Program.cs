using entitys;
using World;
using Input;

public class Xcom() {

    private static readonly Random rng = new Random();
    public static int width = 30;
    public static int height=30;

    public static Tile[][] Map = world.CreateGrid(width, height);

    public static Operator[] PlayerTroops = new Operator[2];


    static void Main()
    {
        PlayerTroops[0] = OpData.Troops[rng.Next(0, OpData.Troops.Length)];
        PlayerTroops[1] = PlayerTroops[0];
        while (PlayerTroops[1]==PlayerTroops[0]) {
            PlayerTroops[1] = OpData.Troops[rng.Next(0, OpData.Troops.Length)];
        }
        PlayerTroops[0].x = 2;
        PlayerTroops[0].x = 3;
        PlayerTroops[0].color = ConsoleColor.Cyan;

        PlayerTroops[1].x = 3;
        PlayerTroops[1].x = 1;
        PlayerTroops[1].color = ConsoleColor.Green;

        world.GenerateMap();
        while (true)
        {
            Console.Clear();

            world.Render(width, height);

            Console.WriteLine(
                $"Selected: {PlayerTroops[Inputs.SelectedTroop].Name} | WASD: Move | TAB: Switch | Q: Quit");

            Inputs.HandleInput();
        }
    }
}
