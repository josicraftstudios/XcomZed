namespace entitys;

public class Operator:Entity
{
    public Operator(string name,int[] pos){
        this.x = pos[0];
        this.y = pos[1];
        this.Name = name;
    }
}
