namespace Renderring;

using World;
using entitys;

class Rendere
{
    static public void RenderWorld(int width, int height)
    {
        RenderTile[,] Grid = new RenderTile[
            width, height
        ];

        for (int i = 0; i <width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Grid[i, j] = new RenderTile('#',ConsoleColor.Red);
            }
        }

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Console.ForegroundColor = Grid[i, j].color;
                Console.Write($"{Grid[i, j].Sign}  ");
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }


}

public class RenderTile{
    public char Sign;
    public ConsoleColor color;

    public RenderTile(char Sign, ConsoleColor color){
        this.Sign = Sign;
        this.color = color;
    }

}
