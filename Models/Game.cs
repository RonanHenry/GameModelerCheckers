using System;
using System.Collections.ObjectModel;
using Map.Models;

namespace Checkers.Models
{
    /// <summary>
    /// Define a game
    /// </summary>
    [Serializable]
    public class Game : BaseModel
    {
        #region Attributes

        private string _name;

        private ObservableCollection<Player> _players;

        private ObservableCollection<Piece> _pieces;

        #endregion

        #region Properties

        /// <summary>
        /// Game's name
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Game's players
        /// </summary>
        public ObservableCollection<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Game's pieces
        /// </summary>
        public ObservableCollection<Piece> Pieces
        {
            get => _pieces;
            set
            {
                _pieces = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Name of the game</param>
        public Game(string name)
        {
            Name = name;
            Players = new ObservableCollection<Player>();
            Pieces = new ObservableCollection<Piece>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// String describing the gane (Debug)
        /// </summary>
        /// <returns>String description of the game</returns>
        public override string ToString()
        {
            return "[Game] " +
                   $"Name({Name}) " +
                   $"Players({Players}) " +
                   $"Pieces({Pieces})";
        }

        #endregion
    }
}
