namespace Renderring;

using World;
using entitys;

class Rendere
{
    static public void RenderWorld()
    {
        RenderTile[,] Grid = new RenderTile[Xcom.Map.Length, Xcom.Map[0].Length];
    }


}

public class RenderTile{
    char Sign;
    ConsoleColor color;

    public RenderTile(char Sign, ConsoleColor color){
        this.Sign = Sign;
        this.color = color;
    }

}
