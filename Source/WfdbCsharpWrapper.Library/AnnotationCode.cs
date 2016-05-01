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
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    /// <summary>
    /// Annotation Codes.
    /// <remarks>
    /// The annotation codes are the predefined values of the <see cref="Annotation.Type"/>
    /// field. Other values in the range of 1 to <see cref="AnnotationCode.ACMax"/> are legal but do not have preassigned meanings. The constant
    /// <see cref="AnnotationCode.NotQrs"/>, is not a legal value for <see cref="Annotation.Type"/>, but is a
    /// possible output of macros implemented in this class.
    /// </remarks>
    /// </summary>
    public class AnnotationCode : IComparable<AnnotationCode>, IEquatable<AnnotationCode>
    {
        #region ctor
        /// <summary>
        /// Creates a new instance from <see cref="AnnotationCode"/>
        /// </summary>
        /// <param name="value">Annotation code's value.</param>
        public AnnotationCode(byte value)
        {
            this.value = value;
        }

        static AnnotationCode()
        {
            annotationCodes = new List<AnnotationCode>();
            annotationCodes.Add(AnnotationCode.Aberr);
            annotationCodes.Add(AnnotationCode.ACMax);
            annotationCodes.Add(AnnotationCode.Aesc);
            annotationCodes.Add(AnnotationCode.Apc);
            annotationCodes.Add(AnnotationCode.Arfct);
            annotationCodes.Add(AnnotationCode.Bbb);
            annotationCodes.Add(AnnotationCode.Diastole);
            annotationCodes.Add(AnnotationCode.FLWav);
            annotationCodes.Add(AnnotationCode.Fusion);
            annotationCodes.Add(AnnotationCode.JPt);
            annotationCodes.Add(AnnotationCode.Lbbb);
            annotationCodes.Add(AnnotationCode.Learn);
            annotationCodes.Add(AnnotationCode.Link);
            annotationCodes.Add(AnnotationCode.Measure);
            annotationCodes.Add(AnnotationCode.NApc);
            annotationCodes.Add(AnnotationCode.Nesc);
            annotationCodes.Add(AnnotationCode.Noise);
            annotationCodes.Add(AnnotationCode.Normal);
            annotationCodes.Add(AnnotationCode.Note);
            annotationCodes.Add(AnnotationCode.NotQrs);
            annotationCodes.Add(AnnotationCode.Npc);
            annotationCodes.Add(AnnotationCode.Pace);
            annotationCodes.Add(AnnotationCode.PaceSP);
            annotationCodes.Add(AnnotationCode.Pfus);
            annotationCodes.Add(AnnotationCode.PQ);
            annotationCodes.Add(AnnotationCode.Pvc);
            annotationCodes.Add(AnnotationCode.PWave);
            annotationCodes.Add(AnnotationCode.Rbbb);
            annotationCodes.Add(AnnotationCode.Reserved42);
            annotationCodes.Add(AnnotationCode.Reserved43);
            annotationCodes.Add(AnnotationCode.Reserved44);
            annotationCodes.Add(AnnotationCode.Reserved45);
            annotationCodes.Add(AnnotationCode.Reserved46);
            annotationCodes.Add(AnnotationCode.Reserved47);
            annotationCodes.Add(AnnotationCode.Reserved48);
            annotationCodes.Add(AnnotationCode.Rhythm);
            annotationCodes.Add(AnnotationCode.ROnT);
            annotationCodes.Add(AnnotationCode.STCh);
            annotationCodes.Add(AnnotationCode.Svesc);
            annotationCodes.Add(AnnotationCode.Svpb);
            annotationCodes.Add(AnnotationCode.Systole);
            annotationCodes.Add(AnnotationCode.TCh);
            annotationCodes.Add(AnnotationCode.TWave);
            annotationCodes.Add(AnnotationCode.Unknown);
            annotationCodes.Add(AnnotationCode.UWave);
            annotationCodes.Add(AnnotationCode.Vesc);
            annotationCodes.Add(AnnotationCode.VfOff);
            annotationCodes.Add(AnnotationCode.VfOn);
            annotationCodes.Add(AnnotationCode.WFOff);
            annotationCodes.Add(AnnotationCode.WFOn);

            annotationCodes.Sort();
        }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Maps an AHA annotation code into an MIT annotation code (one of the
        /// set {<see cref="AnnotationCode.Normal"/>, <see cref="AnnotationCode.Pvc"/>, <see cref="AnnotationCode.Fusion"/>, <see cref="AnnotationCode.ROnT"/>, <see cref="AnnotationCode.Vesc"/>, 
        /// <see cref="AnnotationCode.Pace"/>, <see cref="AnnotationCode.Unknown"/>, <see cref="AnnotationCode.VfOn"/>, <see cref="AnnotationCode.VfOff"/>, <see cref="AnnotationCode.Noise"/>,
        /// <see cref="AnnotationCode.Note"/>}), or <see cref="AnnotationCode.NotQrs"/>
        /// </summary>
        /// <param name="ahaCode">AHA Annotation code.</param>
        /// <returns>Corresponding MIT Code</returns>
        public static AnnotationCode MapAhaToMit(char ahaCode)
        {
            return PInvoke.wfdb_ammap(ahaCode);
        }

        /// <summary>
        ///  Maps this MIT annotation code into an AHA annotation code.
        /// </summary>
        /// <param name="mitSubCode">
        /// MIT annotation sub code
        /// <remarks>
        /// This parameter is significant only if mitCode is <see cref="AnnotationCode.Noise"/>)
        /// </remarks>
        /// </param>
        /// <returns>The corresponding AHA annotation code</returns>
        public char ToAha(AnnotationCode mitSubCode)
        {
            return (char)PInvoke.wfdb_mamap(this, mitSubCode);
        }

        /// <summary>
        ///  Maps this MIT annotation code into an AHA annotation code.
        /// </summary>
        /// <param name="mitSubCode">
        /// MIT annotation sub code
        /// <remarks>
        /// This parameter is significant only if mitCode is <see cref="AnnotationCode.Noise"/>)
        /// This overloaded version is used when mitSubCode is -1.
        /// </remarks>
        /// </param>
        /// <returns>The corresponding AHA annotation code</returns>
        public char ToAha(int mitSubCode)
        {
            return (char)PInvoke.wfdb_mamap(this, mitSubCode);
        }

        /// <summary>
        /// Converts a string into a valid annotation code if possible.
        /// </summary>
        /// <param name="code">Annotation code's string.</param>
        /// <returns>Annotation Code.</returns>
        /// <remarks>
        /// Illegal strings are translated into <see cref="AnnotationCode.NotQrs"/>. Input strings
        /// for Parse and ParseEcgString should match those returned by <see cref="String"/> and <see cref="EcgString"/> respectively.
        /// </remarks>
        public static AnnotationCode Parse(string code)
        {
            return PInvoke.strann(code);
        }

        /// <summary>
        /// Converts a string into a valid annotation code if possible.
        /// </summary>
        /// <param name="ecgString">Annotation code's string.</param>
        /// <returns>Annotation Code.</returns>
        /// <remarks>
        /// Illegal strings are translated into <see cref="AnnotationCode.NotQrs"/>. Input strings
        /// for Parse and ParseEcgString should match those returned by <see cref="AnnotationCode.String"/> and <see cref="EcgString"/> respectively.
        /// </remarks>
        public static AnnotationCode ParseEcgString(string ecgString)
        {
            return PInvoke.strecg(ecgString);
        }

        public override string ToString()
        {
            return String;
        }

        public bool Equals(AnnotationCode obj)
        {
            return Value.Equals(obj.Value);
        }

        public override bool Equals(object obj)
        {
            var annotationCode = obj as AnnotationCode;
            if (annotationCode == null)
                return false;

            return Equals(annotationCode);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not a given code is a legal annotation code.
        /// </summary>
        /// <returns>True if code is a legal annotation code, false otherwise.</returns>
        public bool IsAnnotation
        {
            get
            {
                return PInvoke.wfdb_isann(this);
            }
        }

        private static List<AnnotationCode> annotationCodes;
        /// <summary>
        /// Gets the list of supported annotation codes.
        /// </summary>
        public static List<AnnotationCode> AnnotationCodes
        {
            get { return annotationCodes; }
        }

        private byte value;
        /// <summary>
        /// Gets the integer value of this annotation code.
        /// </summary>
        public byte Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Converts the specified annotation code into a string.
        /// </summary>
        public string String
        {
            get
            {
                return Marshal.PtrToStringAnsi(PInvoke.annstr(this));
            }
            set
            {
                PInvoke.setannstr(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the string representation of this annotation code.
        /// </summary>
        /// <remarks>
        /// The strings returned by EcgString are usually
        /// the same as those returned by <see cref="AnnotationCode.String"/>, but they can be modified only using this property's setter.
        /// </remarks>
        public string EcgString
        {
            get
            {
                return Marshal.PtrToStringAnsi(PInvoke.ecgstr(this));
            }
            set
            {
                PInvoke.setecgstr(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the description of the this annotation code.
        /// </summary>
        public string Description
        {
            get
            {
                return Marshal.PtrToStringAnsi(PInvoke.anndesc(this));
            }
            set
            {
                PInvoke.setanndesc(this, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this annotation code denotes a QRS complex.
        /// </summary>
        public bool IsQrs
        {
            get
            {
                return PInvoke.wfdb_isqrs(this);
            }
            set
            {
                PInvoke.wfdb_setisqrs(this, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Gets or sets the resulting annotation code using <see cref="PInvoke.wfdb_map1"/> macro.
        /// The resulting annotation code is one of {<see cref="AnnotationCode.NotQrs"/>, <see cref="AnnotationCode.Normal"/>, 
        /// <see cref="AnnotationCode.Pvc"/>, <see cref="AnnotationCode.Fusion"/>, <see cref="AnnotationCode.Learn"/>}
        /// </summary>
        public AnnotationCode Map1
        {
            get
            {
                return PInvoke.wfdb_map1(this);
            }
            set
            {
                PInvoke.wfdb_setmap1(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the resulting annotation code using <see cref="PInvoke.wfdb_map1"/> macro.
        /// The resulting annotation code is one of the set {<see cref="AnnotationCode.NotQrs"/>, 
        /// <see cref="AnnotationCode.Normal"/>, <see cref="AnnotationCode.Pvc"/>, <see cref="AnnotationCode.Fusion"/>, <see cref="AnnotationCode.Learn"/>}
        /// </summary>
        public AnnotationCode Map2
        {
            get
            {
                return PInvoke.wfdb_map2(value);
            }
            set
            {
                PInvoke.wfdb_setmap2(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the appropriate position code for this annotation code.
        /// <remarks>
        /// This macro was first introduced in WFDB library version 6.0.
        /// </remarks>
        /// </summary>
        public AnnotationPos AnnotationPos
        {
            get
            {
                return (AnnotationPos)PInvoke.wfdb_annpos(this);
            }
            set
            {
                PInvoke.wfdb_setannpos(this, (int)value);
            }
        }
        #endregion

        #region operator overloads
        public static implicit operator AnnotationCode(byte value)
        {
            return new AnnotationCode(value);
        }

        public static implicit operator byte(AnnotationCode code)
        {
            return code.value;
        }

        public static bool operator == (AnnotationCode code1, AnnotationCode code2)
        {
            return code1.Value == code2.Value;
        }

        public static bool operator != (AnnotationCode code1, AnnotationCode code2)
        {
            return code1.Value != code2.Value;
        }

        #endregion

        #region Annotation Codes
        /// <summary>
        /// Not Qrs, no meaning but legal.
        /// </summary>
        public static AnnotationCode NotQrs { get { return 0; } }

        /// <summary>
        /// Normal beat 'N'.
        /// </summary>
        public static AnnotationCode Normal { get { return 1; } }


        /// <summary>
        /// Left bundle branch block beat 'L'.
        /// </summary>
        public static AnnotationCode Lbbb { get { return 2; } }

        /// <summary>
        /// Right bundle branch block beat 'R'.
        /// </summary>
        public static AnnotationCode Rbbb { get { return 3; } }

        /// <summary>
        /// Bundle branch block beat (unspecified) 'B'.
        /// </summary>
        public static AnnotationCode Bbb { get { return 25; } }

        /// <summary>
        /// Atrial premature beat 'A'.
        /// </summary>
        public static AnnotationCode Apc { get { return 8; } }

        /// <summary>
        /// Aberrated atrial premature beat 'a'.
        /// </summary>
        public static AnnotationCode Aberr { get { return 4; } }

        /// <summary>
        /// Nodal (junctional) premature beat 'J'.
        /// </summary>
        public static AnnotationCode Npc { get { return 7; } }

        /// <summary>
        /// Supraventricular premature or ectopic beat (atrial or nodal) 'S'.
        /// </summary>
        public static AnnotationCode Svpb { get { return 9; } }

        /// <summary>
        /// Premature ventricular contraction 'V'.
        /// </summary>
        public static AnnotationCode Pvc { get { return 5; } }

        /// <summary>
        /// R-on-T premature ventricular contraction 'r'.
        /// </summary>
        public static AnnotationCode ROnT { get { return 41; } }

        /// <summary>
        /// Fusion of ventricular and normal beat 'F'.
        /// </summary>
        public static AnnotationCode Fusion { get { return 6; } }

        /// <summary>
        /// Atrial escape beat 'e'.
        /// </summary>
        public static AnnotationCode Aesc { get { return 34; } }

        /// <summary>
        /// Nodal (junctional) escape beat 'j'.
        /// </summary>
        public static AnnotationCode Nesc { get { return 11; } }

        /// <summary>
        /// Supraventricular escape beat (atrial or nodal) 'n'.
        /// <remarks>
        /// This code was first introduced in WFDB library version 4.0.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Svesc { get { return 35; } }

        /// <summary>
        /// Ventricular escape beat 'E'.
        /// </summary>
        public static AnnotationCode Vesc { get { return 10; } }

        /// <summary>
        /// Paced beat '/'.
        /// </summary>
        public static AnnotationCode Pace { get { return 12; } }

        /// <summary>
        /// Fusion of paced and normal beat 'f'.
        /// </summary>
        public static AnnotationCode Pfus { get { return 38; } }

        /// <summary>
        /// Unclassifiable beat 'Q'.
        /// </summary>
        public static AnnotationCode Unknown { get { return 13; } }

        /// <summary>
        /// Beat not classified during learning '?'.
        /// </summary>
        public static AnnotationCode Learn { get { return 30; } }

        #region Non-beat annotation codes:
        /// <summary>
        /// Start of ventricular flutter/fibrillation '['
        /// </summary>
        public static AnnotationCode VfOn { get { return 32; } }

        /// <summary>
        /// Ventricular flutter wave '!'
        /// </summary>
        public static AnnotationCode FLWav { get { return 31; } }

        /// <summary>
        /// End of ventricular flutter/fibrillation ']'
        /// </summary>
        public static AnnotationCode VfOff { get { return 33; } }

        /// <summary>
        /// Non-conducted P-wave (blocked APC) 'x'
        /// </summary>
        public static AnnotationCode NApc { get { return 37; } }

        /// <summary>
        /// Waveform onset '('
        /// </summary>
        public static AnnotationCode WFOn { get { return 39; } }

        /// <summary>
        /// Waveform end ')'
        /// </summary>
        public static AnnotationCode WFOff { get { return 40; } }

        /// <summary>
        /// Peak of P-wave 'p'
        /// <remarks>
        /// This code was first introduced in DB library version
        /// 8.3. The ‘p’ mnemonic now assigned to <see cref="PWave"/> was formerly assigned to <see cref="NApc"/>.
        /// </remarks>
        /// </summary>
        public static AnnotationCode PWave { get { return 24; } }

        /// <summary>
        /// Peak of T-wave 't'
        /// <remarks>
        /// This code was first introduced in DB library version
        /// 8.3. The ‘t’ mnemonic now assigned to <see cref="TWave"/> was formerly assigned to <see cref="TCh"/>.
        /// </remarks>
        /// </summary>
        public static AnnotationCode TWave { get { return 27; } }

        /// <summary>
        /// Peak of U-wave 'u'
        /// <remarks>
        /// This code was first introduced in DB library version 8.3. 
        /// </remarks>
        /// </summary>
        public static AnnotationCode UWave { get { return 29; } }

        /// <summary>
        /// PQ junction '‘'.
        /// <remarks>
        /// The obsolete code PQ (designating the PQ junction) is still defined , but is identical to <see cref="WFOn"/>.
        /// </remarks>
        /// </summary>
        [Obsolete]
        public static AnnotationCode PQ { get { return WFOn; } }

        /// <summary>
        /// J-point '’'
        /// <remarks>
        /// The obsolete code JPt (designating the J-point) is still defined , but is identical to <see cref="WFOff"/>.
        /// </remarks>
        /// </summary>
        [Obsolete]
        public static AnnotationCode JPt { get { return WFOff; } }

        /// <summary>
        /// (Non-captured) pacemaker artifact '^'
        /// </summary>
        public static AnnotationCode PaceSP { get { return 26; } }

        /// <summary>
        /// Isolated QRS-like artifact '|'
        /// <remarks>
        /// In MIT and ESC DB ‘atr’ files, each non-zero bit in the subtyp field indicates that
        /// the corresponding signal contains noise (the least significant bit corresponds to signal
        /// 0).
        /// </remarks>
        /// </summary>
        public static AnnotationCode Arfct { get { return 16; } }

        /// <summary>
        /// Change in signal quality '~'
        /// <remarks>
        /// In MIT and ESC DB ‘atr’ files, each non-zero bit in the subtyp field indicates that
        /// the corresponding signal contains noise (the least significant bit corresponds to signal
        /// 0).
        /// </remarks>
        /// </summary>
        public static AnnotationCode Noise { get { return 14; } }

        /// <summary>
        /// Rhythm change '+'
        /// <remarks>
        /// The aux field contains an ASCII string (with prefixed byte count) describing the
        /// rhythm, ST segment, T-wave change, measurement, or the nature of the comment.
        /// By convention, the character that follows the byte count in the aux field of a RHYTHM
        /// annotation is ‘(’. See the MIT-BIH Arrhythmia Database Directory for a list of rhythm
        /// annotation strings.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Rhythm { get { return 28; } }

        /// <summary>
        /// ST segment change 's'
        /// <remarks>
        /// - This code was first introduced in WFDB library version 4.0.
        /// - The aux field contains an ASCII string (with prefixed byte count) describing the
        /// rhythm, ST segment, T-wave change, measurement, or the nature of the comment.
        /// By convention, the character that follows the byte count in the aux field of a <see cref="Rhythm"/>
        /// annotation is ‘(’. See the MIT-BIH Arrhythmia Database Directory for a list of rhythm
        /// annotation strings.
        /// </remarks>
        /// </summary>
        public static AnnotationCode STCh { get { return 18; } }

        /// <summary>
        /// T-wave change 'T'
        /// <remarks>
        /// - This code was first introduced in WFDB library version 4.0.
        /// - The aux field contains an ASCII string (with prefixed byte count) describing the
        /// rhythm, ST segment, T-wave change, measurement, or the nature of the comment.
        /// By convention, the character that follows the byte count in the aux field of a <see cref="Rhythm"/>
        /// annotation is ‘(’. See the MIT-BIH Arrhythmia Database Directory for a list of rhythm
        /// annotation strings.
        /// - the ‘t’ mnemonic now assigned to TWAVE was formerly assigned to TCH.
        /// </remarks>
        /// </summary>
        public static AnnotationCode TCh { get { return 19; } }

        /// <summary>
        /// Systole '*'
        /// <remarks>
        /// This code was first introduced in WFDB library version 7.0.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Systole { get { return 20; } }

        /// <summary>
        /// Diastole 'D'
        /// <remarks>
        /// - This code was first introduced in WFDB library version 7.0.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Diastole { get { return 21; } }

        /// <summary>
        /// Measurement annotation '='
        /// <remarks>
        /// - This code was first introduced in WFDB library version 7.0.
        /// - The aux field contains an ASCII string (with prefixed byte count) describing the
        /// rhythm, ST segment, T-wave change, measurement, or the nature of the comment.
        /// By convention, the character that follows the byte count in the aux field of a <see cref="Rhythm"/>
        /// annotation is ‘(’. See the MIT-BIH Arrhythmia Database Directory for a list of rhythm
        /// annotation strings.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Measure { get { return 23; } }

        /// <summary>
        /// Comment annotation '"'
        /// <remarks>
        /// The aux field contains an ASCII string (with prefixed byte count) describing the
        /// rhythm, ST segment, T-wave change, measurement, or the nature of the comment.
        /// By convention, the character that follows the byte count in the aux field of a <see cref="Rhythm"/>
        /// annotation is ‘(’. See the MIT-BIH Arrhythmia Database Directory for a list of rhythm
        /// annotation strings.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Note { get { return 22; } }

        /// <summary>
        /// Link to external data '@'.
        /// <remarks>
        /// The <see cref="Link"/>  code was first introduced in WFDB library version 9.6. The aux field
        /// of a LINK annotation contains a URL (a uniform resource locator, in the form
        /// ‘http://machine.name/some/data’, suitable for passing to a Web browser such as
        /// Netscape or Mosaic). LINK annotations may be used to associate extended text,
        /// images, or other data with an annotation file. If the aux field contains any whitespace,
        /// text following the first whitespace is taken as descriptive text to be displayed by a
        /// WFDB browser such as WAVE.
        /// </remarks>
        /// </summary>
        public static AnnotationCode Link { get { return 36; } }

        /// <summary>
        /// Value of largest valid annotation code.
        /// </summary>
        public static AnnotationCode ACMax { get { return 49; } }

        /// <summary>
        /// User defined.
        /// </summary>
        public static AnnotationCode Reserved42 { get { return 42; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved43 { get { return 43; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved44 { get { return 44; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved45 { get { return 45; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved46 { get { return 46; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved47 { get { return 47; } }

        /// <summary>
        /// User defined
        /// </summary>
        public static AnnotationCode Reserved48 { get { return 48; } }

        #endregion

        #endregion

        #region IComparable<AnnotationCode> Members

        public int CompareTo(AnnotationCode other)
        {
            return Value.CompareTo(other.Value);
        }

        #endregion
    }
}
