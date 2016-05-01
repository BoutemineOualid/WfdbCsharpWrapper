/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Oualid BOUTEMINE, 2009-2016
 * Contact: boutemine.walid@hotmail.com
 * Project web page: https://github.com/oualidb/WfdbCsharpWrapper
 * wfdb: 
 * -----
 * a library for reading and writing annotated waveforms (time series data)
 * Copyright (C) 1999 George B. Moody
 *
 * This library is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Library General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option) any
 * later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
 * PARTICULAR PURPOSE.  See the GNU Library General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Library General Public License along
 * with this library; if not, write to the Free Software Foundation, Inc., 59
 * Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 *
 * You may contact the author by e-mail (george@mit.edu) or postal mail
 * (MIT Room E25-505A, Cambridge, MA 02139 USA).  For updates to this software,
 * please visit PhysioNet (http://www.physionet.org/).
 * _______________________________________________________________________________
 */
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace WfdbCsharpWrapper.Test
{
    /// <summary>
    /// A set of tests for wrapped functions of <see cref="PInvoke"/>
    /// </summary>
    [TestFixture]
    public class PInvokeTests
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            PInvoke.wfdbquit();
        }

        /// <summary>
        /// A test for <see cref="PInvoke.wfdbquit"/>
        /// </summary>
        [Test]
        [Explicit("To be tested later")]
        public void wfdbquitTest()
        {
            // TODO
        }

        /// <summary>
        /// A test for <see cref="PInvoke.timstr"/>, <see cref="PInvoke.mstimstr"/>, <see cref="PInvoke.strtim"/> functions.
        /// </summary>
        [Test]
        public void strtim_timstr_mstimstrTest()
        {
            // TODO, Test for negative values
            int t1 = 5;

            // timstr, mstimstr tests
            string expectedTimResult = "0:05";
            string timResult = Marshal.PtrToStringAnsi(PInvoke.timstr(t1)).Trim();
            Assert.AreEqual(expectedTimResult, timResult);
            
            string expectedMsTimResult = "0:05.000";
            string msTimResult = Marshal.PtrToStringAnsi(PInvoke.mstimstr(t1)).Trim();
            Assert.AreEqual(expectedMsTimResult, msTimResult);

            // strtim test
            int strTimResult1 = PInvoke.strtim(timResult);
            Assert.AreEqual(strTimResult1, t1);
            
            int strTimResult2 = PInvoke.strtim(timResult);
            Assert.AreEqual(strTimResult2, t1);
        }

        [Test]
        public void isigopenTest()
        {

            var ret = PInvoke.isigopen(string.Empty, null, 0);
            Assert.AreEqual(ret, -1); // no input file

            // Testing for invalid header file.
            string file = "data/invalid";
            ret = PInvoke.isigopen(file, null, 0);
            Assert.IsTrue(ret == -2);

            // Testing for real data
            file = "data/100s";
            ret = PInvoke.isigopen(file, null, 1); // Number of signals > 1
            Assert.IsTrue(ret == 0);

            // Two signals in the record, this syntax will return the number of available signals
            var numberOfSignals = PInvoke.isigopen(file, null, 0);
            Assert.AreEqual(numberOfSignals, 2);

            
            // Dealing with real data.
            var s = new Signal[numberOfSignals];
            ret = PInvoke.isigopen(file, s, 2);

            Assert.AreEqual(ret, 2);

            // Check for results : Signal 100s

            Assert.AreEqual("100s.dat", s[0].FileName);
            Assert.AreEqual("100s.dat", s[1].FileName);

            Assert.AreEqual("MLII", s[0].Description);
            Assert.AreEqual("V5", s[1].Description);

            Assert.AreEqual(21600, s[0].NumberOfSamples);
            Assert.AreEqual(21600, s[1].NumberOfSamples);

            Assert.AreEqual(SignalStorageFormat.Sf212Bit, s[0].Format);
            Assert.AreEqual(SignalStorageFormat.Sf212Bit, s[1].Format);

            Assert.AreEqual(1, s[0].SamplesPerFrame);
            Assert.AreEqual(1, s[1].SamplesPerFrame);

            Assert.AreEqual(Gain.DefaultGain, s[0].Gain);
            Assert.AreEqual(Gain.DefaultGain, s[1].Gain);

            Assert.AreEqual(200.0, s[0].Gain.Value);
            Assert.AreEqual(200.0, s[1].Gain.Value);

            Assert.AreEqual(11, s[0].AdcResolution);
            Assert.AreEqual(11, s[1].AdcResolution);

            Assert.AreEqual(1024 , s[0].AdcZero);
            Assert.AreEqual(1024, s[1].AdcZero);

            Assert.AreEqual(1024, s[0].Baseline);
            Assert.AreEqual(1024, s[1].Baseline);

            Assert.AreEqual(995, s[0].InitValue);
            Assert.AreEqual(1011, s[1].InitValue);

            Assert.AreEqual(0, s[0].Group);
            Assert.AreEqual(0, s[1].Group);

            Assert.AreEqual(0, s[0].BlockSize);
            Assert.AreEqual(0, s[1].BlockSize);

            Assert.AreEqual(21537, s[0].CheckSum);
            Assert.AreEqual(-3962, s[1].CheckSum);

            PInvoke.wfdbquit();
        }

        [Test]
        public void getvecTest()
        {
            var signals = new Signal[2];
            PInvoke.isigopen("data/100s", signals, 2);
            var samples = new Sample[2];
            var ret = PInvoke.getvec(samples);
            Assert.AreEqual(2, ret);

            // Verifying data
            Assert.AreEqual(signals[0].InitValue, samples[0]);
            Assert.AreEqual(signals[1].InitValue, samples[1]);
        }

        [Test]
        public void getinfoTest()
        {
            var info = Marshal.PtrToStringAnsi(PInvoke.getinfo("data/100s"));
            Assert.IsFalse(string.IsNullOrEmpty(info));
            Assert.AreEqual(" 69 M 1085 1629 x1", info); // just the first line, to get the hole text, we need to iterate until the return value is null.
            info = Marshal.PtrToStringAnsi(PInvoke.getinfo(null));
            Assert.AreEqual(" Aldomet, Inderal", info);
            info = Marshal.PtrToStringAnsi(PInvoke.getinfo(null));
            Assert.AreEqual("Produced by xform from record 100, beginning at 0:0", info);
            info = Marshal.PtrToStringAnsi(PInvoke.getinfo(null));
            Assert.AreEqual(null, info);
        }

        [Test]
        public void sampfreqTest()
        {
            var ret = PInvoke.sampfreq("some inexistent file");
            Assert.AreEqual(-1d, ret); // unable to read the header file

            ret = PInvoke.sampfreq("data/invalid");
            Assert.AreEqual(-2d, ret); // invalid header file

            Frequency fre = PInvoke.sampfreq("data/100s");
            Assert.AreEqual(360, fre.Value);
            PInvoke.wfdbquit();
        }
    }
}