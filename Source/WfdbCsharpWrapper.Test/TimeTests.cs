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

using NUnit.Framework;

namespace WfdbCsharpWrapper.Test
{
    /// <summary>
    /// A set of tests for <see cref="Time"/> datatype and related functions
    /// </summary>
    [TestFixture]
    public class TimeTests
    {
        /// <summary>
        /// A test for <see cref="Time.ToString"/>
        /// </summary>
        [Test]
        public void ToStringTest()
        {
            // TODO, Test for negative values
            Time t1 = 5;

            // timstr, mstimstr tests
            string expectedTimResult = "0:05";
            string timResult = t1.ToString().Trim();
            Assert.AreEqual(expectedTimResult, timResult);

        }

        /// <summary>
        /// A test for <see cref="Time.ToMSString"/>
        /// </summary>
        [Test]
        public void ToMSStringTest()
        {
            // TODO, Test for negative values
            Time t1 = 5;

            string expectedMsTimResult = "0:05.000";
            string msTimResult = t1.ToMSString().Trim();
            Assert.AreEqual(expectedMsTimResult, msTimResult);

        }

        /// <summary>
        /// A test for <see cref="Time.Parse"/>
        /// </summary>
        [Test]
        public void ParseTest()
        {
            // TODO, Test for negative values
            Time expected = 5;

            string str = expected.ToString();
            string msStr = expected.ToMSString();

            // strtim test
            Time t1 = Time.Parse(str);
            Assert.AreEqual(t1, expected);

            long t2 = Time.Parse(msStr);
            Assert.AreEqual(t2, expected);
        }

        /// <summary>
        /// A test for <see cref="Time.ToTimeSpan"/>
        /// </summary>
        [Test]
        public void ToTimeSpanTest()
        {
            Time t = 5;
            var ts = t.ToTimeSpan();
            Assert.AreEqual(5, ts.Seconds);
        }

        /// <summary>
        /// A test for <see cref="Time.Equals"/> and equality operators.
        /// </summary>
        [Test]
        public void EqualityTest()
        {
            Time t1 = 5;
            Time t2 = 5;

            // Equality operators
            Assert.IsTrue(t1 != null);
            Assert.IsTrue(t2 != null);
            
            Assert.IsTrue(t1 == t2);
            Assert.IsFalse(t1 != t2);
            // Equals methods
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t2.Equals(t1));

            Assert.AreEqual(t1, t2); // Equals(object, object)
            t1 = 6;
            t2 = 5;

            // Equality operators
            Assert.IsTrue(t1 != t2);
            Assert.IsFalse(t1 == t2);

            // Equals methods
            Assert.IsFalse(t1.Equals(t2));
            Assert.IsFalse(t2.Equals(t1));

        }

        /// <summary>
        /// A test for <see cref="Time.CompareTo"/> and greater than and less than operator
        /// </summary>
        [Test]
        public void ComparabilityTest()
        {
            // Comparability
            // Operators
            Time t1 = 5;
            Time t2 = 6;

            // < operator
            Assert.IsTrue(t1 < t2);
            Assert.IsFalse(t2 < t1);

            // > operator
            Assert.IsTrue(t2 > t1);
            Assert.IsFalse(t1 > t2);
            
            // CompareTo
            Assert.IsTrue(t1.CompareTo(t2) < 0);
            Assert.IsTrue(t2.CompareTo(t1) > 0);

            t1 = 5;
            t2 = 5;

            // < operator
            Assert.IsFalse(t1 < t2);
            Assert.IsFalse(t2 < t1);

            // > operator
            Assert.IsFalse(t2 > t1);
            Assert.IsFalse(t1 > t2);

            // CompareTo
            Assert.AreEqual(0, t1.CompareTo(t2));
            Assert.AreEqual(0, t2.CompareTo(t1));

        }

        [Test]
        public void PlusMinusTests()
        {
            Time t1 = 1 * 20 * 60 + 5; // 1 hour and 20 minutes and 5 seconds
            Time t2 = 0; // 6 seconds

            Time result = t1 + t2; // 11 seconds
            Assert.AreEqual(t1, result);

            result = t1 + 5; // + 5 seconds so the result should be 1 hour and 20 minutes and 10 seconds

            Assert.AreEqual(1 * 20 * 60 + 5 + 5, result);

            // minus test
            result -= 5;
            Assert.AreEqual(t1, result);

        }
    }
}