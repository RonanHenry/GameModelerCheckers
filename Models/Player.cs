﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using Checkers.Enums;
using DataBase.Database.Utils;
using Map.Interfaces;
using Map.Models;

namespace Checkers.Models
{
    /// <summary>
    /// Define a player
    /// </summary>
    [Serializable]
    [Persistent]
    [Table("Players")]
    public class Player : BaseModel, IModel
    {
        #region Attributes

        private string _username;

        private Brush _color;

        private Brush _accentColor;

        private bool _isActive;

        private int _piecesCount;

        private Side _side;

        #endregion

        #region Properties

        /// <summary>
        /// Player's id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Player's username
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Player's color
        /// </summary>
        [NotMapped]
        public Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                ColorHex = BrushConverter.ConvertToString(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Color as an hexa string value
        /// </summary>
        public string ColorHex { get; set; }

        /// <summary>
        /// player's accent color
        /// </summary>
        [NotMapped]
        public Brush AccentColor
        {
            get => _accentColor;
            set
            {
                _accentColor = value;
                AccentColorHex = BrushConverter.ConvertToString(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Accent color as an hexa string value
        /// </summary>
        public string AccentColorHex { get; set; }

        /// <summary>
        /// Player's state
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Player's pieces count
        /// </summary>
        public int PiecesCount
        {
            get => _piecesCount;
            set
            {
                _piecesCount = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Player's side
        /// </summary>
        public Side Side
        {
            get => _side;
            set
            {
                _side = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Player()
        {
            IsActive = false;
            PiecesCount = 12;
        }

        /// <summary>
        /// Construct a player with a username, a color and an accent color
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <param name="color">Color of the player</param>
        /// <param name="accentColor">Accent color of the player</param>
        /// <param name="side">Side of the player</param>
        public Player(string username, Brush color, Brush accentColor, Side side) 
            : this()
        {
            Username = username;
            Color = color;
            AccentColor = accentColor;
            Side = side;
        }

        #endregion

        #region Methods

        /// <summary>
        /// String describing the player (Debug)
        /// </summary>
        /// <returns>String description of the player</returns>
        public override string ToString()
        {
            return "[Player] " +
                   $"Username({Username}) " +
                   $"Color({Color}) " +
                   $"AccentColor({AccentColor}) " +
                   $"IsActive({IsActive})";
        }

        #endregion
    }
}
