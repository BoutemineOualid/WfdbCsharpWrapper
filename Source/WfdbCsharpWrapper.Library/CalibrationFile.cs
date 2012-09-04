/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2010
 * Contact: boutemine.walid@hotmail.com
 * Project web page: wfdbcsharpwrapper.codeplex.com
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
    /// Contains functions used to determine specifications for calibration pulses and
    /// customary scales for plotting signals. All of them make use of the calibration list, which is
    /// maintained in memory and which contains entries for various types of signals.
    /// </summary>
    public static class CalibrationFile
    {

        #region Methods
        /// <summary>
        /// Reads the specified calibration file into the calibration
        /// list.
        /// </summary>
        /// <param name="fileName">
        /// Calibration file name (which must be located in one of the directories specified by <see cref="Wfdb.WfdbPath"/>
        /// into the calibration list. 
        /// </param>
        /// <remarks>
        /// If fileName is NULL, the file named by <see cref="Wfdb.WfdbCal"/> is read. Normally, the current contents of
        /// the calibration list are discarded before reading the calibration file; if file begins with ‘+’,
        /// however, the ‘+’ is stripped from the file name and the contents of the file are appended to
        /// the current calibration list. If file is ‘-’, calopen reads the standard input rather than a
        /// calibration file. (This function was first introduced in WFDB library version 6.0.)
        /// </remarks>
        public static void Open(string fileName)
        {
            var ret = PInvoke.calopen(fileName);
            if (ret == -1)
                throw new Exception("Insufficient memory for calibration list");
            if (ret == -2)
                throw new Exception("Unable to open calibration file");
        }

        /// <summary>
        /// Creates a new calibration file (in the current directory) containing the contents
        /// of the calibration list (which is not modified).
        /// </summary>
        /// <param name="fileName">
        /// The file name of the new created calibration file.
        /// </param>
        /// <remarks>
        /// fileName must satisfy the standard conditions for a WFDB file name, i.e., it may contain letters, digits, or underscores. (This function was
        /// first introduced in WFDB library version 6.0.)
        /// </remarks>
        public static void CreateNew(string fileName)
        {
            int ret = PInvoke.newcal(fileName);

            if (ret == -1)
                throw new Exception("Unable to open file");
        }

        #endregion
    }
}
