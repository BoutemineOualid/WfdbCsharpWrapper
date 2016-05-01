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
    /// A signed integer type (at least 32 bits) used to represent times and time intervals,
    /// in units of sample intervals. Only the magnitude is significant; the sign of a
    /// Time variable indicates how it is to be printed by ToString() or mstimstr.
    /// </summary>
    public struct Time : IComparable<Time>, IEquatable<Time>
    {
        private int value;
        /// <summary>
        /// Creates a new instance from <see cref="Time"/>
        /// </summary>
        /// <param name="value">The value of the created instance.</param>
        public Time(int value)
        {
            this.value = value;
        }

        public static implicit operator Time(int value)
        {
            return new Time(value);
        }

        public static implicit operator int(Time time)
        {
            return time.value;
        }

        /// <summary>
        /// Returns a string representing the current time in the HH:MM:SS format.
        /// </summary>
        /// <remarks>
        /// This method calls the <see cref="PInvoke.timstr"/> native function to perform the underlying conversion task.
        /// </remarks>
        /// <returns>A string that represenets the current time object in the HH:MM:SS format.</returns>
        public override string ToString()
        {
            IntPtr str = PInvoke.timstr(this);
            return Marshal.PtrToStringAnsi(str);
        }

        /// <summary>
        /// Returns a string that represents the current time in the HH:MM:SS.SSS format.
        /// </summary>
        /// <remarks>
        /// This method calls the mstimstr native function to perform this task.
        /// </remarks>
        /// <returns>A string that represenets the current time object in the HH:MM:SS.SSS format.</returns>
        public string ToMSString()
        {
            return Marshal.PtrToStringAnsi(PInvoke.mstimstr(this));
        }

        /// <summary>
        /// Converts a string in standard time format to a valid <see cref="Time"/> object.
        /// </summary>
        /// <param name="time">A string in the HH:MM:SS.SSS Format.</param>
        /// <returns>
        /// A valid time object.
        /// </returns>
        /// <remarks>
        /// The returned value is either
        /// - A positive value: number of sample intervals corresponding to the argument interpreted as a time interval.
        /// - A negative value: (negated) elapsed time in sample intervals from the beginning of the record,
        /// corresponding to the argument interpreted as a time of day.
        /// - Zero 0 : a legal return if the argument matches the base time; otherwise an error return
        /// indicating an incorrectly formatted argument.
        /// </remarks>
        /// <example>
        /// 2:14.875 2 minutes + 14.875 seconds
        /// [13:6:0] 13:06 (1:06 PM)
        /// [8:0:0 1] 8 AM on the day following the base date
        /// [12:0:0 1/3/1992] noon on 1 March 1992
        /// 143 143 seconds (2 minutes + 23 seconds)
        /// 4:02:01 4 hours + 2 minutes + 1 second
        /// s12345 12345 sample intervals
        /// c350.5 counter value 350.5
        /// e time of the end of the record (if defined)
        /// i time of the next sample in input signal 0
        /// o (the letter ‘o’) time of the next sample in output signal 0
        /// </example>
        public static Time Parse(string time)
        {
            return PInvoke.strtim(time);
        }


        /// <summary>
        /// Converts the current time object to the .NET Framework BCL Type <see cref="TimeSpan"/>
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> instance that represents this object.</returns>
        public TimeSpan ToTimeSpan()
        {
            try
            {
                return TimeSpan.Parse(ToMSString());
            }
            catch
            {
                // otherwise, parse it from secondes
                return TimeSpan.FromSeconds(value);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Time)) return false;
            return Equals((Time) obj);
        }

        public bool Equals(Time obj)
        {
            return obj.value == value;
        }

        public int CompareTo(Time other)
        {
            return value.CompareTo(other.value);
        }

        /// <summary>
        /// Const representing the T0 of a Signal.
        /// </summary>
        public static readonly Time Zero = 0;

        /// <summary>
        /// Sets the base time used by time-conversion functions.
        /// </summary>
        /// <param name="time">The base time.</param>
        public static void SetBaseTime(Time time)
        {
            var ret = PInvoke.setbasetime(time.ToString());
            if (ret == -1) 
                throw new InvalidOperationException("Failure: Incorrect time.");
        }

        /// <summary>
        /// Sets the base time used by time-conversion functions.
        /// </summary>
        /// <param name="time">The base time in this format hh:MM:ss.</param>
        public static void SetBaseTime(string time)
        {
            var ret = PInvoke.setbasetime(time);
            if (ret == -1)
                throw new InvalidOperationException("Failure: Incorrect time.");
        }

        public override int GetHashCode()
        {
            return value;
        }

        #region Operator overloads
        public static Time operator +(Time left, Time right)
        {
            return new Time(left.value + right.value);
        }

        public static bool operator ==(Time left, Time right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(Time left, Time right)
        {
            return left.value < right.value;
        }

        public static bool operator >(Time left, Time right)
        {
            return left.value > right.value;
        }
        #endregion
    }
}
