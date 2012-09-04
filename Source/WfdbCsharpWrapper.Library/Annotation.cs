/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2012
 * Contact: boutemine.walid@hotmail.com
 * https://github.com/oualidb/WfdbCsharpWrapper
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
    /// Describes one or more attributes of one or more signals at a given
    /// time.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Annotation : IComparable<Annotation>, IEquatable<Annotation>
    {

        private int time;
        /// <summary>
        /// Gets or sets annotation time, in sample intervals from the beginning of the record.
        /// <remarks>
        /// The times of beat annotations in the ‘atr’ files for the MIT DB generally coincide with
        /// the R-wave peak in signal 0; for the AHA DB, they generally coincide with the
        /// PQ-junction.
        /// </remarks>
        /// </summary>
        public Time Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        private byte type;
        /// <summary>
        /// Gets or sets annotation code; an integer between 1 and <see cref="AnnotationCode.ACMax"/>. 
        /// </summary>
        public AnnotationCode Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        private byte subType;
        /// <summary>
        /// Gets or sets annotation subtype.
        /// </summary>
        public AnnotationCode SubType
        {
            get
            {
                return subType;
            }
            set
            {
                subType = value;
            }
        }

        private byte channelNumber;
        /// <summary>
        /// Gets or sets the channel number
        /// </summary>
        public byte ChannelNumber
        {
            get
            {
                return channelNumber;
            }
            set
            {
                channelNumber = value;
            }
        }

        private byte annotatorNumber;
        /// <summary>
        /// Gets or sets annotator number.
        /// </summary>
        public byte AnnotatorNumber
        {
            get
            {
                return annotatorNumber;
            }
            set
            {
                annotatorNumber = value;
            }
        }

        private IntPtr aux;
        /// <summary>
        /// Gets or sets auxiliary information.
        /// </summary>
        public string Aux
        {
            get
            {
                return Marshal.PtrToStringAnsi(aux);
            }
            set
            {
                aux = Marshal.StringToHGlobalAnsi(value);
            }
        }

        #region Methods

        public static bool operator > (Annotation a1, Annotation a2)
        {
            return a1.Time > a2.Time;
        }

        public static bool operator < (Annotation a1, Annotation a2)
        {
            return a1.Time < a2.Time;
        }

        public static bool operator ==(Annotation ann1, Annotation ann2)
        {
            return ann1.Equals(ann2);
        }

        public static bool operator != (Annotation ann1, Annotation ann2)
        {
            return !ann1.Equals(ann2);
        }

        public int CompareTo(Annotation other)
        {
            return this.Time.CompareTo(other.Time);
        }

        public bool Equals(Annotation other)
        {
            return other.time == time && other.type == type && other.annotatorNumber == annotatorNumber;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}, {2}", this.Time.ToMSString(), this.Type.ToString(), this.Type.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Annotation)) return false;
            return Equals((Annotation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = time;
                result = (result*397) ^ type.GetHashCode();
                result = (result*397) ^ annotatorNumber.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}