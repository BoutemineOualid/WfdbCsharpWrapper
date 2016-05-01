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
using System.IO;
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// Holds the name and global attributes of a given signal.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Signal : IComparable<Signal>, IEquatable<Signal>, IEnumerable<Sample>, IDisposable
    {
        #region Fields, any modification will affect the behaviour of the runtime marshaller.
        private IntPtr fileName;

        /// <summary>
        /// Name of the file in which samples of the associated signal are stored.
        /// <remarks>
        /// Input signal files are found by prefixing FileName with
        /// each of the components of the database path in turn.
        /// FileName may include relative or absolute path specifications
        /// if necessary; the use of an absolute pathname, combined with an initial null
        /// component in WFDB, reduces the time needed to find the signal file to a minimum.
        /// If FileName is ‘-’, it refers to the standard input or output.
        /// </remarks>
        /// </summary>
        public String FileName
        {
            get { return Marshal.PtrToStringAnsi(fileName); }
            set { fileName = Marshal.StringToHGlobalAnsi(value); }
        }

        private IntPtr description;

        /// <summary>
        /// Signal's description text.
        /// <remarks>
        /// This is a string without embedded newlines (e.g., ‘ECG lead V1’ or ‘trans-thoracic impedance’). 
        /// The length of the description string is restricted to a maximum of <see cref="MaxDescriptionLength"/> characters,
        /// not including the null.
        /// This field is used internally by the wrapper implementation to fetch the current signal's <see cref="Number"/>. If two or more signals share the same description, 
        /// the first signal in the record gets returned whenever the description string is provided.
        /// </remarks>
        /// </summary>
        public string Description
        {
            get { return Marshal.PtrToStringAnsi(description); }
            set
            {
                if (value != null && value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException(paramName: "value", message: string.Format(
                                                              "The length of description should not exceed {0} characters.",
                                                              MaxDescriptionLength));

                description = Marshal.StringToHGlobalAnsi(value);
            }
        }

        private IntPtr units;

        /// <summary>
        /// Specifies the physical units of the signal; if null, the units are assumed to
        /// be millivolts (mV unless otherwise specified).
        /// <remarks>
        /// The length of the units string is restricted to a maximum of
        /// <see cref="MaxUnitsLength"/> characters (not including the null).
        /// </remarks>
        /// </summary>
        public string Units
        {
            get { return Marshal.PtrToStringAnsi(units); }
            set
            {
                if (value != null && value.Length > MaxUnitsLength)
                    throw new ArgumentOutOfRangeException("Value",
                                                          string.Format(
                                                              "The length of units should not exceed {0} characters.",
                                                              MaxUnitsLength));

                units = Marshal.StringToHGlobalAnsi(value);
            }
        }

        private double gain;
        /// <summary>
        /// The number of analog-to-digital converter units (adus) per physical unit (<see cref="Units"/>)
        /// relative to the original analog signal; for an ECG, this is roughly
        /// equal to the amplitude of a normal QRS complex. If gain is zero, no amplitude
        /// calibration is available; in this case, a gain of <see cref="WfdbCsharpWrapper.Gain.DefaultGain"/>
        /// may be assumed.
        /// </summary>
        public Gain Gain
        {
            get { return gain; }
            set { gain = value; }
        }

        private int initValue;
        /// <summary>
        /// The initial value of the associated signal (i.e., the value of sample number 0).
        /// </summary>
        public Sample InitValue
        {
            get { return initValue; }
            set { initValue = value; }
        }

        private int group;
        /// <summary>
        /// The signal group number. All signals in a given group are stored in the same file.
        /// If there are two or more signals in a group, the file is called a multiplexed signal
        /// file. Group numbers begin at 0; arrays of Signal are always
        /// kept ordered with respect to the group number, so that signals belonging to the
        /// same group are described by consecutive entries in the array.
        /// </summary>
        public Group Group
        {
            get { return group; }
            set { group = value; }
        }

        private SignalStorageFormat format;
        /// <summary>
        /// The signal storage format. 
        /// <remarks>
        /// The most commonly-used formats are format 8 (8-bit
        /// first differences), format 16 (16-bit amplitudes), and format 212 (pairs of 12-bit
        /// amplitudes bit-packed into byte triplets). See <see cref="SignalStorageFormat"/> enumeration for a complete
        /// list of supported formats.
        /// All signals belonging to the same group must be
        /// stored in the same format.
        /// </remarks>
        /// </summary>
        public SignalStorageFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        private int samplesPerFrame;
        /// <summary>
        /// The number of samples per frame. This is 1, for all except oversampled signals
        /// in multi-frequency records, for which spf may be any positive integer.
        /// <remarks>
        /// Note that non-integer values are not permitted (thus the frame rate must be chosen
        /// such that all sampling frequencies used in the record are integer multiples of
        /// the frame rate).
        /// </remarks>
        /// </summary>
        public int SamplesPerFrame
        {
            get { return samplesPerFrame; }
            set { samplesPerFrame = value; }
        }

        private int blockSize;
        /// <summary>
        /// The block size, in bytes.
        /// <remarks>
        /// For signal files that reside on Unix character device
        /// special files (or their equivalents), the BlockSize field indicates how many bytes
        /// must be read or written at a time. For ordinary disk files, BlockSize is zero. 
        /// All signals belonging to a given group have the same BlockSize.
        /// </remarks>
        /// </summary>
        public int BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }

        private int adcResoluation;
        /// <summary>
        /// The ADC resolution in bits.
        /// <remarks>
        /// Typical ADCs have resolutions between 8 and 16
        /// bits inclusive.
        /// </remarks>
        /// </summary>
        public int AdcResolution
        {
            get { return adcResoluation; }
            set { adcResoluation = value; }
        }

        private int adcZero;
        /// <summary>
        /// The ADC output given an input that falls exactly at the center of the ADC
        /// range (normally 0 VDC).
        /// <remarks>
        /// Bipolar ADCs produce two’s complement output; for
        /// these, AdcZero is usually zero. For the MIT DB, however, an offset binary
        /// ADC was used, and AdcZero was 1024.
        /// </remarks>
        /// </summary>
        public int AdcZero
        {
            get { return adcZero; }
            set { adcZero = value; }
        }

        private int baseLine;
        /// <summary>
        /// The value of ADC output that would map to 0 physical units input.
        /// <remarks>
        /// The value of AdcZero is not synonymous with that of Baseline (the isoelectric or physical
        /// zero level of the signal); the Baseline is a characteristic of the signal, while
        /// AdcZero is a characteristic of the digitizer. The value of baseline need not
        /// necessarily lie within the output range of the ADC; for example, if the units
        /// are ‘degrees_Kelvin’, and the ADC range is 200–300 degrees Kelvin, baseline
        /// corresponds to absolute zero, and lies well outside the range of values actually
        /// produced by the ADC.
        /// </remarks>
        /// </summary>
        public int Baseline
        {
            get { return baseLine; }
            set { baseLine = value; }
        }

        private int numberOfSamples;
        /// <summary>
        /// The number of samples in the signal. (Exception: in multi-frequency records,
        /// NumberOfSamples is the number of samples divided by SamplesPerFrame, see above, i.e., the number of
        /// frames.).
        /// <remarks>
        /// All signals in a given record must have the same NumberOfSamples. If NumberOfSamples is
        /// zero, the number of samples is unspecified, and the cksum 
        /// is not used; this is useful for specifying signals that are obtained from pipes,
        /// for which the length may not be known.
        /// </remarks>
        /// </summary>
        public int NumberOfSamples
        {
            get { return numberOfSamples; }
            set { numberOfSamples = value; }
        }

        private int checkSum;
        /// <summary>
        /// A 16-bit checksum of all samples. This field is not usually accessed by application
        /// programs; newheader records checksums calculated by <see cref="PInvoke.putvec"/> when it creates a
        /// new ‘hea’ file, and getvec compares checksums that it calculates against cksum
        /// at the end of the record, provided that the entire record was read through
        /// without skipping samples.
        /// </summary>
        public int CheckSum
        {
            get { return checkSum; }
            set { checkSum = value; }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets the signal number.
        /// </summary>
        /// <remarks>
        /// The current wrapper implementation uses the signal's description to recover the underlying number since it's the only available method to find it (<see cref="PInvoke.findsig"/>).
        /// This implies that the property getter would potentially fail in case two or more signals share the same description.
        /// </remarks>
        public int Number
        {
            get { return PInvoke.findsig(this.Description); }
        }

        /// <summary>
        /// Gets the record to which this signal belongs.
        /// </summary>
        public Record Record
        {
            get { return GetRecord(this); }
        }

        /// <summary>
        /// Gets or sets the intersignal's skew.
        /// </summary>
        public int Skew
        {
            get { return PInvoke.wfdbgetskew((int)this.Number); }
            set { PInvoke.wfdbsetskew((int)this.Number, value); }
        }

        /// <summary>
        /// Returns the current reading pointer's position.
        /// </summary>
        public Time CurrentTime
        {
            get { return GetCurrentTime(this); }
        }

        #endregion

        #region Indexer

        public Sample this[Time t]
        {
            get { return this.ReadNext(t); }
        }

        public bool IsEof
        {
            get { return this.CurrentTime == this.Duration; }
        }

        public Time Duration
        {
            get
            {
                return (int)this.NumberOfSamples;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Moves the reading pointer to the specified position.
        /// </summary>
        /// <remarks>
        /// WFDB lib implements a single global pointer for all available signals in the current record at the same time.
        /// The current wrapper implementation however uses a backing field to keep track of the last position for each signal independently.
        /// This helps when reading data from each signal independently by keeping the state of the record's pointer for each signal independently.
        /// Although it allows the calling code to manipulate each signal's data in isolation, the current implementation will only work in a singlethreaded context.
        /// Concurrent access might result in an unpredictable behaviour.
        /// </remarks>
        /// <param name="t">The new pointer position.</param>
        public void Seek(Time t)
        {
            var ret = PInvoke.tnextvec((int)this.Number, t);
            if (ret == -1)
                throw new InvalidOperationException("EOF reached or improper seek.");

            RegisterSignalTime(this, t);
        }

        /// <summary>
        /// Returns the available sample at the specified position.
        /// </summary>
        /// <param name="t">Position of the sample to be read.</param>
        /// <returns>The available sample at the specified position.</returns>
        public Sample ReadNext(Time t)
        {
            this.Seek(t);
            return this.ReadNext();
        }


        /// <summary>
        /// Returns the specified number of samples starting from the specified position.
        /// </summary>
        /// <param name="from">Reading position.</param>
        /// <param name="count">Number of samples to be read.</param>
        /// <returns>A list containing the available samples starting for the specified position.</returns>
        public IEnumerable<Sample> ReadNext(Time from, int count)
        {
            List<Sample> samples;
            
            try
            {
                Seek(from);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, "from");
            }

            try
            {
                samples = new List<Sample>(Sample.GetSamples(this, count));
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, "count");
            }

            if (samples.Count < count)
            {
                samples.Clear();
                throw new ArgumentOutOfRangeException("count", "The number of remaining samples is less than the specified number");
            }

            RegisterSignalTime(this, this.CurrentTime + count);
            return samples;
        }


        /// <summary>
        /// Returns the Sample at the next pointer location.
        /// </summary>
        /// <returns>Sample at the next pointer location.</returns>
        public Sample ReadNext()
        {
            this.Seek(this.CurrentTime);
            // A row contains n columns where n is the number of available signals in the record holding the provided signal.
            var row = new Sample[this.Record.signals.Count];
            // Read the current row
            var ret = PInvoke.getvec(row);
            if (ret == -1) // EoF
                throw new InvalidOperationException("Failure : EOF reached");
            if (ret == -3)
                throw new NotSupportedException("Unexpected physical end of file.");
            if (ret == -4)
                throw new NotSupportedException("Checksum error.");
            // now save a reference
            row[this.Number].SignalNumber = this.Number;
            RegisterSignalTime(this, this.CurrentTime + 1);
            return row[this.Number];
        }

        /// <summary>
        /// Returns a list containing the specified number of samples available starting from the current pointer location.
        /// </summary>
        /// <param name="count">Number of samples you want to read.</param>
        /// <returns>A list containing the available samples starting for the current pointer location.</returns>
        public IEnumerable<Sample> ReadNext(int count)
        {
            if (IsEof)
                throw new InvalidOperationException("Failure : EOF reached");
            if (count<0)
                throw new ArgumentOutOfRangeException("count", "Please specify a positive value.");
            
            // moving the pointer to the last known position.
            // The current wrapper implementation however uses a backing field to keep track of the last position for each signal independently.
            // This helps when reading data from each signal independently by keeping the state of the record's pointer for each signal independently.
            // Although it allows the calling code to manipulate each signal's data in isolation, the current implementation will only work in a singlethreaded context.
            // Concurrent access might result in an unpredictable behaviour.
            this.Seek(this.CurrentTime);
            var samples = new List<Sample>(Sample.GetSamples(this, count));

            if (samples.Count < count)
            {
                samples.Clear();
                throw new ArgumentOutOfRangeException("count", "the number of remaining samples is less than the specified number");
            }
            
            RegisterSignalTime(this, this.CurrentTime + count);
            return samples;
        }

        /// <summary>
        /// Returns the available samples from the current position to the end of the signal.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sample> ReadToEnd()
        {
            while (!this.IsEof)
            {
                yield return this.ReadNext();
            }
        }

        /// <summary>
        /// Gets the list of all available samples in the current signal
        /// </summary>
        /// <returns>A list containing all the available samples.</returns>
        public IEnumerable<Sample> ReadAll()
        {
            this.Seek(0);
            return ReadToEnd();
        }

        /// <summary>
        /// Gets the available signals in the specified record.
        /// </summary>
        /// <param name="record">The name of the record to be opened.</param>
        /// <returns>A list containing the signals of the specified record.</returns>
        public static IEnumerable<Signal> GetSignals(string record)
        {
            var signalsCount = GetSignalsCount(record);
            if (signalsCount == -1)// no input file
                throw new ArgumentNullException("record", "The specified input file does not exist.");
            else if (signalsCount == -2)
                throw new ArgumentException("Invalid header file.", "record");
            else
            {
                var signals = new Signal[signalsCount];
                PInvoke.isigopen(record, signals, signals.Length);

                var objRecord = new Record(record);
                foreach (var signal in signals)
                {
                    objRecord.signals.Add(signal);
                    if (!RecordCache.ContainsKey(signal))
                        RecordCache.Add(signal, objRecord);
                }
                return new List<Signal>(signals);
            }
        }

        /// <summary>
        /// Returns a list containing the available signals in the specified record.
        /// </summary>
        /// <param name="record">Record to be read.</param>
        /// <returns>List of available signals in the specified record.</returns>
        internal static IEnumerable<Signal> GetSignals(Record record)
        {
            var signalsCount = GetSignalsCount(record);
            if (signalsCount == -1)// no input file
                throw new ArgumentNullException("record", "The specified input file does not exist");
            else if (signalsCount == -2)
                throw new ArgumentException("Invalid header file.", "record");
            else
            {
                var signals = new Signal[signalsCount];
                PInvoke.isigopen(record, signals, signals.Length);

                foreach (var signal in signals)
                {
                    if (!RecordCache.ContainsKey(signal))
                        RecordCache.Add(signal, record);
                }

                return new List<Signal>(signals);
            }

        }

        /// <summary>
        /// Gets the number of available signals in the specified record.
        /// </summary>
        /// <param name="record">The name of the record to be read.</param>
        /// <returns>The number of available signals.</returns>
        public static int GetSignalsCount(string record)
        {
            int signalsCount = 0;
            try
            {
                signalsCount = PInvoke.isigopen(record, null, 0);
            }
            catch (Exception)
            {
                throw new ArgumentNullException("record", "The specified input file does not exist.");
            }

            if (signalsCount == -1) // no input file
                throw new ArgumentNullException("record", "The specified input file does not exist.");
            else if (signalsCount == -2)
                throw new ArgumentException("Invalid header file.", "record");
            return signalsCount;
        }

        internal static Dictionary<Signal, Record> RecordCache = new Dictionary<Signal, Record>();
        /// <summary>
        /// Returns the record associated with the specified signal. For internal use only.
        /// </summary>
        /// <param name="signal">Signal's reference.</param>
        /// <returns>The record instance associated with the specified signal.</returns>
        internal static Record GetRecord(Signal signal)
        {
            if (RecordCache.ContainsKey(signal))
                return RecordCache[signal];
            return null;
        }

        private static Dictionary<Signal, Time>  SignalCurrentTime = new Dictionary<Signal, Time>();
        /// <summary>
        /// Returns the current pointer's position for the specified signal.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private static Time GetCurrentTime(Signal signal)
        {
            if (SignalCurrentTime.ContainsKey(signal))
                return SignalCurrentTime[signal];
            else
            {
                RegisterSignalTime(signal, Time.Zero);
                return Time.Zero;
            }
        }

        private static void RegisterSignalTime(Signal signal, Time time)
        {
            if (SignalCurrentTime.ContainsKey(signal))
            {
                SignalCurrentTime[signal] = time;
            }
            else
            {
                SignalCurrentTime.Add(signal, time);
            }
        }

        internal static void RegisterSignalsTime(Time time)
        {
            var signals = new List<Signal>();
            foreach (var signal in SignalCurrentTime)
            {
                signals.Add(signal.Key);
            }

            SignalCurrentTime.Clear();

            foreach (var signal in signals)
            {
                RegisterSignalTime(signal, time);
            }
        }

        private static void UnregisterSignalTime(Signal signal)
        {
            if (SignalCurrentTime.ContainsKey(signal))
                SignalCurrentTime.Remove(signal);
        }


        #endregion

        #region Overriden Methods
        public int CompareTo(Signal other)
        {
            return this.Description.CompareTo(other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Signal)) return false;
            return Equals((Signal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = fileName.GetHashCode();
                result = (result * 397) ^ description.GetHashCode();
                result = (result * 397) ^ samplesPerFrame;
                result = (result * 397) ^ numberOfSamples.GetHashCode();
                return result;
            }
        }

        public void Dispose()
        {
            RecordCache.Remove(this);
            UnregisterSignalTime(this);
        }

        public override string ToString()
        {
            var result = string.Format("Signal {0}, {1}, {2} samples, {3}", this.Number, this.Description, this.NumberOfSamples, this.Format);
            return result;
        }

        public bool Equals(Signal other)
        {
            return other.fileName.Equals(fileName) && other.description.Equals(description) && other.samplesPerFrame == samplesPerFrame && other.numberOfSamples == numberOfSamples;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ReadAll().GetEnumerator();
        }

        public IEnumerator<Sample> GetEnumerator()
        {
            return this.ReadAll().GetEnumerator();
        }

        #endregion

        #region Operators overloads
        public static bool operator ==(Signal value1, Signal value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Signal value1, Signal value2)
        {
            return !value1.Equals(value2);
        }

        public static bool operator >(Signal value1, Signal value2)
        {
            return value1.Number > value2.Number;
        }

        public static bool operator <(Signal value1, Signal value2)
        {
            return value1.Number < value2.Number;
        }

        public static implicit operator int (Signal value)
        {
            return value.Number;
        }

        #endregion

        #region Consts

        /// <summary>
        /// Maximum allowed characters length of <see cref="Description"/>
        /// </summary>
        public const int MaxDescriptionLength = 60;

        /// <summary>
        /// Maximum allowed characters length of <see cref="Units"/>
        /// </summary>
        public const int MaxUnitsLength = 20;

        #endregion

    }
}