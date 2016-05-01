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

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// A signed integer type (at least 32 bits) used to represent Julian dates, in units
    /// of days.
    /// </summary>
    public struct Date : IComparable<Date>, IEquatable<Date>
    {
        private int value;

        /// <summary>
        /// Creates a new instance from <see cref="Date"/>
        /// </summary>
        /// <param name="value">The value of the created instance.</param>
        public Date(int value)
        {
            this.value = value;
        }


        #region Methods
        /// <summary>
        /// Returns a string that represents the current date in the DD/MM/YYYY format.
        /// </summary>
        /// <remarks>
        /// This method calls the <see cref="PInvoke.datstr"/> native function to perform this task.
        /// </remarks>
        /// <returns>A string that represents the current time object in the DD/MM/YYYY Format</returns>
        public override string ToString()
        {
            IntPtr str = PInvoke.datstr(this);
            return Marshal.PtrToStringAnsi(str);
        }

        /// <summary>
        /// Converts string into a Julian date
        /// </summary>
        /// <param name="date">A string in DD/MM/YYYY Format.</param>
        /// <remarks>
        /// This method calls the datstr native function to perform this task.
        /// </remarks>
        /// <returns>An equivalent Date object.</returns>
        public static Date Parse(string date)
        {
            return PInvoke.strdat(date);
        }

        /// <summary>
        /// Converts the current date object to the .NET Framework BCL Type <see cref="DateTime"/>
        /// </summary>
        /// <returns>A <see cref="DateTime"/> instance representing this object.</returns>
        public DateTime ToDateTime()
        {
            return DateTime.Parse(ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Date)) return false;
            return Equals((Date)obj);
        }

        public bool Equals(Date obj)
        {
            return obj.value == value;
        }

        public int CompareTo(Date other)
        {
            return value.CompareTo(other.value);
        }

        public override int GetHashCode()
        {
            return value;
        }

        #endregion

        #region Operator overloads
        public static bool operator ==(Date left, Date right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Date left, Date right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(Date left, Date right)
        {
            return left.value > right.value;
        }

        public static bool operator <(Date left, Date right)
        {
            return left.value < right.value;
        }

        public static implicit operator Date(int value)
        {
            return new Date(value);
        }

        public static implicit operator int(Date date)
        {
            return date.value;
        }
        #endregion
    }
}
