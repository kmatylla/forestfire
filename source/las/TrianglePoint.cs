//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Automaton
{
    public class TrianglePoint
    {
        public int X { get { return (int)(_x*_parent.Width/2+_parent.Width/2); } }
        public int Y { get { return (int)(_y*_parent.Height/2+_parent.Height/2); } }
        public int X2 { get { return (int)(_x2 * _parent.Width / 2 + _parent.Width / 2); } }
        public int Y2 { get { return (int)(_y2 * _parent.Height / 2 + _parent.Height / 2); } }
        public int Size { get { return (int)(_size); } }
        public Color PColor 
        {
            get 
            {
                var c = _pColor;
 //               c.A = (byte)(255 - Age);
                return c;
            } 
        }
        public Brush ColorBrush { get { return new SolidColorBrush(PColor); } }
        private TrianglePresenter _parent;

        private double _x;
        private double _y;
        private double _size;
        private Color _pColor;
        private double _y2;
        private double _x2;
        public int Age { get; private set; }
        public void Step()
        {
            ++Age;
        }

        public TrianglePoint(double x, double y,double x2 ,double y2 , double size, Color color, TrianglePresenter m)
        {
            _x = x;
            _y = y;
            _x2 = x2;
            _y2 = y2;
            _size = size;
            _pColor = color;
            Age = 1;
            _parent = m;
        }

        public TrianglePoint(TrianglePoint source):
            this(source._x, source._y, source._x2, source._y2, source._size, source._pColor, source._parent)
        {}
    }
}
