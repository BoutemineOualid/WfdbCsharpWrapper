/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Oualid BOUTEMINE, 2009-2016
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
    public class psamplex
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the sampling frequency");
            int frequency;
            int.TryParse(Console.ReadLine(), out frequency);
            UsingPInvoke(frequency);
            UsingWfdbClasses(frequency);
            Console.Read();
        }

        private static void UsingWfdbClasses(int frequency)
        {
            try
            {
                Frequency f = frequency;
                Time t = 0, t0 = 0, t1 = 0;

                using (var record = new Record("data/100s"))
                {
                    record.Open();
                    if (f <= 0)
                        f = record.SamplingFrequency;

                    Frequency.InputFrequency = f;
                    t0 = Time.Parse("1");
                    record.Seek(t);
                    t1 = Time.Parse("2");
                    for (t = t0; t <= t1; t++)
                    {
                        Console.WriteLine("{0}\t{1}", record[0].ReadNext(), record[1].ReadNext());
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }   
        }

        private static void UsingPInvoke(int frequency)
        {
            try
            {
                Frequency f = frequency;
                Sample[] v = new Sample[2];
                Signal[] s = new Signal[2];
                Time t = 0, t0 = 0, t1 = 0;

                if (f <= 0)
                    f = PInvoke.sampfreq("data/100s");

                if (PInvoke.isigopen("data/100s", s, 2) < 1)
                    return;
                PInvoke.setifreq(f);
                t0 = PInvoke.strtim("1");
                PInvoke.isigsettime(t);
                t1 = PInvoke.strtim("2");
                for (t = t0; t <= t1; t++)
                {
                    if (PInvoke.getvec(v) < 0)
                        break;
                    Console.WriteLine("{0}\t{1}", v[0], v[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}