using System;
using VContainer.Unity;

public class GameEndController : IInitializable, IDisposable
{
    private const string PopupTextWin = "You Won!";
    private const string PopupTextLose = "You lose...";

    private const string ButtonTextWin = "Go Again";
    private const string ButtonTextLose = "Try Again";

    private readonly GameEndPopup gameEndPopup;
    private readonly GameOverVisitor gameOverVisitor;
    private readonly GameEndSolver gameEndSolver;
    public Action RetryButtonPressed;

    public GameEndController(
        GameEndPopup gameEndPopup, 
        GameOverVisitor gameOverVisitor,
        GameEndSolver gameEndSolver) 
    {
        this.gameEndPopup = gameEndPopup;
        this.gameOverVisitor = gameOverVisitor;
        this.gameEndSolver = gameEndSolver;
    }

    public void GameWon()
    {
        gameEndPopup.ShowPopup();
        gameEndPopup.SetupButtonText(ButtonTextWin, PopupTextWin);
    }

    public void GameLose()
    {
        gameEndPopup.ShowPopup();
        gameEndPopup.SetupButtonText(ButtonTextLose, PopupTextLose);
    }

    public void Dispose()
    {
        gameOverVisitor.GameLostDetected -= OnGameOverDetected;
        gameEndSolver.GameWonDetected -= OnGameWinDetected;
        gameEndPopup.RetryButtonPressed -= OnRetryButtonPressed;
    }

    public void Initialize()
    {
        gameOverVisitor.GameLostDetected += OnGameOverDetected;
        gameEndSolver.GameWonDetected += OnGameWinDetected;
        gameEndPopup.RetryButtonPressed += OnRetryButtonPressed;
    }

    private void OnGameWinDetected()
    {
        GameWon();
    }

    private void OnRetryButtonPressed()
    {
        gameEndPopup.HidePopup();
        RetryButtonPressed?.Invoke();
    }

    private void OnGameOverDetected()
    {
        GameLose();
    }
}
