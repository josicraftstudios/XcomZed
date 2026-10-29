namespace World;

using Renderring;

class world()
{
    public static Tile[][] CreateGrid(int width, int height)
    {
        Tile[][] outcome = new Tile[width][height];
        return outcome;
    }

    public static void Render(){
        Rendere.RenderWorld();
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
