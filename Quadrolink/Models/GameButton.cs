using Quadrolink.Pages.Constants;

namespace Quadrolink.Models;

public class GameButton
{
    //public bool TurnStatus { get; set; }
    public int ColumnIndex { get; set; }
    public string Fill { get; set; }
    public TurnStatus TurnStatus { get; set; }
}