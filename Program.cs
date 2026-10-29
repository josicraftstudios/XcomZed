using entitys;
using World;

class Xcom() {

    Tile[,] Map = world.CreateGrid(10,10);

    static void Main()
    {
        Operator guy = new Operator("Emil", [2, 2]);
        Console.WriteLine(guy.Name);
    }
}
