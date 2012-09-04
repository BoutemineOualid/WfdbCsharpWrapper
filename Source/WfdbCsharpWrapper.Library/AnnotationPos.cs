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

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// These may be used by applications which plot
    /// signals and annotations to determine where to print annotation mnemonics.
    /// </summary>
    public enum AnnotationPos
    {
        /// <summary>
        /// Undefined annotation types 
        /// </summary>
        APUndef = 0,

        /// <summary>
        /// Standard position 
        /// </summary>
        APStd = 1,

        /// <summary>
        /// A level above <see cref="APStd"/>
        /// </summary>
        APHigh = 2,

        /// <summary>
        /// A level below <see cref="APStd"/>
        /// </summary>
        APLow = 3,

        /// <summary>
        /// Attached to the signal specified by `chan' 
        /// </summary>
        APAtt = 4,

        /// <summary>
        /// A level above <see cref="APAtt"/>
        /// </summary>
        APAHigh = 5,

        /// <summary>
        /// A level below <see cref="APAtt"/>
        /// </summary>
        APALow = 6,
    }
}
