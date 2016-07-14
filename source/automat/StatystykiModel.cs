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
    public class StatsModel
    {
       private Dictionary<Cell.StateEnum, StatModel> _stats;
        public Dictionary<Cell.StateEnum,StatModel> Stats{get {return _stats;}}

        public void Update(Dictionary<Cell.StateEnum,int> stats)
        {
          foreach (var s in stats)
              Stats[s.Key].Update(s.Value);
        }

        public StatsModel(Dictionary<Cell.StateEnum, int> stats)
        {
         _stats=new Dictionary<Cell.StateEnum,StatModel>();
          foreach(var s in stats) _stats.Add(s.Key,new StatModel(s.Value,s.Key));
        }

    }
}
