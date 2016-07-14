//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Automaton
{
    public class TrianglePresenter : INotifyPropertyChanged
    {
        private StatystykiPresenter _stats;
        private Cell.StateEnum[] _circle=new Cell.StateEnum[7]{
        Cell.StateEnum.Soil,
        Cell.StateEnum.YoungTree,
        Cell.StateEnum.MatureTree,
        Cell.StateEnum.OldTree,
        Cell.StateEnum.DeadTree,
        Cell.StateEnum.Fire,
        Cell.StateEnum.RichSoil
        };

        public int MaxBuffer;
        private AutomatonPresenter _ap;

        public List<TrianglePoint> Buffer
        {
            get 
            {
                lock (_buffer)
                {
                    //return _buffer;
                    return _buffer.Select(t => new TrianglePoint(t)).ToList();
                }
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        private List<TrianglePoint> _buffer;
     
        /// <summary>
        /// (-1,1)
        /// </summary>
        public double X
        {
            get
            {
                var n = _circle.Select(x => (double)(_stats.StatsD[x].Value));
                double sum = n.Sum();
                if (sum == 0) return 0;
                n = n.Select(x=>x/sum);
                return n.Select((x, i) => x * Math.Sin(2*i * Math.PI / n.Count())).Sum();
            }
        }
        /// <summary>
        /// (-1,1)
        /// </summary>
        public double Y
        {
            get
            {
                var n = _circle.Select(x => (double)(_stats.StatsD[x].Value));
                double sum = n.Sum();
                if (sum == 0) return 0;
                n = n.Select(x => x / sum);
                return n.Select((x, i) => x * Math.Cos(2 * i * Math.PI / n.Count())).Sum();
            }
        }

        public void Update(object sender, StepData s)
        {
            Update();
        }


        public void Update()
        {
            lock (_buffer)
            {
                foreach (var p in _buffer) p.Step();
                _buffer.Add(new TrianglePoint(X, Y, _x2, _y2, PSize, PColor, this));
                if (MaxBuffer>=0) while (_buffer.Count > MaxBuffer) _buffer.RemoveAt(0);
                _x2 = X;
                _y2 = Y;
            }
        }

        public void Refresh()
        {
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
            OnPropertyChanged("Buffer");
            OnPropertyChanged("PColor");
            OnPropertyChanged("PSize");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private double _y2;
        private double _x2;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public TrianglePresenter(StatystykiPresenter s, AutomatonPresenter ap, int m=2500)
        {
            _buffer = new List<TrianglePoint>();
            _stats = s;
            MaxBuffer = m;
            _ap = ap;
            _ap.StepEvent += Update;
            Width = 300;
            Height=300;
            _x2 = 0;
            _y2 = 0;
            _ap.ModelChanged += OnNewAutomat;
        }

        private void OnNewAutomat(object s, EventArgs e)
        {
            _buffer.Clear();
            _x2 = 0;
            _y2 = 0;
            OnPropertyChanged("Buffer");

        }

        public System.Windows.Media.Color PColor { 
            get 
            {
                var c = new System.Windows.Media.Color();
                c.A = 255;
                c.R = (byte)(_circle.Select(x => (double)(CellToColorConverter.Convert(x)).R * (double)(_stats.StatsD[x].Scaled())).Sum());
                c.G = (byte)(_circle.Select(x => (double)(CellToColorConverter.Convert(x)).G * (double)(_stats.StatsD[x].Scaled())).Sum());
                c.B = (byte)(_circle.Select(x => (double)(CellToColorConverter.Convert(x)).B * (double)(_stats.StatsD[x].Scaled())).Sum());
                return c;
            } 
        }

        public double PSize { get { return _circle.Select(x => (_stats.StatsD[x].Scaled())).Sum(); } }
    }
}
