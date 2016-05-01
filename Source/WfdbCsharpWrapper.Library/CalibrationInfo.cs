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
    /// Holds calibration specifications for signals of a given type.
    /// </summary>
    public struct CalibrationInfo
    {
        #region Properties

        private string signalType;
        /// <summary>
        /// Gets or sets a string (without embedded tabs or newlines) that describes the type(s) of signals to which the calibration specifications apply.
        /// <remarks>
        /// Usually, <see cref="SignalType"/> is an exact match to (or a prefix of) the <see cref="Signal.Description"/> field of the
        /// <see cref="Signal"/> object that describes a matching signal.
        /// </remarks>
        /// </summary>
        public string SignalType
        {
            get
            {
                return signalType;
            }
            set
            {
                signalType = value;
            }
        }

        private string units;
        /// <summary>
        /// Gets or sets a string without embedded whitespace that specifies the physical units 
        /// of signals to which the calibration specifications apply. 
        /// <remarks>
        /// Usually, the units field of a 
        /// <see cref="CalibrationInfo"/> structure must exactly match
        /// the <see cref="Signal.Units"/> field of the <see cref="Signal"/> structure that describes a matching signal.
        /// </remarks>
        /// </summary>
        public string Units
        {
            get
            {
                return units;
            }
            set
            {
                units = value;
            }
        }

        private double scale;
        /// <summary>
        /// Gets or sets the customary plotting scale, in physical units per centimeter. 
        /// <remarks>
        /// WFDB applications that produce graphical output may use scale as a default. Except
        /// in unusual circumstances, signals of different types should be plotted at equal
        /// multiples of their respective scales.
        /// </remarks>
        /// </summary>
        public double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        private double low;
        /// <summary>
        /// Gets or sets the value (in physical units) corresponding to the low level of a calibration
        /// pulse. 
        /// <remarks>
        /// If the signal is AC-coupled, low is zero, and high is the pulse amplitude.
        /// </remarks>
        /// </summary>
        public double Low
        {
            get
            {
                return low;
            }
            set
            {
                low = value;
            }
        }

        private double high;
        /// <summary>
        /// Gets or sets the values (in physical units) corresponding to high level of a calibration
        /// pulse. 
        /// <remarks>
        /// If the signal is AC-coupled, low is zero, and high is the pulse amplitude.
        /// </remarks>
        /// </summary>
        public double High
        {
            get
            {
                return high;
            }
            set
            {
                high = value;
            }
        }


        private CalibrationType calibrationType;
        /// <summary>
        /// Gets or sets a value that specifies the shape of the calibration pulse.
        /// <remarks>
        /// Type is even if signals of the corresponding <see cref="SignalType"/> 
        /// are AC-coupled, and odd if they are DC-coupled.
        /// </remarks>
        /// </summary>
        public CalibrationType CalibrationType
        {
            get
            {
                return calibrationType;
            }
            set
            {
                calibrationType = value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// This function attempts to find calibration data for signals of type description, having physical
        /// units as given by units. 
        /// </summary>
        /// <param name="description">signal's description</param>
        /// <param name="units">physical units</param>
        /// <returns>
        /// If successful, it fills in the contents of the CalibrationInfo structure.
        /// </returns>
        /// <remarks>
        /// Caller must not modify the contents of the strings addressed by the <see cref="SignalType"/> and <see cref="Units"/> fields of the CalibrationInfo structure
        /// after GetCalibration returns. GetCalibration returns data from the first entry in the calibration list that
        /// contains a <see cref="SignalType"/> field that is either an exact match or a prefix of description, and a <see cref="Units"/>
        /// field that is an exact match of units; if either description or units is NULL, however, it is ignored
        /// for the purpose of finding a match. GetCalibration cannot succeed unless the calibration list has
        /// been initialized by a previous invocation of <see cref="Open"/> or <see cref="PutCalibration"/>. 
        /// (This function was first introduced in WFDB library version 6.0.)
        /// </remarks>
        public static CalibrationInfo? GetCalibration(string description, string units)
        {
            var cal = new CalibrationInfo();
            var ret = PInvoke.getcal(description, units, ref cal);
            if (ret == -1) // no match found
                return null;
            return cal;
        }

        /// <summary>
        /// This function adds the given CalibrationInfo structure pointed to by the argument to the end of the calibration
        /// list.
        /// </summary>
        /// <remarks>
        /// This function was first introduced in WFDB library version 6.0.
        /// </remarks>
        /// <param name="calibration">The calibration data to put in the calibration list.</param>
        /// <returns>A value indicating whether or not the operation was successful.</returns>
        public static bool PutCalibration(CalibrationInfo calibration)
        {
            int ret = PInvoke.putcal(ref calibration);
            if (ret == -1)
                return false;
            return true;
        }

        /// <summary>
        /// Discards the current calibration list and returns the memory that it occupied to the heap. 
        /// </summary>
        /// <remarks>
        /// Note that <see cref="Wfdb.Quit"/> does not perform the function of FlushCalibrationList. 
        /// This function was first introduced in WFDB library version 6.0.
        /// </remarks>
        public static void FlushCalibrationList()
        {
            PInvoke.flushcal();
        }

        #endregion
    }
}
