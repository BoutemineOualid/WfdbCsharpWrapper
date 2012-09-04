﻿/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2012
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

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// Holds the supported values for the <see cref="CalibrationType"/> Property
    /// <remarks>
    /// <see cref="AcCoupled"/> and <see cref="DcCoupled"/> are used in combination with the pulse
    /// shape definitions in this enumeration to characterize calibration pulses.
    /// </remarks>
    /// </summary>
    public enum CalibrationType
    {
        /// <summary>
        /// AC coupled signal
        /// </summary>
        AcCoupled = 0,

        /// <summary>
        /// DC coupled signal
        /// </summary>
        DcCoupled = 1,

        /// <summary>
        /// Square wave pulse
        /// </summary>
        CalSquare = 2,

        /// <summary>
        /// Sine wave pulse
        /// </summary>
        CalSine = 4,

        /// <summary>
        /// Sawtooth pulse
        /// </summary>
        CalSawtooth = 6,

        /// <summary>
        /// Undefined pulse shape
        /// </summary>
        CalUndef = 8
    }
}
