//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Automaton
{

    public partial class MainWindow : Window
    {
        private ForestModel _automatOb1;
        public AutomatonPresenter AutomatPresenterOb { get; set; }
        public ParametersPresenter ParamsPresenterOb { get; set; }
        private Thread _evolutionThread;
        private DispatcherTimer _refreshTimer;
        private Parameters _paramWindow;
        private StatWindow _statWindow;
        public StatystykiPresenter StatPresenterOb;
        private FileLogger _log;
        private TrianglePresenter _triangle;
        private TriangleWindow _tWindow;
        public int GraphUpdateInterval { get; set; }

        private void RefreshStep(object sender, EventArgs e)
        {
            AutomatPresenterOb.AfterCellsChanged();
            StatPresenterOb.Refresh();
            _tWindow.Refresh();
            _triangle.Refresh();
        }


        public MainWindow()
        {
            _automatOb1 = new ForestModel(50, 50, false, false, 0.05);
            AutomatPresenterOb = new AutomatonPresenter(_automatOb1);
            StatPresenterOb = new StatystykiPresenter (AutomatPresenterOb);
            _statWindow = new StatWindow(StatPresenterOb);
            _paramWindow = new Parameters(AutomatPresenterOb);
            _triangle = new TrianglePresenter(StatPresenterOb, AutomatPresenterOb);
            _tWindow = new TriangleWindow(_triangle);

            GraphUpdateInterval = 100;
            InitializeComponent();
          
            button2.Content="Start";
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromMilliseconds(250);
            _refreshTimer.Tick += RefreshStep;
            AutomatPresenterOb.AfterCellsChanged();

            _statWindow.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs a) => { StatMenuItem.IsChecked = _statWindow.IsVisible; };
            _paramWindow.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs a) => { ParamMenuItem.IsChecked = _paramWindow.IsVisible; };
            _tWindow.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs a) => { PhaseMenuItem.IsChecked = _tWindow.IsVisible; };
        }

        private void OnChangeModel(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
            if (_log != null) _log.Close();
        }

        private void EvolutionOneStep(object sender, RoutedEventArgs e)
        {
            AutomatPresenterOb.Step();
            StatPresenterOb.Update();
        }

        private void EvolutionLoop()
        {
            while (true) 
            {
                AutomatPresenterOb.Step();
                StatPresenterOb.Update();
            }
        }

        private void Stop()
        {
            _evolutionThread.Abort();
            _evolutionThread = null;
            _refreshTimer.Stop();
            button2.Content = "Start";
        }

        private void Start()
        {
            _evolutionThread = new Thread(EvolutionLoop);
            _evolutionThread.IsBackground = true;
            _evolutionThread.Start();
            _refreshTimer.Start();
            button2.Content = "Stop";
        }

        private void StartStop(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (_evolutionThread == null)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var dialog = new SaveFileDialog { FileName = "", DefaultExt = ".png", Filter = "Image files|*.png" };
            var result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }
            Cell.StateEnum[,] plansza= new Cell.StateEnum[_automatOb1.Cells.GetLength(0),_automatOb1.Cells.GetLength(1)];
            for (int i = 0; i < plansza.GetLength(0); ++i) for (int j = 0; j < plansza.GetLength(1); ++j) plansza[i, j] = _automatOb1.Cells[i, j].MyState;
            CellToBitmapConverter konwerter = new CellToBitmapConverter(plansza);
            konwerter.Bmp.Save(dialog.FileName);
        }

        private void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (_evolutionThread != null)
            {
                Stop();
            }
           AutomatPresenterOb.ExecuteOpen(sender, e);
        }

        private void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute=(_automatOb1 != null);
        }

        private void CanExecuteStop(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute=(_evolutionThread!=null);
        }

        private void CanExecuteStart(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_evolutionThread == null);
        }

        private void ExecuteStart(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Start();
        }

      
        private void ExecuteStop(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Stop();
        }

        private void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (_evolutionThread != null)
            {
                Stop();
            }
            AutomatPresenterOb.ExecuteNew(sender,e);
        }

        private void ShowStats(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _statWindow.Show();
        }

        private void ShowParams(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _paramWindow.Show();
        }

        private void HideStats(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _statWindow.Hide();
        }

        private void HideParams(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _paramWindow.Hide();
        }

        private void ShowPhase(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _tWindow.Show();
        }

        private void HidePhase(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _tWindow.Hide();
        }

        private void CanShowStats(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_statWindow != null && _statWindow.IsVisible == false);
        }

        private void CanHideStats(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_statWindow != null && _statWindow.IsVisible == true);
        }
        private void CanShowParams(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_paramWindow != null && _paramWindow.IsVisible == false);
        }
        private void CanHideParams(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_paramWindow != null && _paramWindow.IsVisible == true);
        }

        private void CanShowPhase(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_tWindow != null && _tWindow.IsVisible == false);
        }
        private void CanHidePhase(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = (_tWindow != null && _tWindow.IsVisible == true);
        }

        private void StartLog(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var d = new SaveFileDialog { Filter = "Pliki tekstowe|*.txt" };
            var result = d.ShowDialog();
            if (result != true)
            {
                return;
            }
            _log = new FileLogger(AutomatPresenterOb, d.FileName);
        }

        private void CanStartLog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
           e.CanExecute= (_log==null);
        }

        private void StopLog(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (_log != null)
            {
                _log.Close();
                _log = null;
            }
        }

        private void CanStopLog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute= (_log != null);
        }

        public static RoutedCommand SaveCommand = new RoutedCommand("Save",
          typeof(MainWindow),
          new InputGestureCollection(new[] { new KeyGesture(Key.S, ModifierKeys.Control) }));

        public static RoutedCommand OpenCommand = new RoutedCommand("Open",
         typeof(MainWindow),
         new InputGestureCollection(new[] { new KeyGesture(Key.O, ModifierKeys.Control) }));

        public static RoutedCommand NewCommand = new RoutedCommand("New",
         typeof(MainWindow),
        new InputGestureCollection(new[] { new KeyGesture(Key.N, ModifierKeys.Control) }));

        public static RoutedCommand StartCommand = new RoutedCommand("Start",
       typeof(MainWindow),
       new InputGestureCollection(new[] { new KeyGesture(Key.R, ModifierKeys.Control) }));

        public static RoutedCommand StopCommand = new RoutedCommand("Stop",
      typeof(MainWindow),
      new InputGestureCollection(new[] { new KeyGesture(Key.P, ModifierKeys.Control) }));

        public static RoutedCommand ShowStatsCommand = new RoutedCommand("Show statistics",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand HideStatsCommand = new RoutedCommand("Hide statistics",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand ShowParamsCommand = new RoutedCommand("Show parameters",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand HideParamsCommand = new RoutedCommand("Hide parameters",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand ShowPhaseCommand = new RoutedCommand("Show ?",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand HidePhaseCommand = new RoutedCommand("Hide ?",
typeof(MainWindow),
new InputGestureCollection());

        public static RoutedCommand StartLogCommand = new RoutedCommand("Start logging to file",
         typeof(MainWindow),
         new InputGestureCollection(new[] { new KeyGesture(Key.L, ModifierKeys.Control) }));

        public static RoutedCommand StopLogCommand = new RoutedCommand("Stop logging",
         typeof(MainWindow),
         new InputGestureCollection(new[] { new KeyGesture(Key.L, ModifierKeys.Control) }));

        private void Mouse(object sender, MouseButtonEventArgs e)
        {
            bool b=false;
            if (_evolutionThread != null)
            {
                Stop();
                b = true;
            }
            if(e.ChangedButton==MouseButton.Left) 
                ((CellPresenter)(((Rectangle)sender).Tag)).Color = CellToColorConverter.LeftColor;
            if (e.ChangedButton == MouseButton.Right)
                ((CellPresenter)(((Rectangle)sender).Tag)).Color = CellToColorConverter.RightColor;
            if (e.ChangedButton == MouseButton.Middle)
                ((CellPresenter)(((Rectangle)sender).Tag)).Color = CellToColorConverter.MiddleColor;
            AutomatPresenterOb.AfterCellsChanged();
            if (b) Start();
        }

        private void QuitApp(object sender, EventArgs e)
        {
            if (_paramWindow != null)
            {
                _paramWindow.CanClose = true;
                _paramWindow.Close();
            }
            if (_statWindow != null)
            {
                _statWindow.CanClose = true;
                _statWindow.Close();
            }
            if (_tWindow != null)
            {
                _tWindow.CanClose = true;
                _tWindow.Close();
            }
            if (_log != null) _log.Close();
        }

        public bool StatWindowExists { get { return (_statWindow != null); } }

        private void ToggleStatWindowOn(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
          _statWindow.Show();
        }

        private void ToggleStatWindowOff(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _statWindow.Hide();
        }

        public bool PhaseWindowExists { get { return (_tWindow != null); } }

        private void TogglePhaseWindowOn(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _tWindow.Show();
        }

        private void TogglePhaseWindowOff(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _tWindow.Hide();
        }

        public bool ParamWindowExists { get { return (_paramWindow != null); } }

        private void ToggleParamWindowOn(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _paramWindow.Show();
        }

        private void ToggleParamWindowOff(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _paramWindow.Hide();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            var a=new About();
            a.Show();
        }

    }
}