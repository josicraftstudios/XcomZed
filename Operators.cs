namespace entitys;

public class Operator:Entity
{
    public char Sign;
    public ConsoleColor color;

    public Operator(string name,char Sign)
    {
        this.Name = name;
        this.Sign = Sign;
        this.color = color;
    }

}

public class OpData
{
    public static Operator[] Troops =
    [
        new Operator("Merlin",'M'),
        new Operator("Whistle",'W'),
        new Operator("Hedgehog",'S'),
        new Operator("Bullfrog",'F'),
        new Operator("Bison",'B')
    ];
}
