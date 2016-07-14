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
    public class GraphPoint
    {
        public Brush ColorBrush { get; private set; }
        public int Bottom { get; private set; }
        public int Height { get; private set; }

        public GraphPoint(Brush b, int v1, int v0)
        {
            ColorBrush = b;
            Bottom = Math.Min(v1, v0);
            Height = Math.Max(2, Math.Abs(v1 - v0));
        }

        public GraphPoint(Brush b, int v)
        {
            ColorBrush = b;
            Bottom = v;
            Height = 1;
        }
    }
}
