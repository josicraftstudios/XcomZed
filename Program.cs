using entitys;
using World;

public class Xcom() {

    public static int width=10;
    public static int height=10;

    public static Tile[][] Map = world.CreateGrid(width, height);

    static void Main()
    {
        Operator guy = new Operator("Emil", [2, 2]);
        Console.WriteLine(guy.Name);

        world.Render(width, height);
    }
}
