using Microsoft.AspNetCore.Components;
using Quadrolink.Models;
using Quadrolink.Pages.Constants;

namespace Quadrolink.Pages.Bases;

public class QuadrolinkGameBase : ComponentBase
{
    #region Parameters

    #endregion

    #region Injections

    #endregion

    #region Properties

    private string color { get; set; } = TurnColors.RedTurn;
    protected List<Spot> GameSpots { get; set; } = new();
    protected List<GameButton> GameButtons { get; set; } = new();
    protected List<List<Spot>> GameColumns { get; set; } = new();
    protected int Rows { get; set; } = 6;
    protected int Columns { get; set; } = 7;
    
    
    #endregion

    #region Data Methods

    protected async Task DropCoin(int columnIndex, string coinColor)
    {
        var column = GameColumns.First(col => col.Any(s => s.ColumnIndex == columnIndex));
        if (column.All(s => s.IsFilled))
        {
            return;
        }

        var spot = column.OrderBy(s => s.RowIndex).First(p => p.IsFilled == false);
        spot.IsFilled = true;
        spot.Fill = coinColor;
        foreach (var button in GameButtons)
        {
            button.TurnStatus = !button.TurnStatus;
        }
        await InvokeAsync(StateHasChanged);
    }

    protected async Task CheckWinner(int columnIndex, int rowIndex)
    {
        

    }

    #endregion

    #region Setup

    private void FillBoard()
    {
        var position = 1;
        for (var column = Columns; column >= 1; column--)
        {
            for (var row = Rows; row >= 1; row--)
            {
                var newSpot = new Spot
                {
                    BoardPosition = position,
                    RowIndex = row,
                    ColumnIndex = column,
                    Fill = "#000000"
                };
                GameSpots.Add(newSpot);
                position++;
            }

            var newButton = new GameButton
            {
                ColumnIndex = column
            };
            var spotsInColumn = GameSpots.FindAll(s => s.ColumnIndex == column);
            GameColumns.Add(spotsInColumn);
            GameButtons.Add(newButton);
        }
    }

    #endregion

    #region Initializers

    protected override async Task OnInitializedAsync()
    {
        FillBoard();
        await InvokeAsync(StateHasChanged);
    }
    #endregion

    #region Finalizers

    #endregion
}