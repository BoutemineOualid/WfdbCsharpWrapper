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
    /// A set of tests for <see cref="Date"/>
    /// </summary>
    [TestFixture]
    public class DateTests
    {
        /// <summary>
        /// A test for <see cref="Date.ToString"/>
        /// </summary>
        [Test]
        public void ToStringTest()
        {
            string expectedDateResult = "01/01/2010";
            Date d1 = Date.Parse(expectedDateResult);

            string dateResult = d1.ToString().Trim();
            Assert.AreEqual(expectedDateResult, dateResult);
        }

        /// <summary>
        /// A test for <see cref="Date.Parse"/>
        /// </summary>
        [Test]
        public void ParseTest()
        {
            string expectedDateResult = "01/01/2010";
            Date expected = Date.Parse(expectedDateResult);

            string str = expected.ToString();

            Date t1 = Date.Parse(str);
            Assert.AreEqual(expected, t1);

        }

        /// <summary>
        /// A test for <see cref="Date.ToDateTime"/>
        /// </summary>
        [Test]
        public void ToDateTimeTest()
        {
            Date t = Date.Parse("01/01/2010");
            var ts = t.ToDateTime();
            string expected = t.ToString().Trim();
            string result = ts.ToString("dd/MM/yyyy");
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// A test for <see cref="Date.Equals"/> and equality operators.
        /// </summary>
        [Test]
        public void EqualityTest()
        {
            Date d1 = 5;
            Date d2 = 5;

            // Equality operators
            Assert.IsTrue(d1 != null);
            Assert.IsTrue(d2 != null);

            Assert.IsTrue(d1 == d2);
            Assert.IsFalse(d1 != d2);
            // Equals methods
            Assert.IsTrue(d1.Equals(d2));
            Assert.IsTrue(d2.Equals(d1));

            Assert.AreEqual(d1, d2); // Equals(object, object)
            d1 = 6;
            d2 = 5;

            // Equality operators
            Assert.IsTrue(d1 != d2);
            Assert.IsFalse(d1 == d2);

            // Equals methods
            Assert.IsFalse(d1.Equals(d2));
            Assert.IsFalse(d2.Equals(d1));

        }

        /// <summary>
        /// A test for <see cref="Date.CompareTo"/> and greater than and less than operator
        /// </summary>
        [Test]
        public void ComparabilityTest()
        {
            // Comparability
            // Operators
            Date d1 = 5;
            Date d2 = 6;

            // < operator
            Assert.IsTrue(d1 < d2);
            Assert.IsFalse(d2 < d1);

            // > operator
            Assert.IsTrue(d2 > d1);
            Assert.IsFalse(d1 > d2);

            // CompareTo
            Assert.IsTrue(d1.CompareTo(d2) < 0);
            Assert.IsTrue(d2.CompareTo(d1) > 0);

            d1 = 5;
            d2 = 5;

            // < operator
            Assert.IsFalse(d1 < d2);
            Assert.IsFalse(d2 < d1);


            // > operator
            Assert.IsFalse(d2 > d1);
            Assert.IsFalse(d1 > d2);

            // CompareTo
            Assert.AreEqual(0, d1.CompareTo(d2));
            Assert.AreEqual(0, d2.CompareTo(d1));

        }
    }
}
