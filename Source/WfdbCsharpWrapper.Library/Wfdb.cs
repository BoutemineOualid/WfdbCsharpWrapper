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
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    public static class Wfdb
    {
        /// <summary>
        /// Closes all open WFDB files and frees any memory allocated by other WFDB
        /// library functions.
        /// </summary>
        /// <remarks>
        /// This method also resets the following:
        /// - The factors used for converting between samples, seconds, and counter values (reset to
        /// 1), the base time (reset to 0, i.e., midnight), and the base counter value (reset to 0).
        /// - The parameters used for converting between adus and physical units.
        /// - Internal variables used to determine output signal specifications.
        /// </remarks>
        public static void Quit()
        {
            PInvoke.wfdbquit();
        }

        public static void Init(string record, Annotator[] annotators, Signal[] signals)
        {
            var ret = PInvoke.wfdbinit(record, annotators, (int)annotators.Length, signals, (int)signals.Length);
            if (ret < 0)
            {
                switch (ret)
                {
                    case -1:
                        throw new InvalidOperationException("Failure: Unable to read header file.");
                    case -2:
                        throw new InvalidOperationException("Failure: Incorrect header file format.");
                    case -3:
                        throw new InvalidOperationException("Failure: Unable to open input annotation file.");
                    case -4:
                        throw new InvalidOperationException("Failure: Unable to open output annotation file.");
                    case -5:
                        throw new InvalidOperationException("Failure: illegal Stat specified for annotation file.");
                }
            }
        }

        /// <summary>
        /// Enable Verbose Mode.
        /// </summary>
        public static void EnableErrorReporting()
        {
            PInvoke.wfdbverbose();
        }

        /// <summary>
        /// Disable Verbose Mode.
        /// </summary>
        public static void DisableErrorReporting()
        {
            PInvoke.wfdbquiet();
        }

        /// <summary>
        /// Write pending changes to hard disk.
        /// </summary>
        public static void Flush()
        {
            PInvoke.wfdbflush();
        }

        /// <summary>
        /// Gets the version of invoked WFDB library.
        /// </summary>
        public static string Version
        {
            get
            {
                return Marshal.PtrToStringAnsi(PInvoke.wfdbversion());
            }
        }

        /// <summary>
        /// Gets or sets the value of the WFDB environment variable.
        /// <remarks>
        /// The string contains a list of locations where input files may be found. These
        /// locations may be absolute directory names (such as ‘/usr/local/database’ under Unix,
        /// or ‘d:/database’ under MS-DOS), relative directory names (e.g., ../mydata), or URL prefixes
        /// (e.g., ‘http://www.physionet.org/physiobank/database’).
        /// 
        /// If NETFILES support is unavailable, any URL prefixes in the string are ignored. The special form ‘.’ refers to
        /// the current directory. Entries in the list may be separated by whitespace or by semicolons;
        /// under Unix, colons may also be used as separators. An empty component, indicated by an
        /// initial or terminal separator, or by two consecutive separators, will be understood to specify
        /// the current directory (which may also be indicated by a component consisting of a single
        /// ‘.’). If the string is empty or NULL, the database path is limited to the current directory.
        /// 
        /// If string begins with ‘@’, the remaining characters of string are taken as the name of a file
        /// from which the WFDB path is to be read. This file may contain either the WFDB path, as
        /// described in the previous paragraph, or another indirect WFDB path specification. Indirect
        /// WFDB path specifications may be nested no more than ten levels deep (an arbitrary limit
        /// imposed to avoid infinite recursion). Evaluation of indirect WFDB paths is deferred until
        /// getwfdb is invoked, either explicitly or by the WFDB library while attempting to open
        /// an input file (e.g., using annopen or isigopen). (The features described in this paragraph
        /// were first introduced in WFDB library version 8.0.) See [getwfdb], page 59 for an example
        /// of the use of setwfdb.
        /// </remarks>
        /// </summary>
        public static string WfdbPath
        {
            get
            {

                return Marshal.PtrToStringAnsi(PInvoke.getwfdb());
                // same as 
                // return Marshal.PtrToStringAnsi(PInvoke.wfdbdefwfdb());
            }
            set
            {
                PInvoke.setwfdb(value);
            }
        }

        /// <summary>
        /// Gets the value of the WFDBCAL environment variable.
        /// </summary>
        public static string WfdbCal
        {
            get
            {
                return Marshal.PtrToStringAnsi(PInvoke.wfdbdefwfdbcal());
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the current Samples Per Frame Rate.
        /// </summary>
        public static int SamplesPerFrame
        {
            get { return PInvoke.getspf(); }
        }

        /// <summary>
        /// Returns a string containing the most recent WFDB error message.
        /// </summary>
        public static string LastErrorMessage
        {
            get { return Marshal.PtrToStringAnsi(PInvoke.wfdberror()); }
        }

        /// <summary>
        /// Sets the current GV mode.
        /// </summary>
        /// <param name="mode">new GV mode.</param>
        public static void SetGVMode(GVMode mode)
        {
            PInvoke.setgvmode((int)mode);
        }

    }
}