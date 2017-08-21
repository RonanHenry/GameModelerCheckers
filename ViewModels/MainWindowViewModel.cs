using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Checkers.Enums;
using Checkers.Models;
using DataBase.Database.DbContexts.Interfaces;
using DataBase.Database.DbSettings;
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

        /// <summary>
        /// MySQL server
        /// </summary>
        private const string Server = "localhost";

        /// <summary>
        /// MySQL user
        /// </summary>
        private const string User = "gamemodeler";

        /// <summary>
        /// MySQL password
        /// </summary>
        private const string Password = "p@ssword";

        /// <summary>
        /// MySQL database name
        /// </summary>
        private const string Database = "game_modeler";

        #endregion

        #region Properties

        /// <summary>
        /// Entity Framework context
        /// </summary>
        public IUniversalContext Context { get; set; }

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
        /// Command to open a saved game from a database
        /// </summary>
        public ICommand OpenDbCommand { get; set; }

        /// <summary>
        /// Command to save the game in a database
        /// </summary>
        public ICommand SaveDbCommand { get; set; }

        /// <summary>
        /// Command to save changes
        /// </summary>
        public ICommand SaveCommand { get; set; }

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
            var mySqlDbSettings = DatabaseFactory
                .MySqlDb
                .Set
                .DatabaseName(Database)
                .Server(Server)
                .UserId(User)
                .Password(Password)
                .ToMySqlDatabase;

            Context = DatabaseFactory.CreateContext(mySqlDbSettings);

            NewGameCommand = new RelayCommand(NewGame, NewGameCanExecute);
            OpenDbCommand = new RelayCommand(OpenDb, null);
            SaveDbCommand = new RelayCommand(SaveDb, null);
            SaveCommand = new RelayCommand(Save, SaveCanExecute);
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
            Game.Players.Clear();
            Game.Pieces.Clear();

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

        /// <summary>
        /// Triggered when a cell gets left clicked
        /// Move the selected piece to the new position if the movement is allowed
        /// </summary>
        /// <param name="sender">Object calling the method</param>
        /// <param name="e">Mouse event arguments</param>
        public override void OnCellMouseLeftPressed(object sender, MouseEventArgs e)
        {
            base.OnCellMouseLeftPressed(sender, e);

            // If the position im focus is allowed
            if (IsLegalMove(PositionInFocus))
            {
                var piece = (Piece) SelectedItem;
                var oldPosition = piece.Position;
                var movement = PositionInFocus - oldPosition;
                Point? capturedPiecePosition = null;

                if (movement == new Vector(-2, -2))
                {
                    capturedPiecePosition = oldPosition + new Vector(-1, -1);
                }
                else if (movement == new Vector(2, -2))
                {
                    capturedPiecePosition = oldPosition + new Vector(1, -1);
                }
                else if (movement == new Vector(-2, 2))
                {
                    capturedPiecePosition = oldPosition + new Vector(-1, 1);
                }
                else if (movement == new Vector(2, 2))
                {
                    capturedPiecePosition = oldPosition + new Vector(1, 1);
                }

                ValidPositions.Clear();
                ValidatePositions();

                // Move the piece to the destination
                piece.Position = PositionInFocus;

                // If the piece has moved the first or last row depending on the active player
                if (piece.Player.Side == Side.Top && piece.Position.Y.Equals(8) ||
                    piece.Player.Side == Side.Bottom && piece.Position.Y.Equals(1))
                {
                    // Make it a king
                    piece.IsKing = true;
                }

                if (capturedPiecePosition != null)
                {
                    var otherPlayer = Game.Players.First(p => !p.IsActive);

                    Game.Pieces.Remove(Game.Pieces.First(p => p.Position == capturedPiecePosition));
                    otherPlayer.PiecesCount--;

                    if (otherPlayer.PiecesCount < 1)
                    {
                        var player = Game.Players.First(p => p.IsActive);
                        MessageBox.Show($"{player.Username} won the game with {player.PiecesCount} remaining pieces",
                            "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        player.IsActive = false;
                        return;
                    }

                    PlayTurn(true);
                }
                else
                {
                    ChangeActivePlayer();
                }
            }
        }

        public override void OnUserControlMouseLeftPressed(object sender, MouseEventArgs e)
        {
            // Base view model sets the SelectedItem
            base.OnUserControlMouseLeftPressed(sender, e);

            PlayTurn();
        }

        public void PlayTurn(bool afterCapture = false)
        {
            // Cast item to a piece and clear valid positions
            var piece = SelectedItem as Piece;

            ValidPositions.Clear();
            ValidatePositions();

            // If item is not a piece or piece's owner is not the active player
            if (piece == null || !piece.Player.IsActive)
            {
                return;
            }

            // If piece is king (move in any direction)
            if (piece.IsKing)
            {
                ValidPositions.AddRange(new[]
                {
                    piece.Position + new Vector(-1, -1),
                    piece.Position + new Vector(1, -1),
                    piece.Position + new Vector(-1, 1),
                    piece.Position + new Vector(1, 1)
                });
            }
            else
            {
                // If active player is the top one (Move down)
                if (piece.Player.Side == Side.Top)
                {
                    ValidPositions.AddRange(new[]
                    {
                        piece.Position + new Vector(-1, 1),
                        piece.Position + new Vector(1, 1)
                    });
                }

                // If active player is the bottom one (Move up)
                else
                {
                    ValidPositions.AddRange(new[]
                    {
                        piece.Position + new Vector(-1, -1),
                        piece.Position + new Vector(1, -1)
                    });
                }
            }

            // For each position, verify if it is available and if a capture can happen
            for (var i = 0; i < ValidPositions.Count; i++)
            {
                // Get data in the destination cell
                var dataInPosition = GetLayerData(ValidPositions[i]);

                // If a piece exists
                if (dataInPosition != null)
                {
                    var existingPiece = (Piece)dataInPosition;

                    // If the piece belongs to the other player
                    if (!existingPiece.Player.IsActive)
                    {
                        var position = ValidPositions[i] - piece.Position;
                        Point newPosition;

                        if (position == new Vector(-1, 1))
                        {
                            newPosition = ValidPositions[i] + new Vector(-1, 1);
                        }
                        else if (position == new Vector(1, 1))
                        {
                            newPosition = ValidPositions[i] + new Vector(1, 1);
                        }
                        else if (position == new Vector(-1, -1))
                        {
                            newPosition = ValidPositions[i] + new Vector(-1, -1);
                        }
                        else
                        {
                            newPosition = ValidPositions[i] + new Vector(1, -1);
                        }

                        if (GetLayerData(newPosition) == null)
                        {
                            ValidPositions.Add(newPosition);
                        }
                    }
                }
            }

            if (afterCapture)
            {
                ValidPositions.RemoveRange(0, piece.IsKing ? 4 : 2);
            }

            ValidatePositions();

            if (afterCapture && ValidPositions.Count == 0)
            {
                ChangeActivePlayer();
            }
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
        /// Open a saved game from a database
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public async void OpenDb(object parameter)
        {
            var param = parameter as string;
            int id;

            if (param == null || !int.TryParse(param, out id))
            {
                return;
            }

            Game = await Context.Entity<Game>().GetAsync(id);

            foreach (var player in Game.Players)
            {
                player.Color = (Brush)player.BrushConverter.ConvertFromString(player.ColorHex);
                player.AccentColor = (Brush)player.BrushConverter.ConvertFromString(player.AccentColorHex);
            }

            foreach (var piece in Game.Pieces)
            {
                piece.Color = (Brush)piece.BrushConverter.ConvertFromString(piece.ColorHex);
                piece.AccentColor = (Brush)piece.BrushConverter.ConvertFromString(piece.AccentColorHex);
                // piece.KingColor = (Brush)piece.BrushConverter.ConvertFromString(piece.KingColorHex);

                piece.Position = new Point(piece.X, piece.Y);
            }
        }

        /// <summary>
        /// Save the game in a database
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public async void SaveDb(object parameter)
        {
            await Context.Entity<Game>().InsertAsync(Game);
        }

        /// <summary>
        /// Decide if the game can be updated
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        /// <returns></returns>
        public bool SaveCanExecute(object parameter)
        {
            return Game != null;
        }

        /// <summary>
        /// Save the changes of a game
        /// </summary>
        /// <param name="parameter">Parameter sent by the caller</param>
        public async void Save(object parameter)
        {
            await Context.Entity<Game>().UpdateAsync(Game);
        }

        /// <summary>
        /// Decide if the application can be terminated
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
