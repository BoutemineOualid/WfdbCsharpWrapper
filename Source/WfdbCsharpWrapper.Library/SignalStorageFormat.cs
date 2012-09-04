/*_______________________________________________________________________________
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

using System;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// Legal values for the <see cref="Signal.Format"/> member.
    /// </summary>
    [Flags]
    public enum SignalStorageFormat
    {
        /// <summary>
        /// Null signal (nothing read or written)
        /// </summary>
        NullSignal = 0,

        /// <summary>
        /// 8-bit first differences
        /// </summary>
        Sf8Bit = 8,

        /// <summary>
        /// 16-bit 2's complement amplitudes, low byte first
        /// </summary>
        Sf16Bit = 16,

        /// <summary>
        /// 16-bit 2's complement amplitudes, high byte first
        /// </summary>
        Sf61Bit = 61,

        /// <summary>
        /// 8-bit offset binary amplitudes
        /// </summary>
        Sf80Bit = 80,

        /// <summary>
        /// 16-bit offset binary amplitudes
        /// </summary>
        Sf160Bit = 160,

        /// <summary>
        /// 2 12-bit amplitudes bit-packed in 3 bytes
        /// </summary>
        Sf212Bit = 212,

        /// <summary>
        /// 3 10-bit amplitudes bit-packed in 4 bytes
        /// </summary>
        Sf310Bit = 310,

        /// <summary>
        /// 3 10-bit amplitudes bit-packed in 4 bytes
        /// </summary>
        Sf311Bit = 311

    }
}
