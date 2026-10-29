namespace World;

using Renderring;

class world()
{
    public static Tile[][] CreateGrid(int width, int height)
    {
        Tile[][] outcome = new Tile[width][];

        for (int x = 0; x < width; x++)
        {
            outcome[x] = new Tile[height];
        }
        return outcome;
    }

    public static void Render(int width, int height){
        Rendere.RenderWorld(width,height);
    }
}

public class Tile{
    public bool Block;
    public bool Cover;
    bool Destructible;

    public bool getDestructible(){
        return Destructible;
    }
}
