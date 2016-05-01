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
    public class exgetann
    {
        static void Main(string[] args)
        {
            UsingPInvoke();
            UsingAnnotatorClass1();
            UsingAnnotatorClass2();
            UsingAnnotatorClass3();
            Console.Read();
        }


        private static void UsingPInvoke()
        {
            Console.WriteLine("exgetann Using PInvoke");
            var annotator = new Annotator();
            var annotation = new Annotation();

            annotator.Name = "atr";
            annotator.Stat = Stat.Read;
            var ret = PInvoke.annopen("data/100s", ref annotator, 1);
            if (ret < 0)
                return;
            while (PInvoke.getann(0, ref annotation) == 0)
            {
                Console.WriteLine(annotation.ToString());
            }
            PInvoke.wfdbquit();
        }

        public static void UsingAnnotatorClass1()
        {
            Console.WriteLine("exgetann Using Annotator.ReadAll method");
            var annotator = new Annotator();
            annotator.Name = "atr";
            annotator.Stat = Stat.Read;
            annotator.Open("data/100s");
            foreach (var annotation in annotator.ReadAll())
            {
                Console.WriteLine(annotation.ToString());
            }
            annotator.Close();
        }

        public static void UsingAnnotatorClass2()
        {
            Console.WriteLine("exgetann Using Annotator.ReadNext(int count) method, reading 50 annotation.");
            var annotator = new Annotator();
            annotator.Name = "atr";
            annotator.Stat = Stat.Read;
            annotator.Open("data/100s");
            
            foreach (var annotation in annotator.ReadNext(50)) 
            {
                Console.WriteLine(annotation.ToString());
            }

            annotator.Close();
        }

        public static void UsingAnnotatorClass3()
        {
            Console.WriteLine("exgetann Using Annotator.IsEof and Annotator.ReadNext method.");
            var annotator = new Annotator();
            annotator.Name = "atr";
            annotator.Stat = Stat.Read;
            
            annotator.Open("data/100s");

            while (!annotator.IsEof)
            {
                var annotation = annotator.ReadNext();
                Console.WriteLine(annotation);
            }

            Console.WriteLine("Seeking to 00:50:00");
            annotator.Seek(Time.Parse("00:50:00.000"));
            while (!annotator.IsEof)
            {
                var annotation = annotator.ReadNext();
                Console.WriteLine(annotation);
            }

            annotator.Dispose();
        }
    }
}
