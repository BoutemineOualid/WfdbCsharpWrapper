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
using System;
using System.Linq;
using NUnit.Framework;

namespace WfdbCsharpWrapper.Test
{
    [TestFixture]
    class RecordTests
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Wfdb.Quit();
        }

        [Test]
        public void InfoTest()
        {
            using (var record = new Record("data/100s"))
            {
                record.Open();
                Assert.AreNotEqual(null, record.Info);
                Assert.AreEqual(" 69 M 1085 1629 x1\n Aldomet, Inderal\nProduced by xform from record 100, beginning at 0:0", record.Info);
            }
        }

        [Test]
        public void SignalsTest()
        {
            var record = new Record("data/100s");
            using (record)
            {
                record.Open();
                Assert.AreEqual(2, record.Signals.Count());
                Assert.AreEqual("100s.dat", record.Signals.ToList()[0].FileName);
                Assert.AreEqual("100s.dat", record.Signals.ToList()[1].FileName);

                Assert.AreEqual("MLII", record.Signals.ToList()[0].Description);
                Assert.AreEqual("V5", record.Signals.ToList()[1].Description);

                Assert.AreEqual(21600, record.Signals.ToList()[0].NumberOfSamples);
                Assert.AreEqual(21600, record.Signals.ToList()[1].NumberOfSamples);

                Assert.AreEqual(SignalStorageFormat.Sf212Bit, record.Signals.ToList()[0].Format);
                Assert.AreEqual(SignalStorageFormat.Sf212Bit, record.Signals.ToList()[1].Format);

                Assert.AreEqual(1, record.Signals.ToList()[0].SamplesPerFrame);
                Assert.AreEqual(1, record.Signals.ToList()[1].SamplesPerFrame);

                Assert.AreEqual(Gain.DefaultGain, record.Signals.ToList()[0].Gain);
                Assert.AreEqual(Gain.DefaultGain, record.Signals.ToList()[1].Gain);

                Assert.AreEqual(200.0, record.Signals.ToList()[0].Gain.Value);
                Assert.AreEqual(200.0, record.Signals.ToList()[1].Gain.Value);

                Assert.AreEqual(11, record.Signals.ToList()[0].AdcResolution);
                Assert.AreEqual(11, record.Signals.ToList()[1].AdcResolution);

                Assert.AreEqual(1024, record.Signals.ToList()[0].AdcZero);
                Assert.AreEqual(1024, record.Signals.ToList()[1].AdcZero);

                Assert.AreEqual(1024, record.Signals.ToList()[0].Baseline);
                Assert.AreEqual(1024, record.Signals.ToList()[1].Baseline);

                Assert.AreEqual(995, record.Signals.ToList()[0].InitValue);
                Assert.AreEqual(1011, record.Signals.ToList()[1].InitValue);

                Assert.AreEqual(0, record.Signals.ToList()[0].Group);
                Assert.AreEqual(0, record.Signals.ToList()[1].Group);

                Assert.AreEqual(0, record.Signals.ToList()[0].BlockSize);
                Assert.AreEqual(0, record.Signals.ToList()[1].BlockSize);

                Assert.AreEqual(21537, record.Signals.ToList()[0].CheckSum);
                Assert.AreEqual(-3962, record.Signals.ToList()[1].CheckSum);

            }
        }

        [Test]
        public void DisposeTest()
        {
            var record = new Record("data/100s");

            record.Open();
            Assert.IsFalse(record.IsNew);
            //Assert.AreEqual(" 69 M 1085 1629 x1", record.Info);
            Assert.AreEqual((Frequency)360, record.SamplingFrequency);
            Assert.AreEqual(2, record.Signals.Count());
            
            record.Dispose();

            Assert.AreEqual(0, record.Signals.Count());
            Assert.IsTrue(record.IsNew);
            Assert.AreEqual(string.Empty, record.Info);
            Assert.AreEqual((Frequency)0, record.SamplingFrequency);
            Assert.AreEqual(0, record.Signals.Count());
        }

        [Test]
        public void IndexerTest()
        {
            var record = new Record("data/100s");
            using (record)
            {
                record.Open();
                Assert.AreEqual(record.Signals.ToList()[0], record[0]);
                Assert.AreEqual(record.Signals.ToList()[1], record[1]);
            }

            try
            {
                var signal = record[0];
                Assert.Fail("IndexOutOfRangeException should have been thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [Test]
        public void OpenTest()
        {
            var record = new Record("data/100s");
            using (record)
            {
                record.Open();
                Assert.IsFalse(record.IsNew);
                Assert.AreEqual(" 69 M 1085 1629 x1\n Aldomet, Inderal\nProduced by xform from record 100, beginning at 0:0", record.Info);
                Assert.AreEqual((Frequency)360, record.SamplingFrequency);
                Assert.AreEqual(2, record.Signals.Count());
            }

            Assert.AreEqual(0, record.Signals.Count());
            Assert.IsTrue(record.IsNew);
            Assert.AreEqual(string.Empty, record.Info);
            Assert.AreEqual((Frequency)0, record.SamplingFrequency);
            Assert.AreEqual(0, record.Signals.Count());


            using (record)
            {
                record.Open();
                Assert.IsFalse(record.IsNew);
                Assert.AreEqual(" 69 M 1085 1629 x1\n Aldomet, Inderal\nProduced by xform from record 100, beginning at 0:0", record.Info);
                Assert.AreEqual((Frequency)360, record.SamplingFrequency);
                Assert.AreEqual(2, record.Signals.Count());
            }
        }

        [Test]
        public void SamplingFrequencyTest()
        {
            var record = new Record("data/100s");
            using (record)
            {
                record.Open();
                Assert.AreEqual((Frequency)360, record.SamplingFrequency); // Get test
                record.SamplingFrequency = 320; // set test
                Assert.AreEqual((Frequency)320, record.SamplingFrequency); // Get test
            }
        }

        [Test]
        public void SeekTest()
        {
            // Testing On Signal's Samples
            using (var record = new Record("data/100s"))
            {
                record.Open();
                var expectedSignal1Samples0 = record[0].ReadNext(10); // read 10 samples of the first 10
                var expectedSignal2Samples0 = record[1].ReadNext(10); // read 10 samples of the first 10

                record.Signals.First().Seek(10); // advance the 1st pointer by ten samples
                var expectedSignal1Samples10 = record[0].ReadNext(10); // read 10 samples after the first 10 samples

                record.Signals.Skip(1).First().Seek(10); // advance the 1st pointer by ten samples
                var expectedSignal2Samples10 = record[1].ReadNext(10); // read 10 samples after the first 10 samples

                record.Seek(0);
                Assert.AreEqual(expectedSignal1Samples0, record[0].ReadNext(10));
                Assert.AreEqual(expectedSignal2Samples0, record[1].ReadNext(10));

                record.Seek(10);
                Assert.AreEqual(expectedSignal1Samples10, record[0].ReadNext(10));
                Assert.AreEqual(expectedSignal2Samples10, record[1].ReadNext(10));
            }
        }

        [Test]
        public void GetSamplesTest()
        {
        }

        [Test]
        public void SetGroupTime()
        {

        }
    }
}