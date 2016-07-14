//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace Automaton
{
    public class StatPresenter : INotifyPropertyChanged
    {
        private StatModel _stat;
        private AutomatonPresenter _ap;
        private List<int> _valueBuffer;
        public int Height;
        private int _count=1;
        public int UpdatePeriod{get; private set;}
        public Brush StateColor
        {
            get
            {
                return new SolidColorBrush(CellToColorConverter.Convert(_stat.State));
            }
        }
        public List<GraphPoint> GraphBuffer 
        {
            get
            {
                var b = new List<GraphPoint>();
                Brush c = StateColor;
                int tmp = 0;
                lock(_valueBuffer)
                {
                    while (_valueBuffer.Count > MaxBuffer) _valueBuffer.RemoveAt(0);
                    foreach (var x in _valueBuffer)
                    {
                        int i = (int)((double)Height * _stat.PrzeskalowanaMax(x));
                        b.Add(new GraphPoint(c, i,tmp));
                        tmp = i;
                    }
                }
                return b;
            }
        }
        public int MaxBuffer {get;set;}
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public void update()
        {
            lock (_valueBuffer)
            {
                if(_count%UpdatePeriod==0)
                {
                    _valueBuffer.Add(_stat.Value);
                    if (_valueBuffer.Count > MaxBuffer) _valueBuffer.RemoveAt(0);
                }
                _count ++;
            }
        }

        public void refresh()
        {
            OnPropertyChanged("Value");
            OnPropertyChanged("GraphBuffer");
        }

        public double Value { get { return _stat.Normalised(); } }

        public StatPresenter(StatModel s, AutomatonPresenter ap, int m=300, int h=100)
        {
            MaxBuffer = m;
            _stat = s;
            _ap = ap;
            _valueBuffer = new List<int>();
            Height = h;
            UpdatePeriod = 100;
        }

    }
}
