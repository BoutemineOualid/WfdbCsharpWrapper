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
using System.Collections.Generic;
using System.Diagnostics;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// A signed integer type used to represent sample values, in units
    /// of adus.
    /// </summary>
    public struct Sample : IComparable<Sample>, IEquatable<Sample>, IDisposable
    {
        private int adu;
        /// <summary>
        /// Gets the integer value of this sample.
        /// </summary>
        public int Adu
        {
            get { return adu; }
            private set { this.adu = value; }
        }

        /// <summary>
        /// Gets the Signal Number of the Current sample.
        /// </summary>
        public int SignalNumber 
        {
            get { return GetSignalNumber(this); }
            internal set { RegisterSignalNumber(this, value); }
        }


        public Sample(int adu)
        {
            this.adu = adu;
        }

        #region Signal Number Management

        private static Dictionary<Sample, int> SignalNumbers = new Dictionary<Sample, int>();
        private static int GetSignalNumber(Sample sample)
        {
            if (SignalNumbers.ContainsKey(sample))
                return SignalNumbers[sample];
            else
                return -1;
        }

        private static void RegisterSignalNumber(Sample sample, int signalNumber)
        {
            if (SignalNumbers.ContainsKey(sample))
                SignalNumbers[sample] = signalNumber;
            else
                SignalNumbers.Add(sample,signalNumber);
        }

        private static void UnregisterSignalNumber(Sample sample)
        {
            if (SignalNumbers.ContainsKey(sample))
                SignalNumbers.Remove(sample);
        }

        #endregion

        #region Methods


        /// <summary>
        /// Converts the specified physio unit to adus. 
        /// </summary>
        /// <param name="signal">The signal object associated with the resulted sample.</param>
        /// <param name="physUnit">The physiological unit.</param>
        /// <returns>Sample object holding a reference for the converted value.</returns>
        public static Sample ToSample(Signal signal, double physUnit)
        {
            Sample sample = PInvoke.physadu((int)signal.Number, physUnit);
            sample.SignalNumber = signal.Number;
            return sample;
        }

        /// <summary>
        /// Converts the specified microvolts unit to adus. 
        /// </summary>
        /// <param name="signal">The signal object associated with the resulted sample.</param>
        /// <param name="microvolts">The microvolts unit.</param>
        /// <returns>Sample object holding a reference for the converted value.</returns>
        public static Sample ToSample(Signal signal, int microvolts)
        {
            Sample sample = PInvoke.muvadu((int)signal.Number, microvolts);
            sample.SignalNumber = signal.Number;
            return sample;
        }

        /// <summary>
        /// Converts the current adus unit to physiological unit.
        /// </summary>
        /// <returns>Returns the corresponding physiological unit.</returns>
        public double ToPhys()
        {
            return PInvoke.aduphys((int)this.SignalNumber, this);
        }

        /// <summary>
        /// Converts the current adus unit to microvolts.
        /// </summary>
        /// <returns></returns>
        public int ToMicrovolts()
        {
            return PInvoke.adumuv((int)this.SignalNumber, this);
        }

        public int CompareTo(Sample other)
        {
            return this.Adu.CompareTo(other.Adu);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Sample)) return false;
            return Equals((Sample)obj);
        }

        public override int GetHashCode()
        {
            return Adu;
        }

        public bool Equals(Sample other)
        {
            return other.Adu == Adu;
        }

        public override string ToString()
        {
            return Adu.ToString();
        }

        /// <summary>
        /// Returns all available samples in the specified signal.
        /// </summary>
        /// <param name="signal">Source Signal.</param>
        /// <returns>A list containing all available samples.</returns>
        public static IEnumerable<Sample> GetSamples(Signal signal)
        {
            int ret = 0;
            signal.Seek(Time.Zero);
            for (int i = 0; i < signal.NumberOfSamples; i++)
            {
                // A row contains n columns where n is the number of available signals in the record holding the provided signal.
                var row = new Sample[signal.Record.signals.Count];
                // Read the current row
                ret = PInvoke.getvec(row);
                if (ret == -1) // end of data
                    yield break;
                else if (ret == -3)
                    throw new NotSupportedException("Unexpected physical end of file.");
                else if (ret == -4)
                    throw new NotSupportedException("Checksum error.");
                
                // now save a reference
                row[signal.Number].SignalNumber = signal.Number;
                yield return row[signal.Number];
            }
        }

        /// <summary>
        /// Returns the specified number of samples from the available signals at the same time.
        /// </summary>
        /// <param name="numberOfSamples">Number of samples to be read.</param>
        /// <param name="signalsCount">Number of available signal in the current record.</param>
        /// <returns>A list of sample vectors where each entry holds an array containing the available samples at the current pointer in each signal.</returns>
        public static IEnumerable<Sample[]> GetSamples(int numberOfSamples, int signalsCount)
        {
            int ret = 0;
            for (int i = 0; i < numberOfSamples; i++)
            {
                var samples = new Sample[signalsCount];
                // Read the current row
                ret = PInvoke.getvec(samples);
                if (ret == -1) // end of data
                    yield break;
                else if (ret == -3)
                    throw new NotSupportedException("Unexpected physical end of file.");
                else if (ret == -4)
                    throw new NotSupportedException("Checksum error.");
                for (int j = 0; j < signalsCount; j++)
                {
                    samples[j].SignalNumber = j;
                }
                yield return samples;
            }
        }

        /// <summary>
        /// Returns the specified number of samples from the specified signal.
        /// </summary>
        /// <param name="signal">Source Signal.</param>
        /// <param name="numberOfSamples">Number of samples to read.</param>
        /// <returns>A list containing <paramref name="numberOfSamples"/> samples.</returns>
        public static IEnumerable<Sample> GetSamples(Signal signal, int numberOfSamples)
        {
            if (numberOfSamples < 0)
                throw new ArgumentException("Positive value expected.", "numberOfSamples");

            int ret = 0;
            for (int i = 0; i < numberOfSamples; i++)
            {
                // A row contains n columns where n is the number of available signals in the record holding the provided signal.
                var row = new Sample[signal.Record.signals.Count];
                // Read the current row
                ret = PInvoke.getvec(row);
                if (ret == -1) // end of data
                    yield break;
                else if (ret == -3)
                    throw new NotSupportedException("Unexpected physical end of file.");
                else if (ret == -4)
                    throw new NotSupportedException("Checksum error.");
                // now save a reference
                row[signal.Number].SignalNumber = signal.Number;
                yield return row[signal.Number];
            }
        }

        #endregion

        #region Operator overloads
        public static implicit operator Sample(int adu)
        {
            return new Sample { Adu = adu };
        }

        public static implicit operator int(Sample sample)
        {
            return sample.Adu;
        }

        public static bool operator ==(Sample s1, Sample s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(Sample s1, Sample s2)
        {
            return !s1.Equals(s2);
        }

        public static bool operator >(Sample s1, Sample s2)
        {
            return s1.Adu > s2.Adu;
        }

        public static bool operator <(Sample s1, Sample s2)
        {
            return s1.Adu < s2.Adu;
        }

        #endregion

        /// <summary>
        /// Samples from getvec or getframe with this value are not valid.
        /// </summary>
        public const int InvalidSampleAduValue = -32768;

        public void Dispose()
        {
            UnregisterSignalNumber(this);
        }
    }
}
