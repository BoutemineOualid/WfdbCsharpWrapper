/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2012
 * Contact: boutemine.walid@hotmail.com
 * Project web page: https://github.com/oualidb/WfdbCsharpWrapper
 * Code Documentation : From WFDB Programmer's Guide BY George B. Moody
 * wfdb: 
 * -----
 * a library for reading and writing annotated waveforms (time series data)
 * Copyright (C) 1999 George B. Moody

 * This library is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Library General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option) any
 * later version.

 * This library is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
 * PARTICULAR PURPOSE.  See the GNU Library General Public License for more
 * details.

 * You should have received a copy of the GNU Library General Public License along
 * with this library; if not, write to the Free Software Foundation, Inc., 59
 * Temple Place - Suite 330, Boston, MA 02111-1307, USA.

 * You may contact the author by e-mail (george@mit.edu) or postal mail
 * (MIT Room E25-505A, Cambridge, MA 02139 USA).  For updates to this software,
 * please visit PhysioNet (http://www.physionet.org/).
 * _______________________________________________________________________________
 */

using System;

namespace WfdbCsharpWrapper.Examples
{
    class pgain
    {
        public static void Start()
        {
            UsingPInvoke();
            UsingSignal();
            UsingRecord();
        }

        private static void UsingPInvoke()
        {
            Console.WriteLine("pgain Using PInvoke call");
            int i, nsig;
            Signal[] siarray;

            nsig = PInvoke.isigopen("data/100s", null, 0);
            if (nsig < 1)
                return;
            siarray = new Signal[nsig];
            nsig = PInvoke.isigopen("data/100s", siarray, nsig);
            for (i = 0; i < nsig; i++)
            {
                Console.Write("signal {0} gain = {1}\n", i, siarray[i].Gain);
            }
            PInvoke.wfdbquit();
        }

        private static void UsingSignal()
        {
            Console.WriteLine("pgain Using Signal Class");
            var signals = Signal.GetSignals("data/100s");
            int counter = 0; 
            foreach (var signal in signals)
            {
                Console.WriteLine("Signal {0} gain = {1}", counter, signal.Gain);
                counter++;
            }
            Wfdb.Quit();
        }

        private static void UsingRecord()
        {
            Console.WriteLine("pgain Using Record Class");
            using (var record = new Record("data/100s"))
            {
                record.Open();
                int counter = 0;
                foreach (var signal in record)// or (var signal in record.Signals)
                {
                    Console.WriteLine("Signal {0} gain = {1}", counter, signal.Gain);
                    counter++;
                }
            }
        }
    }
}