//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaton
{
    public class ForestModel
    {
        public Cell[,] Cells {get; private set;}
        public int SizeX 
        {
            get { return Cells.GetLength(0); }
        }
        public int SizeY
        {
            get { return Cells.GetLength(1); }
        }
         public int Steps { get; private set; }
        public bool WrapX { get; set; }
        public bool WrapY { get; set; }

        private double[] _parameters = new double[36]
        {
            .0005, .0002, .00005, .0005, .33, .002,
            .00000003,.0008, .001, 0.0003, .25,
            .0000001,.0015, .005, 0.0001, .4,
            .000001,.05, .33, .1, .99 ,
            .000001, .15, .6, .1, .95,
            .000001, .3, .75, .1, .95,
            .000001, .5, .9, .1, .9,
        };

        private bool ShallItchange(double percent, double val0, double valMin, double valOpt, double valMax, double optimum)
        {
            if (optimum<0 || optimum>1) throw (new ArgumentException(
                String.Format("Incorrect peak position ({0}). Should be between 0 and 1.",optimum)));
            if (percent<0 || percent>1) throw (new ArgumentException(
                String.Format("Incorrect percentage value ({0}). Should be between 0 and 1",percent)));
            if (percent==0) return (_rand.NextDouble() < val0);
            if (percent < optimum) return (_rand.NextDouble() < percent*percent*(val0-valOpt)/(optimum*optimum)+percent*2*(valOpt-val0)/optimum+val0);
            return (_rand.NextDouble() < percent * percent * (1 - optimum) * (1 - optimum) / (valMax - valOpt) - 2 * optimum * percent * (1 - optimum) * (1 - optimum) / (valMax - valOpt) + valMax - (1 - optimum) * (1 - optimum) / (valMax - valOpt) + 2 * optimum * (1 - optimum) * (1 - optimum) / (valMax - valOpt));
       
        }

        public double[] Parameters { get { return _parameters; } }

        public readonly string[] ParamNames = new string[36] 
        {
            "Young trees growth rate", 
            "Mature trees growth rate", 
            "Old trees death rate", 
            "Prob. of a dead tree falling", 
            "Prob. of a fire dying", 
            "Prob. of rich soil turning normal",

            "Prob. of tree appearance in empty area",
            "Prob. of tree appearance in sparse area",
            "Prob. of tree appearance in optimum density area",
            "Prob. of tree appearance in maximum density area",
            "Forest density for optimum seeding (0-1)",

            "Prob. of tree appearance in empty area (rich soil)",
            "Prob. of tree appearance in sparse area (rich soil)",
            "Prob. of tree appearance in optimum density area (rich soil)",
            "Prob. of tree appearance in maximum density area (rich soil)",
            "Forest density for optimum seeding  (rich soil) (0-1)",

            "Prob. of spontaneous combustion (young tree)",
            "Prob. of combustion with few neighbours on fire (young tree)",
            "Prob. of combustion with optimum fire density in neighborhood (young tree)",
            "Prob. of combustion with all neighbors on fire (young tree)",
            "Fire density for optimum combustion (young tree) (0-1)",

            "Prob. of spontaneous combustion (mature tree)",
            "Prob. of combustion with few neighbours on fire (mature tree)",
            "Prob. of combustion with optimum fire density in neighborhood (mature tree)",
            "Prob. of combustion with all neighbors on fire (mature tree)",
            "Fire density for optimum combustion (mature tree) (0-1)",
            
            "Prob. of spontaneous combustion (old tree)",
            "Prob. of combustion with few neighbours on fire (old tree)",
            "Prob. of combustion with optimum fire density in neighborhood (old tree)",
            "Prob. of combustion with all neighbors on fire (old tree)",
            "Fire density for optimum combustion (old tree) (0-1)",

            "Prob. of spontaneous combustion (dead tree)",
            "Prob. of combustion with few neighbours on fire (dead tree)",
            "Prob. of combustion with optimum fire density in neighborhood (dead tree)",
            "Prob. of combustion with all neighbors on fire (dead tree)",
            "Fire density for optimum combustion (dead tree) (0-1)"
        };

        private void paramSet(double val,int nr)
        {
            if (nr>=0 && nr <_parameters.Length && val >= 0 && val <= 1) _parameters[nr] = val;
        }

        public double ParamYoungToMat { get { return _parameters[0]; } set { paramSet(value, 0); } }
        public double ParamMatToOld { get { return _parameters[1]; } set { paramSet(value, 1); } }
        public double ParamOldToDead { get { return _parameters[2]; } set { paramSet(value, 2); } }
        public double ParamDeadToSoil { get { return _parameters[3]; } set { paramSet(value, 3); } }
        public double ParamFireToSoil { get { return _parameters[4]; } set { paramSet(value, 4); } }
        public double ParamRichToSoil { get { return _parameters[5]; } set { paramSet(value, 5); } }
        public double ParamGrow0 { get { return _parameters[6]; } set { paramSet(value, 6); } }
        public double ParamGrowMin { get { return _parameters[7]; } set { paramSet(value, 7); } }
        public double ParamGrowOpt { get { return _parameters[8]; } set { paramSet(value, 8); } }
        public double ParamGrowMax { get { return _parameters[9]; } set { paramSet(value, 9); } }
        public double ParamGrowOptVal { get { return _parameters[10]; } set { paramSet(value, 10); } }
        public double ParamGrowRich0 { get { return _parameters[11]; } set { paramSet(value, 11); } }
        public double ParamGrowRichMin { get { return _parameters[12]; } set { paramSet(value, 12); } }
        public double ParamGrowRichOpt { get { return _parameters[13]; } set { paramSet(value, 13); } }
        public double ParamGrowRichMax { get { return _parameters[14]; } set { paramSet(value, 14); } }
        public double ParamGrowRichOptVal { get { return _parameters[15]; } set { paramSet(value, 15); } }
        public double ParamFireY0 { get { return _parameters[16]; } set { paramSet(value, 16); } }
        public double ParamFireYMin { get { return _parameters[17]; } set { paramSet(value, 17); } }
        public double ParamFireYOpt { get { return _parameters[18]; } set { paramSet(value, 18); } }
        public double ParamFireYMax { get { return _parameters[19]; } set { paramSet(value, 19); } }
        public double ParamFireYOptVal { get { return _parameters[20]; } set { paramSet(value, 20); } }
        public double ParamFireM0 { get { return _parameters[21]; } set { paramSet(value, 21); } }
        public double ParamFireMMin { get { return _parameters[22]; } set { paramSet(value, 22); } }
        public double ParamFireMOpt { get { return _parameters[23]; } set { paramSet(value, 23); } }
        public double ParamFireMMax { get { return _parameters[24]; } set { paramSet(value, 24); } }
        public double ParamFireMOptVal { get { return _parameters[25]; } set { paramSet(value, 25); } }
        public double ParamFireO0 { get { return _parameters[26]; } set { paramSet(value, 26); } }
        public double ParamFireOMin { get { return _parameters[27]; } set { paramSet(value, 27); } }
        public double ParamFireOOpt { get { return _parameters[28]; } set { paramSet(value, 28); } }
        public double ParamFireOMax { get { return _parameters[29]; } set { paramSet(value, 29); } }
        public double ParamFireOOptVal { get { return _parameters[30]; } set { paramSet(value, 30); } }
        public double ParamFireD0 { get { return _parameters[31]; } set { paramSet(value, 31); } }
        public double ParamFireDMin { get { return _parameters[32]; } set { paramSet(value, 32); } }
        public double ParamFireDOpt { get { return _parameters[33]; } set { paramSet(value, 33); } }
        public double ParamFireDMax { get { return _parameters[34]; } set { paramSet(value, 34); } }
        public double ParamFireDOptVal { get { return _parameters[35]; } set { paramSet(value, 35); } }


        /// <summary>
        /// maximum radius
        /// </summary>
        private int _maxR = 10;
        private int _paramRFire;
      
        public int ParamRFire { get { return _paramRFire; } set { if (value >= 0 && value <= _maxR) { _paramRFire = value; sasiedzi(false); } } } 
        private int _paramRTrees;
      
        public int ParamRTrees { get { return _paramRTrees; } set { if (value >= 0 && value <= _maxR) { _paramRTrees = value; sasiedzi(true); } } }
        private Random _rand;
        private Dictionary<Cell.StateEnum, int> _stats;
        public EventHandler<StepData> StepEvent;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="x">rozmiar x</param>
        /// <param name="y">rozmiar y</param>
        /// <param name="wx">zawijanie w x</param>
        /// <param name="wy">zawijanie w y</param>
        /// <param name="density">% drzew na starcie</param>
        public ForestModel(int x, int y, bool wx, bool wy, double density)
        {
            Cells = new Cell[x, y];
            _rand = new Random();
            Steps = 0;
            _paramRFire = 1;
            _paramRTrees = 3;
            WrapX = wx;
            WrapY = wy;
            var r=new Random();
            for (int i = 0; i < x; ++i)
                for (int j = 0; j < y; ++j)
                    if (r.NextDouble()<density) Cells[i, j] = new Cell(Cell.StateEnum.YoungTree);
                    else Cells[i, j] = new Cell(Cell.StateEnum.Soil);
            sasiedzi(true);
            sasiedzi(false);
            _stats = new Dictionary<Cell.StateEnum, int>();
            foreach (var p in Enum.GetValues(typeof(Cell.StateEnum)).OfType<Cell.StateEnum>())
            {
                _stats.Add(p, 0);
            }
            
        }

        public ForestModel(Cell.StateEnum [,] pola, bool zx=false, bool zy=false)
        {
            Cells = new Cell[pola.GetLength(0),pola.GetLength(1)];
            _rand = new Random();
            Steps = 0;
            _paramRFire = 1;
            _paramRTrees = 3;
            WrapX = zx;
            WrapY = zy;
            var r = new Random();
            for (int i = 0; i < Cells.GetLength(0); ++i)
                for (int j = 0; j < Cells.GetLength(1); ++j)
                    Cells[i, j] = new Cell(pola[i,j]);
            sasiedzi(true);
            sasiedzi(false);
            _stats = new Dictionary<Cell.StateEnum, int>();
            foreach (var p in System.Enum.GetValues(typeof(Cell.StateEnum)))
            {
                var pole = (Cell.StateEnum)p;
                _stats.Add(pole, 0);
            }
        }


        private void sasiedzi(bool drzewa)
        {
            int ParamR;
            if (drzewa) ParamR = ParamRTrees;
            else ParamR = ParamRFire;
            for (int x = 0; x < SizeX; ++x)
                for (int y = 0; y < SizeY; ++y)
                {
                    var s = new System.Collections.Generic.List<Cell>();
                    for (int i = -ParamR; i <= ParamR; ++i)
                        for (int j = -ParamR; j <= ParamR; ++j)
                        {
                            int x1 = x + i;
                            int y1 = y + j;
                            if (WrapX) x1 =(x1+SizeX)% SizeX;
                            if (WrapY) y1 =(y1+SizeY)% SizeY;
                            if ((x1 >= 0) && (x1 < SizeX) && (y1 >= 0) && (y1 < SizeY)) s.Add(Cells[x1, y1]);
                        }
                    if (drzewa) Cells[x, y].NeighboursT = s.ToArray();
                    else Cells[x, y].NeighboursF = s.ToArray();
                    Cells[x, y].AfterNeighboursChanged(drzewa);
                }
        }

        private Cell.StateEnum pole_krok(int x, int y)
        {
            Cell.StateEnum p = Cells[x, y].MyState;
            switch (p)
            {
                case Cell.StateEnum.Soil:
                {
                    if (ShallItchange(Cells[x, y].NeighboursPercentT,ParamGrow0,ParamGrowMin,ParamGrowOpt,ParamGrowMax,ParamGrowOptVal)) return Cell.StateEnum.YoungTree;
                    break;
                }
                case Cell.StateEnum.YoungTree:    
                   {
                       if (ShallItchange(Cells[x, y].NeighboursPercentF, ParamFireY0, ParamFireYMin, ParamFireYOpt, ParamFireYMax, ParamFireYOptVal))
                    {

                        return Cell.StateEnum.Fire;
                    }
                    if (_rand.NextDouble() < ParamYoungToMat)
                    {
 
                        return Cell.StateEnum.MatureTree;
                    }
                    break;
                }
                case Cell.StateEnum.MatureTree:            
                   {
                       if (ShallItchange(Cells[x, y].NeighboursPercentF, ParamFireM0, ParamFireMMin, ParamFireMOpt, ParamFireMMax, ParamFireMOptVal))
                    {

                        return Cell.StateEnum.Fire;
                    }
                    if (_rand.NextDouble() < ParamMatToOld) return Cell.StateEnum.OldTree;
                    break;
                }
                case Cell.StateEnum.OldTree:
                  {
                      if (ShallItchange(Cells[x, y].NeighboursPercentF, ParamFireO0, ParamFireOMin, ParamFireOOpt, ParamFireOMax, ParamFireOOptVal))
                    {
                   
                        return Cell.StateEnum.Fire;
                    }
                    if (_rand.NextDouble() < ParamOldToDead)
                    {
                 
                        return Cell.StateEnum.DeadTree;
                    }
                    break;
                }
                case Cell.StateEnum.DeadTree:
                   {
                       if (ShallItchange(Cells[x, y].NeighboursPercentF, ParamFireD0, ParamFireDMin, ParamFireDOpt, ParamFireDMax, ParamFireDOptVal))
                    {
    
                        return Cell.StateEnum.Fire;
                    }
                    if (_rand.NextDouble() < ParamDeadToSoil) return Cell.StateEnum.Soil;
                    break;
                }
                case Cell.StateEnum.Fire:
                   {
                       if (_rand.NextDouble() < ParamFireToSoil)
                       {

                           return Cell.StateEnum.RichSoil;
                       }
                    break;
                }
                case Cell.StateEnum.RichSoil:
                   {
                       if (ShallItchange(Cells[x, y].NeighboursPercentT, ParamGrowRich0, ParamGrowRichMin, ParamGrowRichOpt, ParamGrowRichMax, ParamGrowRichOptVal)) return Cell.StateEnum.YoungTree;
                    if (_rand.NextDouble() < ParamRichToSoil) return Cell.StateEnum.Soil;
                    break;
                }
            }

            return Cells[x,y].MyState;
        }

        public void DoStep()
        {
            var nPlansza = new Cell.StateEnum[SizeX, SizeY];
            for (int i = 0; i < SizeX; ++i) for (int j = 0; j < SizeY; ++j)
                    nPlansza[i, j] = pole_krok(i, j);
            var Statystyki2=new Dictionary<Cell.StateEnum, int>();
            foreach (var p in System.Enum.GetValues(typeof(Cell.StateEnum)))
            {
                var pole = (Cell.StateEnum)p;
                Statystyki2[pole] = 0;
            }
            for (int i = 0; i < SizeX; ++i) for (int j = 0; j < SizeY; ++j)
                {
                    Cells[i, j].MyState = nPlansza[i, j];
                    Statystyki2[nPlansza[i, j]]++;
                }
            _stats = Statystyki2;
            if (StepEvent != null) StepEvent(this, new StepData(_stats,Steps));
            ++Steps;
        }

        public ForestModel() : this(256, 256, false, false, 0) { }

        public override string ToString()
        {
            String s = "";
            for (int x=0;x<SizeX;++x)
            {
                for (int y=0;y<SizeY;++y) s+=Cells[x,y];
                s+="\n";
            }
            return s;
        }

        public Dictionary<Cell.StateEnum, int> Stats
        {
            get
            {
                return _stats;
            }
        }
    }
}
