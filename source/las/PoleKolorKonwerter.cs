//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text;

namespace Automaton
{
    public static class CellToColorConverter
    {
        private static Dictionary<Cell.StateEnum, Color> _colors = new Dictionary<Cell.StateEnum, Color>
        {  { Cell.StateEnum.YoungTree, Colors.LightGreen}
                ,{ Cell.StateEnum.MatureTree
                    ,  Colors.Green}
                ,{ Cell.StateEnum.OldTree
                    ,  Colors.DarkGreen}
                ,{ Cell.StateEnum.DeadTree
                    ,  Colors.Orange}
                ,{ Cell.StateEnum.Soil
                    ,  Colors.DarkGray}
                ,{ Cell.StateEnum.RichSoil
                    ,  Colors.Gray}
                ,{ Cell.StateEnum.Fire
                    ,  Colors.Red}
                    ,{ Cell.StateEnum.Water
                    ,  Colors.Blue}
              };

        public static Color Convert(Cell.StateEnum t) 
        {
            if (_colors.ContainsKey(t)) return _colors[t];
            return Default;
        }
        public static Cell.StateEnum Convert(Color k) 
        {
            //if (_kolory.ContainsValue(k)) return _kolory.Single(a => a.Value == k).Key;
          //  return Pole.TypStan.Ziemia;
            var minDist = _colors.Values.Min(c => (c.R - k.R) * (c.R - k.R) + (c.G - k.G) * (c.G - k.G) + (c.B - k.B) * (c.B - k.B));
            return _colors.First(c => (c.Value.R - k.R) * (c.Value.R - k.R) + (c.Value.G - k.G) * (c.Value.G - k.G) + (c.Value.B - k.B) * (c.Value.B - k.B) == minDist).Key;
        }

        public static Color LeftColor { get; set; }
        public static Color RightColor { get; set; }
        public static Color MiddleColor { get; set; }
        public static Color Default { get; private set; }

        static CellToColorConverter()
        {
            LeftColor = Colors.GreenYellow;
            RightColor = Colors.Blue;
            MiddleColor = Colors.Red;
            Default = Colors.Black;
        }
    }
}
