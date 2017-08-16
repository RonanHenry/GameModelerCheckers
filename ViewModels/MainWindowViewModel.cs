using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Checkers.Enums;
using Checkers.Models;
using Map.Commands;
using Map.ViewModels;

namespace Checkers.ViewModels
{
    /// <summary>
    /// Checkers game logic
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Attributes

        private Game _game;

        #endregion

        #region Properties

        /// <summary>
        /// Game data
        /// </summary>
        public Game Game
        {
            get => _game;
            set
            {
                _game = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to start a new game
        /// </summary>
        public ICommand NewGameCommand { get; set; }

        /// <summary>
        /// Command to show the game's credits
        /// </summary>
        public ICommand AboutCommand { get; set; }

        /// <summary>
        /// Command to exit the application
        /// </summary>
        public ICommand ExitCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            NewGameCommand = new RelayCommand(NewGame, NewGameCanExecute);
            ExitCommand = new RelayCommand(Exit, ExitCanExecute);
            AboutCommand = new RelayCommand(About, null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Decide if a new game can be started
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        /// <returns>True if a new game can be started, false otherwise</returns>
        public bool NewGameCanExecute(object parameter)
        {
            // Todo: Ask to save the game before starting a new game

            return true;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public void NewGame(object parameter)
        {
            Game = new Game("Game 1");

            Game.Players.Add(new Player("Player 1", Brushes.Silver, Brushes.Gray, Side.Top));
            Game.Players.Add(new Player("Player 2", Brushes.Gold, Brushes.DarkOrange, Side.Bottom));

            // For each player
            foreach (var player in Game.Players)
            {
                // Create each piece
                for (var i = 0; i < 12; i++)
                {
                    var piece = new Piece(70, 70, player.Color, player.AccentColor, player);
                    
                    Game.Pieces.Add(piece);

                    // Piece's initial position
                    var position = default(Point);

                    // Player on the top side
                    if (player.Side == Side.Top)
                    {
                        switch (i)
                        {
                            case 0:
                                position = new Point(2, 1);
                                break;
                            case 1:
                                position = new Point(4, 1);
                                break;
                            case 2:
                                position = new Point(6, 1);
                                break;
                            case 3:
                                position = new Point(8, 1);
                                break;
                            case 4:
                                position = new Point(1, 2);
                                break;
                            case 5:
                                position = new Point(3, 2);
                                break;
                            case 6:
                                position = new Point(5, 2);
                                break;
                            case 7:
                                position = new Point(7, 2);
                                break;
                            case 8:
                                position = new Point(2, 3);
                                break;
                            case 9:
                                position = new Point(4, 3);
                                break;
                            case 10:
                                position = new Point(6, 3);
                                break;
                            case 11:
                                position = new Point(8, 3);
                                break;
                        }
                    }

                    // Second player
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                position = new Point(1, 6);
                                break;
                            case 1:
                                position = new Point(3, 6);
                                break;
                            case 2:
                                position = new Point(5, 6);
                                break;
                            case 3:
                                position = new Point(7, 6);
                                break;
                            case 4:
                                position = new Point(2, 7);
                                break;
                            case 5:
                                position = new Point(4, 7);
                                break;
                            case 6:
                                position = new Point(6, 7);
                                break;
                            case 7:
                                position = new Point(8, 7);
                                break;
                            case 8:
                                position = new Point(1, 8);
                                break;
                            case 9:
                                position = new Point(3, 8);
                                break;
                            case 10:
                                position = new Point(5, 8);
                                break;
                            case 11:
                                position = new Point(7, 8);
                                break;
                        }
                    }

                    piece.Position = position;
                }
            }

            // Set the top sided player as the active one for the first turn
            Game.Players.First(p => p.Side == Side.Top).IsActive = true;
        }

        public override void OnCellMouseLeftPressed(object sender, MouseEventArgs e)
        {
            base.OnCellMouseLeftPressed(sender, e);

            if (IsLegalMove(PositionInFocus))
            {
                ((Piece) SelectedItem).Position = PositionInFocus;
            }
        }

        public override void OnCellMouseLeftReleased(object sender, MouseEventArgs e)
        {
            base.OnCellMouseLeftReleased(sender, e);
        }

        public override void OnCellMouseRightPressed(object sender, MouseEventArgs e)
        {
            base.OnCellMouseRightPressed(sender, e);
        }

        public override void OnCellMouseRightReleased(object sender, MouseEventArgs e)
        {
            base.OnCellMouseRightReleased(sender, e);
        }

        public override void OnUserControlMouseLeftPressed(object sender, MouseEventArgs e)
        {
            base.OnUserControlMouseLeftPressed(sender, e);

            var piece = SelectedItem as Piece;
            ValidPositions.Clear();

            if (piece == null || !piece.Player.IsActive)
            {
                return;
            }

            if (piece.Player.Side == Side.Top)
            {
                ValidPositions.AddRange(new []
                {
                    piece.Position + new Vector(-1, 1), 
                    piece.Position + new Vector(1, 1), 
                });

                ValidatePositions();
            }
        }

        public override void OnUserControlMouseLeftReleased(object sender, MouseEventArgs e)
        {
            base.OnUserControlMouseLeftReleased(sender, e);
        }

        public override void OnUserControlMouseRightPressed(object sender, MouseEventArgs e)
        {
            base.OnUserControlMouseRightPressed(sender, e);
        }

        public override void OnUserControlMouseRightReleased(object sender, MouseEventArgs e)
        {
            base.OnUserControlMouseRightReleased(sender, e);
        }

        /// <summary>
        /// Change the active player
        /// </summary>
        public void ChangeActivePlayer()
        {
            foreach (var player in Game.Players)
            {
                player.IsActive ^= true;
            }
        }

        /// <summary>
        /// Decide if the application can be termiated
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        /// <returns>True if the application can be terminated, false otherwise</returns>
        public bool ExitCanExecute(object parameter)
        {
            // Todo: Ask to save the game before exiting

            return true;
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public void Exit(object parameter)
        {
            (parameter as Window)?.Close();
        }

        /// <summary>
        /// Show the game's credits
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public void About(object parameter)
        {
            const string title = "About Checkers";
            const string message = "WPF demo game powered by Game Modeler Corporation";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
