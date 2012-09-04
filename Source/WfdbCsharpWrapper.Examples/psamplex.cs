/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2010
 * Contact: boutemine.walid@hotmail.com
 * Project web page: wfdbcsharpwrapper.codeplex.com
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
    public class psamplex
    {
        public static void Start()
        {
            UsingPInvoke();
        }

        private static void UsingPInvoke()
        {
            Console.WriteLine("psamplex using PInvoke ");
            Frequency f = 0;
            Sample[] v = new Sample[2];
            Signal[] s = new Signal[2];

            Time t, t0, t1;
            Console.WriteLine("Please enter the sampling frequency");
            try
            {
                var sFrequency = Console.ReadLine();
                f = double.Parse(sFrequency);
                if (f < 0)
                    f = PInvoke.sampfreq("data/100s");
                //PInvoke.isigopen("data/100s");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void UsingWrapperClasses()
        {
            Frequency f = 0;
            Sample[] v;
            
            try
            {
                Console.WriteLine("Please enter the sampling frequency");
                var sFrequency = Console.ReadLine();
                f = double.Parse(sFrequency);

                Record r =new Record("data/100s");
                r.Open();
                
                if (f < 0)
                    f = r.SamplingFrequency;

                List<Signal> s = r.Signals;
                Frequency.InputFrequency = f;

                Time t0 = Time.Parse("1");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
