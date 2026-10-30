namespace Renderring;

using World;
using entitys;

class Rendere
{
    static RenderTile[,] Grid = new RenderTile[
        Xcom.width, Xcom.height
    ];
    static public void RenderWorld(int width, int height)
    {
        Grid = new RenderTile[
            width, height
        ];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (Xcom.Map[i][j].Door)
                {
                    Grid[i, j] = new RenderTile('H', ConsoleColor.Yellow);
                }
                else if (Xcom.Map[i][j].Block)
                {
                    Grid[i, j] = new RenderTile('#', ConsoleColor.White);
                }
                else if (Xcom.Map[i][j].Cover)
                {
                    Grid[i, j] = new RenderTile('¤', ConsoleColor.Gray);
                }
                else
                {
                    Grid[i, j] = new RenderTile(' ', ConsoleColor.White);
                }

            }
        }
        //Add player troops

        foreach (Operator troop in Xcom.PlayerTroops)
        {
            Grid[troop.x, troop.y].Sign = troop.Sign;
            Grid[troop.x, troop.y].color = troop.color;
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
