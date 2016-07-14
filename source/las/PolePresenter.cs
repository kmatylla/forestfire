//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace Automaton
{
    public sealed class CellPresenter : INotifyPropertyChanged
    {
        private int _x;
        private int _y;
        public double Scale {get;private set;}
        public int X { get{return System.Convert.ToInt32(_x*Scale);} }
        public int Y { get{return System.Convert.ToInt32(_y*Scale);} }
        private Cell _cell;
        private Color _color;
        public Color Color {
            get
            {
                return _color;
            }
            set 
            {
                if (value != CellToColorConverter.Convert(_cell.MyState))
                {
                    _cell.MyState = CellToColorConverter.Convert(value);
                    _color = value;
                    OnPropertyChanged("Color");
                }
            }
        }
       
        public void Update()
        {
            if (_color != CellToColorConverter.Convert(_cell.MyState))
            {
                _color = CellToColorConverter.Convert(_cell.MyState);
                OnPropertyChanged("Color");
            }
        }

        public void Rescale(double scale)
        {
            if (scale != Scale)
            {
                Scale = scale;
                OnPropertyChanged("Scale");
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public CellPresenter(int x, int y, double skala, Cell pole)
        {
            _x = x;
            _y = y;
            Scale = skala;
            _cell = pole;
            Color = CellToColorConverter.Convert(_cell.MyState);
            OnPropertyChanged("Color");
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
        }
    }
}
