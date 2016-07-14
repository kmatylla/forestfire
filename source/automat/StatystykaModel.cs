//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Automaton
{
    public class StatModel
    {
        private long _sum;
        private long _squareSum;
        private int _steps;
        private int _max;
        private Cell.StateEnum _state;
        public int Value {get; private set;}
        public Cell.StateEnum State { get { return _state; } }

        public double Normalised(int stat=-1)
        {
            if (stat < 0) stat = Value;
            if (_steps == 0 || _squareSum >= (_sum * _sum)) return 0;
            return ((double)stat - (double)_sum / _steps) / (Math.Sqrt((double)_squareSum / _steps - Math.Pow((double)_sum / _steps, 2)));
        }

        public double Scaled(int stat=-1)
        {
            if (stat < 0) stat = Value;
            if (_steps == 0 || _sum == 0) return 0;
            return (((double)stat*_steps)/_sum);
        }

        public double PrzeskalowanaMax(int stat = -1)
        {
            if (stat < 0) stat = Value;
            if (_steps == 0 || _sum == 0) return 0;
            return (((double)stat) / _max);
        }

        public void Update(int stat)
        {
           ++_steps;
           _sum += stat;
           _squareSum+=stat*stat;
           Value = stat;
           if (stat > _max) _max = stat;
        }

        public StatModel(int stat, Cell.StateEnum state)
        {
            _max = 0;
            _steps=1;
            Value = stat;
            _state = state;
            _sum = stat;
            _squareSum=stat;
        }


    
    }
}
