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

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// A floating point type used to represent sampling and counter frequencies, in
    /// units of Hz.
    /// </summary>
    public struct Frequency
    {

        public Frequency(double value)
        {
            if (value<0)
                throw new ArgumentException("Invalid value, please use positive values.", "value");
            this.value = value;
        }

        public static implicit operator Frequency(double value)
        {
            return new Frequency(value);
        }

        public static implicit operator double(Frequency frequency)
        {
            return frequency.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// Gets the sampling frequency of the specified record.
        /// </summary>
        /// <param name="record">The record name.</param>
        /// <returns>The sampling frequency associated with the specified record.</returns>
        public static Frequency GetFrequency(string record)
        {
            var ret = PInvoke.sampfreq(record);
            if (ret == -1)
                throw new ArgumentException("Unable to read header file.", "record");
            else if (ret == -2)
                throw new ArgumentException("Incorrect header format.", "record");
            return ret;
        }

        private double value;

        public double Value
        {
            get { return value; }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the current input sampling frequency (in samples per second per signal)
        /// </summary>
        /// <remarks>
        /// The current input sampling frequency is either the raw sampling frequency for 
        /// the current record or a frequency chosen for this property.
        /// </remarks>
        public static Frequency InputFrequency
        {
            get
            {
                return PInvoke.getifreq();
            }
            set
            {
                PInvoke.setifreq(value);
            }
        }

        /// <summary>
        /// Sets the sampling frequency used by time conversion functions.
        /// </summary>
        /// <remarks>
        /// Use SetSamplingFrequency before creating a new .hea file.
        /// </remarks>
        /// <param name="freq"></param>
        public static void SetSamplingFrequency(Frequency freq)
        {
            var ret = PInvoke.setsampfreq(freq);
            if (ret == -1)
            {
                throw new InvalidOperationException(
                    "Failure: Illegal sampling frequency specified(freq must not be negative.");
            }
        }

        /// <summary>
        /// Gets or sets the current wfdb's output annotation frequency.
        /// </summary>
        public static Frequency OutputAnnotationFrequency
        {
            get { return PInvoke.getafreq(); }
            set
            {
                PInvoke.setafreq(value);
            }
        }
        #endregion
    }
}