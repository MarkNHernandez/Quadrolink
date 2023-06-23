using Quadrolink.Pages.Constants;

namespace Quadrolink.Models;

public class GameButton
{
    public bool TurnStatus { get; set; }
    public int ColumnIndex { get; set; }
    public string Fill => TurnStatus ? "#FFFF00" : "#FF0000";
}