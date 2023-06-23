namespace Quadrolink.Models;

public class Spot
{
    public bool IsFilled { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int BoardPosition { get; set; }
    public string Fill { get; set; }
    
}