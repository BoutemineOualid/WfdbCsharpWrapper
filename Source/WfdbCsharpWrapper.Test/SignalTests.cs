/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2012
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
using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace WfdbCsharpWrapper.Test
{
    [TestFixture]
    public class SignalTests
    {
        [TearDown]
        public void FreeResources()
        {
            Wfdb.Quit();
        }

        [Test]
        public void GetSignalsCountNoInputFileTest()
        {
            try
            {
                Signal.GetSignalsCount(null);
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }

            try
            {
                Signal.GetSignalsCount(string.Empty);
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }

            try
            {
                Signal.GetSignalsCount("some inexistent file");
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }
            Wfdb.Quit();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSignalsCountInvalidHeaderTest()
        {
            Signal.GetSignalsCount("data/invalid");
        }

        [Test]
        public void GetSignalsCountTestValidDataTest()
        {
            var count = Signal.GetSignalsCount("data/100s");
            Assert.AreEqual(2, count);
            Wfdb.Quit();
        }

        [Test]
        public void GetSignalsNoInputFileTest()
        {
            try
            {
                Signal.GetSignals(null);
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }

            try
            {
                Signal.GetSignals(string.Empty);
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }

            try
            {
                Signal.GetSignals("some inexistent file");
                Assert.Fail("ArgumentNullException should have been thrown.");
            }
            catch (ArgumentNullException)
            {
            }
            Wfdb.Quit();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSignalsInvalidHeaderTest()
        {
            Signal.GetSignals("data/invalid");
        }

        [Test]
        public void GetSignalsValidDataTest()
        {
            // Dealing with real data.
            var s = Signal.GetSignals("data/100s");

            Assert.AreEqual(2, s.Count);

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

            Assert.AreEqual(1024, s[0].AdcZero);
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

            Assert.AreEqual(s[0].SamplesPerFrame, s[1].SamplesPerFrame);
            Assert.IsTrue(s[0].Record.Equals(s[1].Record));

            Assert.IsTrue(s[0].Record.Signals[0].Equals(s[0]));
            Assert.IsTrue(s[0].Record.Signals[1].Equals(s[1]));

            Assert.IsTrue(s[1].Record.Signals[0].Equals(s[0]));
            Assert.IsTrue(s[1].Record.Signals[1].Equals(s[1]));

            Wfdb.Quit();
        }

        [Test]
        public void DescriptionTest()
        {
            var s = new Signal();
            s.Description = null;

            var validDescription = string.Empty;
            var random = new Random(1);
            for (int i = 0; i < Signal.MaxDescriptionLength; i++)
            {
                validDescription +=(char) random.Next(1, 255);
            }
            
            try
            {
                s.Description = validDescription;
            }
            catch (Exception)
            {
                Assert.Fail("Exception should not have been thrown.");
            }
            
            var invalidDescription = validDescription + (char)random.Next(1,255);
            try
            {
                s.Description = invalidDescription;
                Assert.Fail("ArgumentException should have been thrown.");
            }
            catch (ArgumentException)
            {
            }
            Wfdb.Quit();
        }

        [Test]
        public void UnitsTest()
        {
            var s = new Signal();
            s.Description = null;

            var validUnits = string.Empty;
            var random = new Random(1);
            for (int i = 0; i < Signal.MaxUnitsLength; i++)
            {
                validUnits += (char)random.Next(1, 255);
            }

            try
            {
                s.Description = validUnits;
            }
            catch (Exception)
            {
                Assert.Fail("Exception should not have been thrown.");
            }

            var invalidUnits = validUnits + (char)random.Next(1, 255);
            try
            {
                s.Units = invalidUnits;
                Assert.Fail("ArgumentException should have been thrown.");
            }
            catch (ArgumentException)
            {
            }

            Wfdb.Quit();
        }

        [Test]
        public void NumberTest()
        {
            var signals = Signal.GetSignals("data/100s");
            Assert.AreEqual(0, signals[0].Number);
            Assert.AreEqual(1, signals[1].Number);
        }

        [Test]
        public void RecordTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                Assert.IsTrue(r.Signals[0].Record == r);
                Assert.IsTrue(r.Signals[1].Record == r);
            }

            var signals = Signal.GetSignals("data/100s");
            Assert.IsTrue(signals[0].Record == signals[1].Record);
        }

        [Test]
        public void IsEofTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);

                signal.ReadNext();
                Assert.IsFalse(signal.IsEof);

                signal.ReadNext((int)signal.NumberOfSamples / 2);
                Assert.IsFalse(signal.IsEof);

                signal.ReadAll();
                Assert.IsTrue(signal.IsEof);

                signal.Seek(0);
                Assert.IsFalse(signal.IsEof);

                signal.Seek((int)signal.NumberOfSamples - 1);
                Assert.IsFalse(signal.IsEof);

                signal.ReadNext();
                Assert.IsTrue(signal.IsEof);
            }
        }

        [Test]
        public void ReadNextTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);

                Assert.AreEqual(signal.InitValue, signal.ReadNext());
                Assert.IsFalse(signal.IsEof);

                signal.Seek(signal.Duration - 1);
                Assert.IsFalse(signal.IsEof);
                
                signal.ReadNext();
                Assert.IsTrue(signal.IsEof);

                try
                {
                    signal.ReadNext();
                    Assert.Fail("InvalidOperationException should have been thrown, we reached the end of the signal.");
                }
                catch (InvalidOperationException)
                {
                }

                signal.Seek(0);
                var samples = new List<Sample>();
                while (!signal.IsEof)
                {
                    samples.Add(signal.ReadNext());
                }

                Assert.IsTrue(signal.IsEof);
                Assert.AreEqual(signal.ReadAll(), samples);
            }
        }

        [Test]
        public void ReadAllTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);

                Assert.AreEqual(signal.NumberOfSamples, signal.ReadAll().Count);
                Assert.IsTrue(signal.IsEof);

                // Recall
                Assert.AreEqual(signal.NumberOfSamples, signal.ReadAll().Count);
                Assert.IsTrue(signal.IsEof);
            }
        }

        [Test]
        public void ReadNextCountTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();
                var signal = r[0];
                var expectedSamples = signal.ReadAll();

                signal.Seek(0);

                Assert.AreEqual(new List<Sample>(), signal.ReadNext(0));

                
                var samples = signal.ReadNext((int)signal.NumberOfSamples); // or simply signal.Duration
                Assert.AreEqual(expectedSamples, samples);

                // we reached the end of signal.
                try
                {
                    signal.ReadNext(101); // some mandatory number
                    Assert.Fail("InvalidOperationException should have been thrown.");
                }
                catch (InvalidOperationException)
                {
                }

                signal.Seek(signal.Duration/2);

                try
                {
                    signal.ReadNext((int) signal.NumberOfSamples);
                    Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
                }
                catch (ArgumentOutOfRangeException)
                {
                }

                try
                {
                    signal.ReadNext(-5);
                    Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
        }

        [Test]
        public void ReadNextTimeTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);
                var expectedSamples = signal.ReadAll();
                
                var expectedFirstSample = expectedSamples[0];
                Assert.AreEqual(expectedFirstSample, signal.ReadNext(Time.Zero));
                Assert.AreEqual(signal.InitValue, signal.ReadNext(Time.Zero));

                var expectedLastSample = expectedSamples[expectedSamples.Count - 1];
                Time lastSampleTime = signal.Duration - 1;
                
                Assert.AreEqual(expectedLastSample, signal.ReadNext(lastSampleTime));

                // EOF REACHED
                try
                {
                    signal.ReadNext(signal.Duration + 5);
                    Assert.Fail("InvalidOperationException should have been thrown cause the end of signal has been reached.");
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        [Test]
        public void ReadNextTimeCountTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                var expectedSamples = signal.ReadAll();

                // lower bound
                var expectedFirstSample = expectedSamples[0];
                Assert.AreEqual(expectedFirstSample, signal.ReadNext(Time.Zero, 1)[0]);
                Assert.AreEqual(signal.InitValue, signal.ReadNext(Time.Zero, 1)[0]);

                // Upper bound
                var expectedLastSample = expectedSamples[expectedSamples.Count - 1];
                Time lastSampleTime = signal.Duration - 1;

                Assert.AreEqual(expectedLastSample, signal.ReadNext(lastSampleTime, 1)[0]);

                var samples = signal.ReadNext(0, (int)signal.NumberOfSamples);
                Assert.AreEqual(expectedSamples, samples);
                Assert.IsTrue(signal.IsEof);

                samples = signal.ReadNext(0, (int)signal.NumberOfSamples / 2);
                
                expectedSamples.RemoveRange((int)signal.NumberOfSamples / 2, (int)signal.NumberOfSamples / 2);

                Assert.AreEqual(expectedSamples, samples);
                Assert.IsFalse(signal.IsEof);

                try
                {
                    signal.ReadNext(signal.Duration / 2, (int)signal.NumberOfSamples);
                    Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
                }
                catch (ArgumentOutOfRangeException)
                {
                }

                try
                {
                    signal.ReadNext(signal.Duration + 5, (int)signal.NumberOfSamples);
                    Assert.Fail("ArgumentException should have been thrown.");
                }
                catch (ArgumentException)
                {
                }

                try
                {
                    signal.ReadNext(signal.Duration / 2, -5);
                    Assert.Fail("ArgumentException should have been thrown.");
                }
                catch (ArgumentException)
                {
                }
            }
        }

        [Test]
        public void ReadToEndTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                var expectedSamples = signal.ReadAll();
                
                signal.Seek(0);

                Assert.AreEqual(expectedSamples, signal.ReadToEnd());
                Assert.IsTrue(signal.IsEof);

                Assert.AreEqual(new List<Sample>(), signal.ReadToEnd()); // no more samples to read
                
                signal.Seek(signal.Duration/2);
                var samples = signal.ReadToEnd();
                expectedSamples.RemoveRange(0, signal.Duration/2);
                Assert.AreEqual(expectedSamples, samples);
            }
        }

        [Test]
        public void SeekTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);
                var expectedSamples = signal.ReadAll();
                var expectedFirstSample = expectedSamples[0];
                var expectedLastSample = expectedSamples[expectedSamples.Count - 1];

                signal.Seek(0);
                Assert.IsFalse(signal.IsEof);
                Assert.AreEqual(expectedFirstSample, signal.ReadNext());

                signal.Seek(signal.Duration - 1);
                Assert.AreEqual(expectedLastSample, signal.ReadNext());

                Assert.IsTrue(signal.IsEof);

                signal.Seek(signal.Duration / 2);

                var restOfSamples = signal.ReadToEnd();
                expectedSamples.RemoveRange(0, signal.Duration / 2);
                Assert.AreEqual(expectedSamples, restOfSamples);
            }
        }

        [Test]
        public void IndexerTest()
        {
            using (var r = new Record("data/100s"))
            {
                r.Open();

                var signal = r.Signals[0];
                Assert.IsFalse(signal.IsEof);
                var expectedSamples = signal.ReadAll();
                for (int i = 0; i < expectedSamples.Count; i++)
                {
                    Assert.AreEqual(expectedSamples[i], signal[i]);
                }

                try
                {
                    var sample = signal[signal.Duration + 5];
                    Assert.Fail("InvalidOperationException should have been thrown.");
                }
                catch (InvalidOperationException)
                {
                }
            }
        }
    }
}