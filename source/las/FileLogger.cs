//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automaton
{
    public class FileLogger
    {
        private AutomatonPresenter _ap;
        private System.IO.StreamWriter _file;
        private bool _isOpened;

        public void AddStep(object sender, StepData s)
        {
            if (_isOpened)
            {
                _file.Write(String.Format("Steps: {0,7};   ", s.Steps));
                _file.WriteLine(String.Join(";   ", s.Counts.Select(x => String.Format("{0}: {1,7}", x.Key, x.Value)).ToArray()));
                _file.Flush();
            }
        }

        public void Close()
        {
            if (_isOpened)
            {
                _isOpened = false;
                _file.Close();
            }
        }

        public FileLogger(AutomatonPresenter ap, String f)
        {
            _ap = ap;
            _ap.StepEvent += AddStep;
            _file = new System.IO.StreamWriter(f);
            _isOpened = true;
            for (int i = 0; i < _ap.Params.Count(); ++i) _file.WriteLine(_ap.ParamNames[i]+": "+_ap.Params[i]);
            _file.WriteLine();
        }
    }
}
