using System.Windows;
using System.Windows.Media;
using Map.Interfaces;
using Map.Models;

namespace Checkers.Models
{
    public class Piece : BaseModel, IMovable
    {
        #region Attributes

        private double _width;

        private double _height;

        private Brush _color;

        private Brush _accentColor;

        private Brush _kingColor;

        private bool _isKing;

        private Point _position;

        private Player _player;

        #endregion

        #region Properties

        /// <summary>
        /// Piece's width
        /// </summary>
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's height
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's color
        /// </summary>
        public Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's accent color
        /// </summary>
        public Brush AccentColor
        {
            get => _accentColor;
            set
            {
                _accentColor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's king color
        /// </summary>
        public Brush KingColor
        {
            get => _kingColor;
            set
            {
                _kingColor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's state
        /// </summary>
        public bool IsKing
        {
            get => _isKing;
            set
            {
                _isKing = value;
                KingColor = AccentColor;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's position
        /// </summary>
        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Piece's owner
        /// </summary>
        public Player Player
        {
            get => _player;
            set
            {
                _player = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Piece()
        {
            _isKing = false;
            KingColor = Brushes.Transparent;
        }

        /// <summary>
        /// Construct a piece with a width, a height, a color, an accent color, allowed directions and an owner
        /// </summary>
        /// <param name="width">Width of the piece</param>
        /// <param name="height">Height of the piece</param>
        /// <param name="color">Color of the piece</param>
        /// <param name="accentColor">Accent color of the piece</param>
        /// <param name="player">Owner of the piece</param>
        public Piece(double width, double height, Brush color, Brush accentColor, Player player)
            : this()
        {
            Width = width;
            Height = height;
            Color = color;
            AccentColor = accentColor;
            Player = player;
        }

        #endregion

        #region Methods

        /// <summary>
        /// String describing the piece (Debug)
        /// </summary>
        /// <returns>String description of the piece</returns>
        public override string ToString()
        {
            return "[Piece] " +
                   $"Width({Width}) " +
                   $"Height({Height}) " +
                   $"Color({Color}) " +
                   $"AccentColor({AccentColor}) " +
                   $"IsKing({IsKing}) " +
                   $"Position({Position}) " +
                   $"Player({Player})";
        }

        #endregion
    }
}
