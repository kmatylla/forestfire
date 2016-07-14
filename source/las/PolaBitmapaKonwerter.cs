//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Automaton
{
    public class CellToBitmapConverter
    {
        private Cell.StateEnum[,] _cells;
        private Bitmap _bmp;
        public Cell.StateEnum[,] Cells
        {
            get { return _cells; }
            set
            {
                if ((_cells==null)||(value != _cells))
                {
                    _cells = value;
                    generateBitmap();
                }
            }
        }
        public Bitmap Bmp
        {
            get { return _bmp; }
            set
            {
                if ((_bmp==null)||(value != _bmp))
                {
                    _bmp = value;
                    generateCells();
                }
            }
        }

        private void generateBitmap()
        {
            _bmp = new Bitmap(_cells.GetLength(0),_cells.GetLength(1));
            for (int i=0;i<_cells.GetLength(0);++i)
                for (int j = 0; j < _cells.GetLength(1); ++j)
                {
                    System.Windows.Media.Color c=CellToColorConverter.Convert(_cells[i, j]);
                    _bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(c.R,c.G,c.B));
                }
        }
        private void generateCells()
        {
            _cells = new Cell.StateEnum[_bmp.Width, _bmp.Height];
            for (int i = 0; i < _cells.GetLength(0); ++i)
                for (int j = 0; j < _cells.GetLength(1); ++j)
                {
                    System.Drawing.Color c = _bmp.GetPixel(i,j);
                    _cells[i, j] = CellToColorConverter.Convert(System.Windows.Media.Color.FromRgb(c.R,c.G,c.B));
                }
        }

        public CellToBitmapConverter(Bitmap bmp)
        {
            Bmp = bmp;
        }
        public CellToBitmapConverter(Cell.StateEnum[,] cells)
        {
            Cells = cells;
        }

    }
}
