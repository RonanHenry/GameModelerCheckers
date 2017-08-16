using System.Windows;
using System.Windows.Data;
using Checkers.UserControls;
using Checkers.ViewModels;
using Map.UserControls;

namespace Checkers.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Pass the grid layer to the library
            var vm = (MainWindowViewModel) DataContext;
            vm.GridLayer = GameBoard;

            // For each piece
            for (var i = 0; i < 24; i++)
            {
                // Create the piece's data context binding
                var pieceDataContextBinding = new Binding
                {
                    Path = new PropertyPath($"DataContext.Game.Pieces[{i}]"),
                    RelativeSource = new RelativeSource
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(Window)
                    },
                    Mode = BindingMode.TwoWay
                };

                // Create the piece's position binding
                var piecePositionBinding = new Binding
                {
                    Path = new PropertyPath($"DataContext.Game.Pieces[{i}].Position"),
                    RelativeSource = new RelativeSource
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(Window)
                    },
                    Mode = BindingMode.TwoWay
                };

                // Create a piece user control
                var pieceUserControl = new PieceUserControl();

                // Add the piece user control to the grid layer
                GameBoard.Children.Add(pieceUserControl);

                // Apply the bindings to the piece user control
                BindingOperations.SetBinding(pieceUserControl, DataContextProperty, pieceDataContextBinding);
                BindingOperations.SetBinding(pieceUserControl, BaseUserControl.PositionProperty, piecePositionBinding);
            }

            vm.Initialize();
        }
    }
}
