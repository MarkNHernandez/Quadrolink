namespace Quadrolink.Models;

public class GameColumn
{
    public int ColumnIndex { get; set; }
    public List<Spot> Spots { get; set; } = new();
}