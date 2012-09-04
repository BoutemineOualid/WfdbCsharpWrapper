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

namespace WfdbCsharpWrapper.Test
{
    [TestFixture]
    public class FrequencyTests
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Wfdb.Quit();
        }

        [Test]
        public void GetFrequencyTest()
        {
            Frequency freq;
            try
            {
                freq = Frequency.GetFrequency("data/invalid");
                Assert.Fail("ArgumentException should have been thrown.");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                freq = Frequency.GetFrequency("inexistent file");
                Assert.Fail("ArgumentException should have been thrown.");
            }
            catch (ArgumentException)
            {
            }

            freq = Frequency.GetFrequency("data/100s");
            Assert.IsTrue(freq == 360);
            Wfdb.Quit();
        }

        [Test]
        public void InputFrequencyTest()
        {
            // Before opening any record
            Assert.IsTrue(Frequency.InputFrequency == 0);

            Frequency.InputFrequency = 320; // positive
            Assert.IsTrue(Frequency.InputFrequency == 0);
            

            // Opening some record
            using (Record r = new Record("data/100s"))
            {
                r.Open();
                Frequency.InputFrequency = 320;
                Assert.IsTrue(Frequency.InputFrequency == 320);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeFrequencyTest()
        {
            Frequency.InputFrequency = -320; // negative
            Assert.IsTrue(Frequency.InputFrequency == 0);
        }
    }
}