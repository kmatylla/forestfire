//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaton
{
    public class Cell
    {
        public Cell[] NeighboursT { get; set; }
        public Cell[] NeighboursF { get; set; }
        public double NeighboursPercentT 
        {
            get 
            {
                if ((double)_trees / NeighboursT.Length > 1) throw (new ArgumentException(String.Format("Zła wartość ({0}).", (double)_trees / NeighboursT.Length)));
                return (double)_trees / NeighboursT.Length; 
            } 
        }
        public double NeighboursPercentF
        {
            get
            {
                if ((double)_fires / NeighboursF.Length > 1) throw (new ArgumentException(String.Format("Zła wartość ({0}).", (double)_fires / NeighboursF.Length)));
                return (double)_fires / NeighboursF.Length;
            }
        }
        private int _trees;
        private int _fires;

        private static bool IsTree(StateEnum t)
        {
            return(t==StateEnum.MatureTree||t==StateEnum.OldTree);
        }

        private static bool IsFire(StateEnum t)
        {
            return(t==StateEnum.Fire);
        }

        private bool IsTree()
        {
            return IsTree(_myState);
        }

        private bool IsFire()
        {
            return IsFire(_myState);
        }

        public void AfterNeighboursChanged(bool trees)
        {
            if (trees)
            {
                _trees = 0;
                for (int i = 0; i < NeighboursT.Length; ++i) if (NeighboursT[i].IsTree()) _trees++;
            }
            else
            {
                _fires = 0;
                for (int i = 0; i < NeighboursF.Length; ++i) if (NeighboursF[i].IsFire()) _fires++;
            }
        }

        public void InformNeighbours(bool increase, bool trees)
        {
            int zmiana;
            if (increase) zmiana=1;
            else zmiana =-1;
            Cell[] pola;
            if (trees) pola = NeighboursT;
            else pola = NeighboursF;
            for (int i = 0; i < pola.Length; ++i)
            {
                if (trees) pola[i]._trees += zmiana;
                else pola[i]._fires += zmiana;
            }

        }

        public enum StateEnum {Soil, RichSoil, YoungTree, MatureTree, OldTree, DeadTree, Fire,  Water }
        private StateEnum _myState;
        public StateEnum MyState 
        {
            get
            {
                return _myState;
            }
            set
            {
                if (_myState != value)
                {
                    if (IsFire()!=IsFire(value)) InformNeighbours(IsFire(value), false);
                    if (IsTree() != IsTree(value)) InformNeighbours(IsTree(value), true);
                    _myState = value;
                }
            }
        }
        public Cell(StateEnum t) { _myState = t; }
        public Cell() : this(StateEnum.Soil) { }


        public override string ToString()
        {
            switch (MyState)
            {
                case StateEnum.Soil:
                    return " ";
                case StateEnum.RichSoil:
                    return "_";
                case StateEnum.Fire:
                    return "*";
                case StateEnum.YoungTree:
                    return "+";
                case StateEnum.MatureTree:
                    return "t";
                case StateEnum.DeadTree:
                    return "I";
                default: return "T";
            }
        }
    }
}
