using Humanizer;
using Microsoft.AspNetCore.Components;
using Quadrolink.Models;
using Quadrolink.Pages.Constants;

namespace Quadrolink.Pages.Bases;

public class QuadrolinkGameBase : ComponentBase
{

    #region Properties
    protected bool GameOver { get; private set; }
    private int TurnCount { get; set; } = 1;
    private int Rows { get; set; } = 6;
    private int Columns { get; set; } = 7;
    protected string GameStatus { get; private set; } = "";
    private List<Spot> GameSpots { get; set; } = new();
    protected List<GameButton> GameButtons { get; set; } = new();
    protected List<List<Spot>> GameColumns { get; set; } = new();
    private TurnStatus TurnStatus { get; set; } = TurnStatus.PlayerOne;

    #endregion

    #region Data Methods

    protected async Task DropCoin(int columnIndex, string coinColor)
    {
        TurnCount++;
        var column = GameColumns.First(col => col.Any(s => s.ColumnIndex == columnIndex));
        if (column.All(s => s.IsFilled))
        {
            return;
        }

        var spot = column.OrderBy(s => s.RowIndex).First(p => p.IsFilled == false);
        spot.IsFilled = true;
        spot.Fill = coinColor;
        spot.ClaimedBy = TurnStatus;
        await CheckWinner(spot.ColumnIndex, spot.RowIndex);
        ChangeButtonColors();
        await InvokeAsync(StateHasChanged);
    }

    private void ChangeButtonColors()
    {
        switch (TurnStatus)
        {
            case TurnStatus.PlayerOne:
                foreach (var button in GameButtons)
                {
                    button.Fill = TurnColors.PlayerTwo;
                }

                TurnStatus = TurnStatus.PlayerTwo;
                break;
            case TurnStatus.PlayerTwo:
                foreach (var button in GameButtons)
                {
                    button.Fill = TurnColors.PlayerOne;
                }

                TurnStatus = TurnStatus.PlayerOne;
                break;
            case TurnStatus.Neutral:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task CheckWinner(int columnIndex, int rowIndex)
    {
        if (TurnCount < 7)
            return;

        if (GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex     && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex - 1 && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex - 2 && s.ClaimedBy == TurnStatus) &&
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex - 3 && s.ClaimedBy == TurnStatus))
        {
            GameStatus = $"{TurnStatus.Humanize(LetterCasing.Title)} Wins!";
            GameOver = true;
            return;
        }
        if (GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex     && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex + 1 && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex + 2 && s.ClaimedBy == TurnStatus) &&
            GameSpots.Any(s => s.RowIndex == rowIndex && s.ColumnIndex == columnIndex + 3 && s.ClaimedBy == TurnStatus))
        {
            GameStatus = $"{TurnStatus.Humanize(LetterCasing.Title)} Wins!";
            GameOver = true;
            return;
        }
        
        if (GameSpots.Any(s => s.RowIndex == rowIndex     && s.ColumnIndex == columnIndex && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 1 && s.ColumnIndex == columnIndex && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 2 && s.ColumnIndex == columnIndex && s.ClaimedBy == TurnStatus) &&
            GameSpots.Any(s => s.RowIndex == rowIndex - 3 && s.ColumnIndex == columnIndex && s.ClaimedBy == TurnStatus))
        {
            GameStatus = $"{TurnStatus.Humanize(LetterCasing.Title)} Wins!";
            GameOver = true;
            return;
        }

        if (GameSpots.Any(s => s.RowIndex == rowIndex     && s.ColumnIndex == columnIndex     && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 1 && s.ColumnIndex == columnIndex - 1 && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 2 && s.ColumnIndex == columnIndex - 2 && s.ClaimedBy == TurnStatus) &&
            GameSpots.Any(s => s.RowIndex == rowIndex - 3 && s.ColumnIndex == columnIndex - 3 && s.ClaimedBy == TurnStatus))
        {
            GameStatus = $"{TurnStatus.Humanize(LetterCasing.Title)} Wins!";
            GameOver = true;
            return;
        }
        if (GameSpots.Any(s => s.RowIndex == rowIndex     && s.ColumnIndex == columnIndex     && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 1 && s.ColumnIndex == columnIndex + 1 && s.ClaimedBy == TurnStatus) && 
            GameSpots.Any(s => s.RowIndex == rowIndex - 2 && s.ColumnIndex == columnIndex + 2 && s.ClaimedBy == TurnStatus) &&
            GameSpots.Any(s => s.RowIndex == rowIndex - 3 && s.ColumnIndex == columnIndex + 3 && s.ClaimedBy == TurnStatus))
        {
            GameStatus = $"{TurnStatus.Humanize(LetterCasing.Title)} Wins!";
            GameOver = true;
            return;
        }

        await InvokeAsync(StateHasChanged);

    }

    protected async Task ResetGame()
    {
        GameSpots.Clear();
        GameColumns.Clear();
        GameButtons.Clear();
        TurnStatus = TurnStatus.PlayerOne;
        TurnCount = 1;
        GameStatus = string.Empty;
        GameOver = false;
        InitializeBoard();
        await InvokeAsync(StateHasChanged);
    }

    #endregion

    #region Setup

    private void InitializeBoard()
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
                    ClaimedBy = TurnStatus.Neutral,
                    Fill = TurnColors.Neutral
                };
                GameSpots.Add(newSpot);
                position++;
            }

            var newButton = new GameButton
            {
                ColumnIndex = column,
                Fill = TurnColors.PlayerOne
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
        InitializeBoard();
        await InvokeAsync(StateHasChanged);
    }

    #endregion
    
}