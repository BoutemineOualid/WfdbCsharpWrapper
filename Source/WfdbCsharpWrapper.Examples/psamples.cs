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
using System.Collections.Generic;

namespace WfdbCsharpWrapper.Examples
{
    class psamples
    {
        public static void Start()
        {
            UsingPInvoke();
        }

        public static void UsingPInvoke()
        {
            Console.WriteLine("psamples using PInvoke calls");
            int i;
            Sample[] v = new Sample[2];
            Signal[] s = new Signal[2];

            if (PInvoke.isigopen("data/100s", s, 2) < 1)
                return;
            for (i = 0; i < 10; i++)
            {
                if (PInvoke.getvec(v) < 0)
                    break;
                Console.WriteLine("{0}\t{1}", v[0], v[1]);
            }
        }

        public static void UsingWrapperClasses()
        {
            var record = new Record("data/100s");
            record.Open();

            Console.WriteLine("Record Name : " + record.Name);
            Console.WriteLine("Record Info : " + record.Info);
            Console.WriteLine("Record's Sampling Frequency : " + record.SamplingFrequency);

            Console.WriteLine("Available signals.");

            foreach (Signal signal in record.Signals)
            {

                Console.WriteLine("=====================================");
                Console.WriteLine("Signal's Name : " + signal.FileName);
                Console.WriteLine("Signal's Description : " + signal.Description);
                Console.WriteLine("Signal's Number of samples : " + signal.NumberOfSamples);
                Console.WriteLine("Signal's First Sample : " + signal.InitValue);

                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Showing the first 10 samples of the signal");
                Console.WriteLine("------------------------------------------");

                List<Sample> samples = signal.ReadNext(10);

                for (int i = 0; i < samples.Count; i++)
                {
                    Console.WriteLine("Sample " + i + " Value (adu) = " + samples[i].Adu);
                    Console.WriteLine("             Value (microvolt) = " + samples[i].ToMicrovolts());
                    Console.WriteLine("             Value (millivolt) = " + samples[i].ToPhys());
                }

                Console.WriteLine("--------------------------------------");

                Console.WriteLine("=====================================");
            }


            record.Dispose();

        }
    }
}
