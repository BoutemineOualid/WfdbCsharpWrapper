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
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// 
    /// </summary>
    public struct Annotator : IEquatable<Annotator>, IDisposable
    {
        #region Properties
        private IntPtr name;
        /// <summary>
        /// Gets or sets the annotator's name.
        /// <remarks>
        /// The name ‘atr’ is reserved for a reference annotation
        /// file supplied by the creator of the database record to document its contents as
        /// accurately and thoroughly as possible. You may use other annotator names to
        /// identify annotation files that you create; unless there are compelling reasons not
        /// to do so, follow the convention that the annotator name is the name of the file’s
        /// creator (a program or a person). To avoid confusion, do not use ‘dat’, ‘datan’,
        /// ‘dn’, or ‘hea’ (all of which are commonly used as parts of WFDB file names) as
        /// annotator names. The special name ‘-’ refers to the standard input or output.
        /// Other annotator names may contain upper- or lower-case letters, digits, and
        /// underscores. Annotation files are normally created in the current directory and
        /// found in any of the directories in the database path.
        /// </remarks>
        /// </summary>
        public string Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(name);
            }
            set
            {
                name = Marshal.StringToHGlobalAnsi(value);
            }
        }

        private Stat stat;
        /// <summary>
        /// Gets or sets the file type/access code. 
        /// <remarks>
        /// Usually, Stat is either <see cref="WfdbCsharpWrapper.Stat.Read"/> or 
        /// <see cref="WfdbCsharpWrapper.Stat.Write"/> to specify standard (“WFDB format”) annotation files to be read by getann
        /// or to be written by putann. Both MIT DB and AHA DB annotation files can
        /// be (and generally are) stored in WFDB format. An AHA-format annotation file
        /// can be read by getann or written by putann if the Stat field is set to <see cref="WfdbCsharpWrapper.Stat.AhaRead"/>
        /// or <see cref="WfdbCsharpWrapper.Stat.AhaWrite"/> before calling annopen or wfdbinit.
        /// </remarks>
        /// </summary>
        public Stat Stat
        {
            get
            {
                return stat;
            }
            set
            {
                stat = value;
            }
        }
        /// <summary>
        /// Gets or sets an unsigned integer type used to represent annotator numbers.
        /// </summary>
        public int Number
        {
            get { return GetAnnotatorNumber(this); }
        }

        /// <summary>
        /// Gets a value indicating whether the end of the annotation file has been reached or not.
        /// </summary>
        public bool IsEof
        {
            get
            {
                if (!IsOpen)
                    throw new NotSupportedException("Annotator is not open.");
                try
                {
                    var next = this.ReadNext();
                    PInvoke.ungetann((int)this.Number, ref next);
                    return false;
                }
                catch (Exception ex)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether this annotator is open or not.
        /// </summary>
        public bool IsOpen
        { 
            get { return inputAnnotators.Contains(this) || outputAnnotators.Contains(this); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the annotator associated with the specified record for read access.
        /// </summary>
        /// <param name="record">The name of the record to be opened.</param>
        /// <param name="keepOldOpen">Specifies whether or not you want to keep the already opened annotators in memory.</param>
        public void Open(string record, bool keepOldOpen)
        {
            if (IsOpen)
                throw new InvalidOperationException("Annotator already opened.");
            var keepOldAnnotatorsStr = record;
            if (keepOldOpen)
            {
                if (!record.StartsWith("+") )
                    keepOldAnnotatorsStr = string.Format("+{0}", record);
            }
            else
            {
                Annotator.CloseAll();
            }

            var ret = PInvoke.annopen(keepOldAnnotatorsStr, ref this, 1);

            if (ret == -3)
                throw new InvalidOperationException("Failure: Unable to open input annotation file.");
            else if (ret == -4)
                throw new InvalidOperationException("Failure: Unable to open output annotation file.");
            else if (ret == -5)
                throw new InvalidOperationException("Failure: Illegal Stat specified for annotation file.");
            RegisterAnnotator(this);
        }

        /// <summary>
        /// Opens the current annotator associated with the specified record.
        /// </summary>
        public void Open(string record)
        {
            Open(record, true);
        }

        /// <summary>
        /// Moves the reading pointer within the Annotator file to the specified position.
        /// </summary>
        /// <param name="t">Seek position.</param>
        public void Seek(Time t)
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");

            var ret = PInvoke.iannsettime(t);

            if (ret == -1)
                throw new InvalidOperationException("Failure: EOF Reached or improper seek.");
            else if (ret == -3)
                throw new InvalidOperationException("Failure: Unexpected physical end of file.");
        }

        /// <summary>
        /// Moves the reading pointer by <paramref name="count"/> annotations starting from the current position.
        /// </summary>
        /// <param name="count">Number of annotations to skip.</param>
        public void Seek(int count)
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "please specify a positive value.");

            new List<Annotation>(this.ReadNext(count)).Clear(); // force a loop
        }

        /// <summary>
        /// Closes the current annotator.
        /// </summary>
        public void Close()
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");
            if (this.Stat == Stat.Read || this.Stat == Stat.AhaRead)
            {
                PInvoke.iannclose((int)this.Number);
            }
            else
            {
                PInvoke.oannclose((int)this.Number);
            }
            UnregisterAnnotator(this);
        }


        /// <summary>
        /// Returns the current annotation and moves the reading pointer automatically to the next position.
        /// </summary>
        /// <returns>The current annotation.</returns>
        public Annotation ReadNext()
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");

            var annotation = new Annotation();
            var ret = PInvoke.getann(this.Number, ref annotation);
            if (ret == -1)
                throw new InvalidOperationException("End of file.");
            else if (ret == -2)
                throw new InvalidOperationException("Failure: incorrect annotator number specified.");
            else if (ret == -3)
                throw new InvalidOperationException("Failure: unexpected physical end of file.");
            //this.CurrentTime = annotation.Time;
            return annotation;
        }

        /// <summary>
        /// Returns the available <paramref name="count"/> annotations starting from the current position.
        /// </summary>
        /// <param name="count">Number of annotations to be read</param>
        /// <returns>A list containing <paramref name="count"/> annotations available starting from the current position.</returns>
        public IEnumerable<Annotation> ReadNext(int count)
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count","please specify a positive value.");
            int counter = 0;
            var ret = 0;
            while (counter < count)
            {
                Annotation annotation = new Annotation();
                ret = PInvoke.getann(this.Number, ref annotation);
                counter++;
                if (ret == 0)
                    yield return annotation;
                else 
                    yield break;
            }
        }

        /// <summary>
        /// Gets the available annotations within a time range.
        /// </summary>
        /// <param name="from">Start position.</param>
        /// <param name="duration">Range length.</param>
        /// <returns>A list containing the avaialble annotations between <paramref name="from"/> and <paramref name="from"/>+<paramref name="duration"/> </returns>
        public IEnumerable<Annotation> ReadNext(Time from, Time duration)
        {
            Seek(from);
            var toTime = from + duration;
            Annotation currentAnnotation; 
            while (true)
            {
                currentAnnotation = this.ReadNext();
                if (currentAnnotation.Time >= toTime || this.IsEof)
                    yield break;
                yield return currentAnnotation;
            }
        }

        /// <summary>
        /// Returns all available annotations in the current annotator file.
        /// </summary>
        /// <returns>A list containing all available annotations in the current annotator file.</returns>
        public IEnumerable<Annotation> ReadAll()
        {
            if (!IsOpen)
                throw new NotSupportedException("Annotator is not open.");

            this.Seek(Time.Zero);
            var ret = 0;
            Annotation annotation;
            while (true)
            {
                annotation = new Annotation();
                ret = PInvoke.getann(this.Number, ref annotation);
                if (ret == 0)
                    yield return annotation;
                else
                    yield break;
            }
        }

        public bool Equals(Annotator other)
        {
            if (other == null)
                throw  new ArgumentNullException("other", "Parameter should not be null.");
            return other.name.Equals(name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Annotator)) return false;
            return Equals((Annotator) obj);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public void Dispose()
        {
            this.Close();
        }

        public override string ToString()
        {
            return Name;
        }


        /// <summary>
        /// Closes all open annotators.
        /// </summary>
        public static void CloseAll()
        {
            var annotatorsToClose = new List<Annotator>();
            foreach (var annotator in inputAnnotators)
            {
                annotatorsToClose.Add(annotator);
            }
            foreach (var annotator in outputAnnotators)
            {
                annotatorsToClose.Add(annotator);
            }

            foreach (var annotator in annotatorsToClose)
            {
                annotator.Close();
            }
        }

        /// <summary>
        /// Opens all available annotators associated with the specified record file.
        /// </summary>
        /// <param name="annotators">An array holding the returned annotator objects.</param>
        /// <param name="record">Record's name.</param>
        public static void OpenAll(Annotator[] annotators, string record)
        {
            var ret = PInvoke.annopen(record, annotators, (int)annotators.Length);

            if (ret == -3)
                throw new InvalidOperationException("Failure: Unable to open input annotation file.");
            else if (ret == -4)
                throw new InvalidOperationException("Failure: Unable to open output annotation file.");
            else if (ret == -5)
                throw new InvalidOperationException("Failure: Illegal Stat specified for annotation file.");
            foreach (var annotator in annotators)
            {
                var annot = annotator;
                RegisterAnnotator(annot);
            }
        }

        #endregion

        #region Operator overloads
        public static bool operator ==(Annotator left, Annotator right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Annotator left, Annotator right)
        {
            return !left.Equals(right);
        }

        public static implicit operator int(Annotator ann)
        {
            return ann.Number;
        }
        #endregion

        #region Annotator Numbers Management
        private static List<Annotator> inputAnnotators = new List<Annotator>();
        private static List<Annotator> outputAnnotators = new List<Annotator>();

        private static int GetAnnotatorNumber(Annotator value)
        {
            if (value.Stat == Stat.Read || value.Stat == Stat.AhaRead)
            {
                return inputAnnotators.IndexOf(value);
            }
            else
            {
                return outputAnnotators.IndexOf(value);
            }
        }

        private static void RegisterAnnotator(Annotator ann)
        {
            if (ann.Stat == Stat.Read || ann.Stat == Stat.AhaRead)
            {
                inputAnnotators.Add(ann);
            }
            else
            {
                outputAnnotators.Add(ann);
            }
        }

        private static void UnregisterAnnotator(Annotator ann)
        {
            if (!inputAnnotators.Contains(ann) && ! outputAnnotators.Contains(ann))
                return;
            if (ann.Stat == Stat.Read || ann.Stat == Stat.AhaRead)
            {
                inputAnnotators.Remove(ann);
            }
            else
            {
                outputAnnotators.Remove(ann);
            }
        }

        #endregion
    }
}
