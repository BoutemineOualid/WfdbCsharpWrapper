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
    public class exgetvec
    {
        public static void Start()
        {
            UsingPInvoke();
            UsingWrapperClasses1();
            UsingWrapperClasses2();
            UsingWrapperClasses3();
        }

        private static void UsingPInvoke()
        {
            Console.WriteLine("exgetvec Using PInvoke");

            int i, j, nsig;
            Sample[] v;
            Signal[] s;

            nsig = PInvoke.isigopen("data/100s", null, 0);
            if (nsig < 1)
                return;
            s = new Signal[nsig];


            if (PInvoke.isigopen("data/100s", s, nsig) != nsig)
                return;

            v = new Sample[nsig];
            for (i = 0; i < 10; i++)
            {
                if (PInvoke.getvec(v) < 0)
                    break;
                for (j = 0; j < nsig; j++)
                    Console.Write("{0} \t", v[j]);
                Console.WriteLine();
            }
            PInvoke.wfdbquit();
        }

        private static void UsingWrapperClasses1()
        {
            Console.WriteLine("exgetvec Using Wrapper Record Class");
            using (var record = new Record("data/100s"))
            {
                record.Open();

                var samples = record.GetSamples(10);
                foreach (var s in samples)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        Console.Write(string.Format("{0}\t", s[i]));
                    }
                    Console.WriteLine();
                }
            }

            Wfdb.Quit();
        }

        private static void UsingWrapperClasses2()
        {
            Console.WriteLine("exgetvec Using Wrapper Signal class");
            using (var record = new Record("data/100s"))
            {
                record.Open();
                var samples1 = record.Signals[0].ReadNext(10);
                var samples2 = record.Signals[1].ReadNext(10);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("{0}\t{1}", samples1[i], samples2[i]);
                }
            }
            Wfdb.Quit();
        }

        private static void UsingWrapperClasses3()
        {
            Console.WriteLine("exgetvec Using Wrapper Signal class and Signal.Seek Method");
            using (var record = new Record("data/100s"))
            {
                record.Open();
                var samples1 = record.Signals[0].ReadNext(10);

                record.Signals[1].Seek(10); // moving the reading pointer of the second signal by ten positions
                var samples2 = record.Signals[1].ReadNext(10);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("{0}\t{1}", samples1[i], samples2[i]);
                }
            }
            Wfdb.Quit();
        }
    }
}
