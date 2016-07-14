//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Input;

namespace Automaton
{
    public sealed class AutomatonPresenter : INotifyPropertyChanged
    {

        private ObservableCollection<CellPresenter> _cells=new ObservableCollection<CellPresenter>();
        public ObservableCollection<CellPresenter> Cells { get {return _cells; }}
        private ForestModel _automaton; 
        private double _scale;
        public double Scale 
        {
            get { return _scale; }
            set 
            {
                if (value != _scale)
                {
                    _scale = value;
                    OnPropertyChanged("Scale");
                    OnPropertyChanged("SizeXScaled");
                    OnPropertyChanged("SizeYScaled");
                    foreach (CellPresenter pole in _cells)
                    {
                        pole.Rescale(_scale);
                    }
  //                  OnPropertyChanged("Cells");
                }
            }
        } 
      

        public int SizeX
        {
            get { return _automaton.SizeX; }
        }
        public int SizeY
        {
            get { return _automaton.SizeY; }
        }

        public double SizeXScaled
        {
            get { return _automaton.SizeX*_scale; }
        }
        public double SizeYScaled
        {
            get { return _automaton.SizeY*_scale; }
        }
        /// <summary>
        /// ilość wykonanych kroków
        /// </summary>
        public int StepCount
        {
            get { return _automaton.Steps; }
        }
        public bool WrapX
        {
            get { return _automaton.WrapX; }
            set { _automaton.WrapX = value; }//onpropchanged dodac
        }
        public bool WrapY
        {
            get { return _automaton.WrapY; }
            set { _automaton.WrapY = value; }
        }

        public EventHandler<StepData> StepEvent;

        void OnNewStep(object sender, StepData s)
        {
            if (StepEvent != null) StepEvent(sender,s);
        }

        public int ParamTreesRadius
        {
            get { return _automaton.ParamRTrees; }
            set { _automaton.ParamRTrees = value; }
        }

        public double[] Params { get { return _automaton.Parameters; } }
        public string[] ParamNames { get { return _automaton.ParamNames; } }
       /*---------------EVENT------------------------------------*/


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public AutomatonPresenter(int x, int y, bool zx, bool zy, double gestosc)
        {
            Scale = 2;

            _automaton=new ForestModel (x, y, zx, zy, gestosc);
            for (int i = 0; i < _automaton.SizeX; ++i)
                for (int j = 0; j < _automaton.SizeY; ++j)
                    _cells.Add(new CellPresenter(i,j,Scale,_automaton.Cells[i,j]));

        }

        public AutomatonPresenter() : this(64, 64, true, true, 0.003) { }

        public AutomatonPresenter(ForestModel a)
        {
            Scale = 2;
            _automaton = a;
            for (int i = 0; i < _automaton.SizeX; ++i)
                for (int j = 0; j < _automaton.SizeY; ++j)
                    _cells.Add(new CellPresenter(i, j, Scale, _automaton.Cells[i, j]));
            _automaton.StepEvent += OnNewStep;
        }
    
        public void Step()
        {
            _automaton.DoStep();
            AfterCellsChanged();
        }

        public void AfterCellsChanged()
        {
            foreach (var p in _cells) p.Update();
            OnPropertyChanged("StepCount");
            OnPropertyChanged("Stats");
        }

        public Dictionary<Cell.StateEnum,int> Stats
        {
            get
            {
                return _automaton.Stats;
            }
        }

        public void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var dialog = new OpenFileDialog { Filter = "Image files|*.png" };
            var result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }
            var bmp = new System.Drawing.Bitmap(dialog.FileName);
            CellToBitmapConverter konwerter = new CellToBitmapConverter(bmp);
            _automaton = new ForestModel(konwerter.Cells, _automaton.WrapX, _automaton.WrapY);
            OnLoad();
        }

        private void OnLoad()
        {
            _cells.Clear();
            for (int i = 0; i < _automaton.SizeX; ++i)
                for (int j = 0; j < _automaton.SizeY; ++j)
                    _cells.Add(new CellPresenter(i, j, Scale, _automaton.Cells[i, j]));
            foreach (var p in _cells) p.Update();
            _automaton.StepEvent += OnNewStep;
            OnPropertyChanged("Cells");
            OnPropertyChanged("StepCount");
            OnPropertyChanged("SizeX");
            OnPropertyChanged("SizeY");
            OnPropertyChanged("SizeXScaled");
            OnPropertyChanged("SizeYScaled");
            OnPropertyChanged("Params");
            OnPropertyChanged("Stats");
            if (ModelChanged != null) ModelChanged(this,new EventArgs() );
        }

        public void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            NewAutomaton n = new NewAutomaton(this);
            n.Show();
        }

        public event EventHandler ModelChanged;

        public void MakeNew(int x, int y, bool zx, bool zy, double gestosc)
        {
            _cells.Clear();
            _automaton = new ForestModel(x, y, zx, zy, gestosc);
            OnLoad();
        }

    }
}

