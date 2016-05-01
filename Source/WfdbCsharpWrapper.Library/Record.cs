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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// 
    /// </summary>
    public class Record : IEnumerable<Signal>, IComparable<Record>, IEquatable<Record>, IDisposable
    {
        public Record(string name)
        {
            this.IsNew = true;
            this.Name = name;
        }

        #region Properties

        private string name = string.Empty;
        /// <summary>
        /// Gets the name of the record.
        /// </summary>
        /// <remarks>
        /// You may qualify the name with the full/relative path on the hard disk. 
        /// </remarks>
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
            }
        }

        private static object syncLock = new object();
        /// <summary>
        /// Gets the signals available in this record.
        /// You should call <seealso cref="Open"/> before using this member.
        /// </summary>
        internal List<Signal> signals = new List<Signal>();
        /// <summary>
        /// Gets the signals associated with this record.
        /// </summary>
        public IEnumerable<Signal> Signals
        {
            get
            {
                return signals;
            }
        }

        private string info = string.Empty;
        /// <summary>
        /// Gets or sets the info associated with this record.
        /// </summary>
        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        /// <summary>
        /// Gets a Boolean value indicating whether this record is new or not.
        /// </summary>
        public bool IsNew { get; private set; }

        /// <summary>
        /// Gets or sets the sampling frequency for this record.
        /// </summary>
        public Frequency SamplingFrequency { get; set; }
        #endregion

        #region Indexer
        /// <summary>
        /// Returns the signal at the specified index.
        /// </summary>
        /// <param name="i">Signal's index</param>
        /// <returns>Signal object at the specified index.</returns>
        public Signal this[int i]
        {
            get
            {
                if (i >= this.signals.Count)
                    throw new ArgumentOutOfRangeException("i");
                return this.signals[i];
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the specified number of available samples from the available signals at the same time.
        /// </summary>
        /// <param name="numberOfSamples">Number of samples to be returned</param>
        /// <returns>A list of sample vectors where each entry holds the a vector containing the samples available at the current pointer in each signal.</returns>
        public List<Sample[]> GetSamples(int numberOfSamples)
        {
            return new List<Sample[]>(Sample.GetSamples(numberOfSamples, this.signals.Count));
        }

        /// <summary>
        /// Resets the signal input file pointers so that the next samples returned by <see cref="Signal.ReadNext(int)"/> and <see cref="Record.GetSamples"/> 
        /// </summary>
        /// <param name="t">The new pointer position.</param>
        public void Seek(Time t)
        {
            var ret = PInvoke.isigsettime(t);
            if (ret == -1)
            {
                throw new InvalidOperationException("EOF reached or improper seek.");
            }
            else
            {
                Signal.RegisterSignalsTime(t);
            }
        }

        /// <summary>
        /// Sets the current time (pointer) for the specified group.
        /// </summary>
        /// <param name="group">The group's number</param>
        /// <param name="t">The new time.</param>
        public static void SetGroupTime(int group, Time t)
        {
            var ret = PInvoke.isgsettime(group, t);
            if (ret == -1)
                throw new InvalidOperationException("EOF reached or improper seek.");
            else if (ret == -2)
                throw new InvalidOperationException("Incorrect signal group number specified.");
        }

        /// <summary>
        /// Opens the current record and fetches its information from the hard disk.
        /// </summary>
        public void Open()
        {
            lock (syncLock)
            {
                try
                {
                    this.signals = new List<Signal>(Signal.GetSignals(this));
                    
                    // Loading the info string
                    this.Info = string.Empty;

                    IntPtr infoPtr = PInvoke.getinfo(this.Name);
                    while (true)
                    {
                        this.Info += Marshal.PtrToStringAnsi(infoPtr) + "\n";
                        infoPtr = PInvoke.getinfo(null);
                        if (infoPtr == IntPtr.Zero)
                            break;
                    }
                    
                    this.Info = this.Info.TrimEnd('\n');

                    this.SamplingFrequency = Frequency.GetFrequency(this.Name);
                    this.IsNew = false;
                }
                catch (Exception)
                {
                    this.signals = new List<Signal>();
                    this.Info = string.Empty;
                    throw;
                }
            }
        }

        #endregion

        #region Static and overriden Methods

        public IEnumerator<Signal> GetEnumerator()
        {
            return this.Signals.GetEnumerator();
        }

        public int CompareTo(Record other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(Record other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Name == Name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Dispose()
        {
            foreach (var signal in Signals)
            {
                signal.Dispose();
            }

            this.signals.Clear();
            this.Info = string.Empty;
            this.SamplingFrequency = 0;
            this.IsNew = true;
            Wfdb.Quit();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(Record value1, Record value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Record value1, Record value2)
        {
            return !value1.Equals(value2);
        }

        public static implicit operator string(Record value)
        {
            return value.Name;
        }

        public static implicit operator Record(string value)
        {
            return new Record(value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Record)) return false;
            return Equals((Record)obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }

        #endregion

        #region Consts
        /// <summary>
        /// Gets the maximum allowed length of record name.
        /// </summary>
        public const int MaxRecordNameLength = 20;

        #endregion
    }
}
