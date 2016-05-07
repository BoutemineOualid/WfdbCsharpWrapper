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
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper.Examples
{
    public class example3
    {
        static void Main(string[] args)
        {
            UsingPInvoke();
            UsingWrapperClasses();
            Console.ReadLine();
        }

        private static void UsingWrapperClasses()
        {

            var a = new Annotator();
            a.Stat = Stat.Read;
            a.Name = "atr";
            a.Open("data/100s");

            while (!a.IsEof)
            {
                var annot = a.ReadNext();
                Console.WriteLine("{0} ({1}) {2} {3} {4} {5} {6}",
                       annot.Time.ToString(),
                       annot.Time,
                       annot.Type.ToString(), // or .String
                       annot.SubType,
                       annot.ChannelNumber,
                       annot.AnnotatorNumber,
                       (!string.IsNullOrEmpty(annot.Aux)) ? annot.Aux + 1 : "");
            }
        }

        static void UsingPInvoke()
        {
            Annotator a = new Annotator();
            a.Name = "data/100s"; // just use 100s for demo
            a.Stat = Stat.Read;

            Annotation annot = new Annotation();
            PInvoke.sampfreq("data/100s");

            if (PInvoke.annopen("data/100s", ref a, 1) < 0) 
                return;
            while (PInvoke.getann(0, ref annot) == 0)
                Console.WriteLine("{0} ({1}) {2} {3} {4} {5} {6}",
                       Marshal.PtrToStringAuto(PInvoke.timstr(-(annot.Time))),
                       annot.Time,
                       Marshal.PtrToStringAuto(PInvoke.annstr(annot.Type)),
                       annot.SubType, 
                       annot.ChannelNumber, 
                       annot.AnnotatorNumber,
                       (!string.IsNullOrEmpty(annot.Aux)) ? annot.Aux + 1 : "");
        }
    }
}
