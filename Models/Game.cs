using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataBase.Database.Utils;
using Map.Interfaces;
using Map.Models;

namespace Checkers.Models
{
    /// <summary>
    /// Define a game
    /// </summary>
    [Serializable]
    [Persistent]
    [Table("Games")]
    public class Game : BaseModel, IModel
    {
        #region Attributes

        private string _name;

        private ObservableCollection<Player> _players;

        private ObservableCollection<Piece> _pieces;

        #endregion

        #region Properties

        /// <summary>
        /// Game's id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public virtual ObservableCollection<Player> Players
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
        public virtual ObservableCollection<Piece> Pieces
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
        public Game() {}

        /// <summary>
        /// Construct a game with a given name
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
