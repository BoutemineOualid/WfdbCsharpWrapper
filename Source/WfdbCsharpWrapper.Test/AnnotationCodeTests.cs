/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Boutemine Oualid, 2009-2012
 * Contact: boutemine.walid@hotmail.com
 * Project web page: https://github.com/oualidb/WfdbCsharpWrapper
 * wfdb: 
 * -----
 * a library for reading and writing annotated waveforms (time series data)
 * Copyright (C) 1999 George B. Moody
 *
 * This library is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Library General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option) any
 * later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
 * PARTICULAR PURPOSE.  See the GNU Library General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Library General Public License along
 * with this library; if not, write to the Free Software Foundation, Inc., 59
 * Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 *
 * You may contact the author by e-mail (george@mit.edu) or postal mail
 * (MIT Room E25-505A, Cambridge, MA 02139 USA).  For updates to this software,
 * please visit PhysioNet (http://www.physionet.org/).
 * _______________________________________________________________________________
 */

using NUnit.Framework;

namespace WfdbCsharpWrapper.Test
{
    /// <summary>
    /// A set of tests for the <see cref="AnnotationCode"/>.
    /// </summary>
    [TestFixture]
    public class AnnotationCodesTests
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Wfdb.Quit();
        }

        [TestFixtureSetUp]
        public void AnnotationCodesTestsFixtureSetup()
        {
            EnumCodes = AnnotationCode.AnnotationCodes.ToArray();
        }

        private AnnotationCode[] EnumCodes { get; set; }
        private bool[] ExpectedQrsAnnotations = new []{
	                        false, true, true, true, true, true, true, true, true, true,			    /* 0 - 9 */
	                        true, true, true, true, false, false, false, false, false, false,			/* 10 - 19 */
	                        false, false, false, false, false, true, false, false, false, false,		/* 20 - 29 */
	                        true, true, false, false, true, true, false, false, true, false,			/* 30 - 39 */
	                        false, true, false, false, false, false, false, false, false, false			/* 40 - 49 */
                        };

        /// <summary>
        /// A test for the <see cref="AnnotationCode.IsAnnotation"/>.
        /// </summary>
        [Test]
        public void IsAnnotationTest()
        {
            // Testing Members
            // 1. Testing the NotQRS which is not a valid annotation
            Assert.IsFalse(EnumCodes[0].IsAnnotation);
            
            // Testing the rest of members
            for (int i = 1; i < EnumCodes.Length; i++)
            {
                Assert.IsTrue(EnumCodes[i].IsAnnotation);
            }
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.IsQrs"/>
        /// </summary>
        [Test]
        public void IsQrsTest()
        {
            // get tests
            foreach (var code in EnumCodes)
            {
                Assert.AreEqual(ExpectedQrsAnnotations[code], code.IsQrs);
            }
            // set tests
            // Setup
            Assert.IsFalse(AnnotationCode.Reserved42.IsQrs);
            AnnotationCode.Reserved42.IsQrs = true;
            bool result = AnnotationCode.Reserved42.IsQrs;
            Assert.IsTrue(result);
            // Rollback
            AnnotationCode.Reserved42.IsQrs =  false;
            bool rollBackResult = AnnotationCode.Reserved42.IsQrs;
            Assert.IsFalse(rollBackResult);

        }

        AnnotationCode[] ExpectedMap1Results = {
	        AnnotationCode.NotQrs,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,		/* 0 - 4 */
	        AnnotationCode.Pvc,	AnnotationCode.Fusion,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,		    /* 5 - 9 */
	        AnnotationCode.Pvc,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.NotQrs,		    /* 10 - 14 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 15 - 19 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 20 - 24 */
	        AnnotationCode.Normal,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 25 - 29 */
	        AnnotationCode.Learn,	AnnotationCode.Pvc,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.Normal,		    /* 30 - 34 */
	        AnnotationCode.Normal,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.Normal, AnnotationCode.NotQrs,		/* 35 - 39 */
	        AnnotationCode.NotQrs, AnnotationCode.Pvc,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		    /* 40 - 44 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs		/* 45 - 49 */
        };

        /// <summary>
        /// A test for <see cref="AnnotationCode.Map1"/>
        /// </summary>
        [Test]
        public void Map1Test()
        {
            // get tests
            foreach(var code in EnumCodes)
            {
                Assert.AreEqual(ExpectedMap1Results[code], code.Map1);
            }

            // set tests
            var defaultMap = AnnotationCode.Reserved42.Map1;
            Assert.AreEqual(defaultMap, AnnotationCode.NotQrs);
            
            // SETUP
            AnnotationCode.Reserved42.Map1 = AnnotationCode.Normal;
            var mapResult = AnnotationCode.Reserved42.Map1;
            Assert.AreEqual(mapResult, AnnotationCode.Normal);
            
            // ROLLBACK
            AnnotationCode.Reserved42.Map1 = AnnotationCode.NotQrs;
            var rollbackResult = AnnotationCode.Reserved42.Map1;
            Assert.AreEqual (rollbackResult, AnnotationCode.NotQrs);
        
        }

        AnnotationCode[] ExpectedMap2Results = {
	        AnnotationCode.NotQrs,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Svpb,		/* 0 - 4 */
	        AnnotationCode.Pvc,	AnnotationCode.Fusion,	AnnotationCode.Svpb,	AnnotationCode.Svpb,	AnnotationCode.Svpb,		    /* 5 - 9 */
	        AnnotationCode.Pvc,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.Normal,	AnnotationCode.NotQrs,		    /* 10 - 14 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 15 - 19 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 20 - 24 */
	        AnnotationCode.Normal,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 25 - 29 */
	        AnnotationCode.Learn,	AnnotationCode.Pvc,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.Normal,		    /* 30 - 34 */
	        AnnotationCode.Normal,	AnnotationCode.NotQrs, AnnotationCode.NotQrs, AnnotationCode.Normal, AnnotationCode.NotQrs,		    /* 35 - 39 */
	        AnnotationCode.NotQrs, AnnotationCode.Pvc,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		    /* 40 - 44 */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs       /* 45 - 49 */
        };

        /// <summary>
        /// A test for <see cref="AnnotationCode.Map2"/>
        /// </summary>
        [Test]
        public void Map2Test()
        {
            // get tests
            foreach (var code in EnumCodes)
            {
                Assert.AreEqual(ExpectedMap2Results[code], code.Map2);
            }

            // set tests
            var defaultMap = AnnotationCode.Reserved42.Map2;
            Assert.AreEqual(defaultMap, AnnotationCode.NotQrs);
            // SETUP
            AnnotationCode.Reserved42.Map2 =  AnnotationCode.Normal;
            var mapResult = AnnotationCode.Reserved42.Map2;
            Assert.AreEqual(mapResult, AnnotationCode.Normal);
            // ROLLBACK
            AnnotationCode.Reserved42.Map2 = AnnotationCode.NotQrs;
            var rollbackResult = AnnotationCode.Reserved42.Map2;
            Assert.AreEqual(rollbackResult, AnnotationCode.NotQrs);

        }

        AnnotationCode[] ExpectedAhaToMitMapResult = {
	        AnnotationCode.Vesc,	AnnotationCode.Fusion,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 'E' - 'I' */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.Normal,		/* 'J' - 'N' */
	        AnnotationCode.Note,	AnnotationCode.Pace,	AnnotationCode.Unknown, AnnotationCode.ROnT,	AnnotationCode.NotQrs,		/* 'O' - 'S' */
	        AnnotationCode.NotQrs,	AnnotationCode.Noise,	AnnotationCode.Pvc,	AnnotationCode.NotQrs,	AnnotationCode.NotQrs,		/* 'T' - 'X' */
	        AnnotationCode.NotQrs,	AnnotationCode.NotQrs,	AnnotationCode.VfOn,	AnnotationCode.NotQrs,	AnnotationCode.WFOff		/* 'Y' - ']' */
        };

        /// <summary>
        /// A test for <see cref="AnnotationCode.MapAhaToMit"/>
        /// </summary>
        [Test]
        public void AhaToMitMapTest()
        {
            for (char i = 'E'; i < ']'; i++)
            {
                var result = AnnotationCode.MapAhaToMit(i);
                Assert.AreEqual(result, ExpectedAhaToMitMapResult[i - 'E']);
            }
        }


        char[] ExpectedMitToAhaMapResults = 
        {
	        'O',	'N',	'N',	'N',	'N',		/* 00 - 04 */
	        'V',	'F',	'N',	'N',	'N',		/* 05 - 09 */
	        'E',	'N',	'P',	'Q',	'U',		/* 10 - 14 */
	        'O',	'O',	'O',	'O',	'O',		/* 15 - 19 */
	        'O',	'O',	'O',	'O',	'O',		/* 20 - 24 */
	        'N',	'O',	'O',	'O',	'O',		/* 25 - 29 */
	        'Q',	'O',	'[',	']',	'N',		/* 30 - 34 */
	        'N',	'O',	'O',	'N',	'O',		/* 35 - 39 */
	        'O',	'R',	'O',	'O',	'O',		/* 40 - 44 */
	        'O',	'O',	'O',	'O',	'O'		    /* 45 - 49 */
        };

        /// <summary>
        /// A test for <see cref="AnnotationCode.ToAha(AnnotationCode)"/>
        /// </summary>
        [Test]
        public void MitToAhaMapTest()
        {
            foreach (var code in EnumCodes)
            {
                Assert.AreEqual(code.ToAha(-1), ExpectedMitToAhaMapResults[code]);
            }
        }

        AnnotationPos[] ExpectedAnnotationPos = {
	        AnnotationPos.APUndef, AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,		    /* 0 - 4 */
	        AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,		/* 5 - 9 */
	        AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APHigh,		/* 10 - 14 */
	        AnnotationPos.APUndef,AnnotationPos.APHigh,	AnnotationPos.APUndef,AnnotationPos.APHigh,	AnnotationPos.APHigh,		        /* 15 - 19 */
	        AnnotationPos.APHigh,	AnnotationPos.APHigh,	AnnotationPos.APHigh,	AnnotationPos.APHigh,	AnnotationPos.APHigh,		/* 20 - 24 */
	        AnnotationPos.APStd,	AnnotationPos.APHigh,	AnnotationPos.APHigh,	AnnotationPos.APLow,	AnnotationPos.APHigh,		/* 25 - 29 */
	        AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,	AnnotationPos.APStd,		/* 30 - 34 */
	        AnnotationPos.APStd,	AnnotationPos.APHigh,	AnnotationPos.APHigh,	AnnotationPos.APStd,	AnnotationPos.APHigh,		/* 35 - 39 */
	        AnnotationPos.APHigh,	AnnotationPos.APStd,	AnnotationPos.APUndef, AnnotationPos.APUndef, AnnotationPos.APUndef,	    /* 40 - 44 */
	        AnnotationPos.APUndef,AnnotationPos.APUndef,AnnotationPos.APUndef,AnnotationPos.APUndef,AnnotationPos.APUndef               /* 45 - 49 */
        };

        /// <summary>
        /// A test for <see cref="AnnotationCode.AnnotationPos"/>
        /// </summary>
        [Test]
        public void AnnotationPosTest()
        {
            // Get tests
            foreach (var code in EnumCodes)
            {
                Assert.AreEqual(ExpectedAnnotationPos[code], code.AnnotationPos);
            }
            // set tests
            var defaultPos = AnnotationCode.Reserved42.AnnotationPos;
            Assert.AreEqual(defaultPos, AnnotationPos.APUndef);

            // SETUP
            AnnotationCode.Reserved42.AnnotationPos = AnnotationPos.APStd;
            var setPosResult = AnnotationCode.Reserved42.AnnotationPos;
            Assert.AreEqual(setPosResult, AnnotationPos.APStd);

            // ROLLBACK
            AnnotationCode.Reserved42.AnnotationPos = AnnotationPos.APUndef;
            var rollbacksSetPosResult = AnnotationCode.Reserved42.AnnotationPos;
            Assert.AreEqual(rollbacksSetPosResult, AnnotationPos.APUndef);
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.Parse"/>
        /// </summary>
        [Test]
        public void ParseTest()
        {
            // Basic test
            var expectedAnnotationCode = AnnotationCode.Pace;
            var parseResult = AnnotationCode.Parse(expectedAnnotationCode.String);
            Assert.AreEqual(parseResult, expectedAnnotationCode);

            // INVALID INPUT STRING
            expectedAnnotationCode = AnnotationCode.NotQrs;
            Assert.AreEqual(AnnotationCode.Parse("Some invalid string"), expectedAnnotationCode);
            Assert.AreEqual(AnnotationCode.Parse(null), expectedAnnotationCode);
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.ParseEcgString"/>
        /// </summary>
        [Test]
        public void ParseEcgString()
        {
            var expectedAnnotationCode = AnnotationCode.PWave;
            var result = AnnotationCode.ParseEcgString(expectedAnnotationCode.EcgString);
            Assert.AreEqual(result, expectedAnnotationCode);

            // INVALID INPUT STRING
            expectedAnnotationCode = AnnotationCode.NotQrs;
            Assert.AreEqual(AnnotationCode.ParseEcgString("Some invalid string"), expectedAnnotationCode);
            Assert.AreEqual(AnnotationCode.ParseEcgString(null), expectedAnnotationCode);
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.String"/>
        /// </summary>
        [Test]
        public void StringTest()
        {
            var annotationCode = AnnotationCode.Pace;
            // Setup
            var oldString = annotationCode.String;
            // generic string
            var testString = "Test String";
            // get set test
            annotationCode.String = testString; // set
            Assert.AreEqual(annotationCode.String, testString); //get

            // Parse test
            var parseResult1 = AnnotationCode.Parse(testString);
            Assert.AreEqual(parseResult1, annotationCode);

            // Generic String
            testString = null;
            // Get Set Test
            annotationCode.String = testString; // set
            Assert.AreEqual(annotationCode.String, string.Empty); //get

            // ROLLBACK
            annotationCode.String = oldString;
            parseResult1 = AnnotationCode.Parse(testString);
            var parseResult2 = AnnotationCode.Parse(oldString);
            Assert.AreNotEqual(parseResult1, annotationCode);
            Assert.AreEqual(parseResult2, annotationCode);
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.Description"/>
        /// </summary>
        [Test]
        public void DescriptionTest()
        {
            var annotationCode = AnnotationCode.Pace;
            var oldString = annotationCode.Description;
            // Generic description
            var testString = "Test String";
            // Get Set Test
            annotationCode.Description = testString; // set
            Assert.AreEqual(annotationCode.Description, testString); //get
            
            // null description
            testString = null;
            // get set test
            annotationCode.Description = testString; // set
            Assert.AreEqual(annotationCode.Description, string.Empty); //get

            // ROLLBACK
            annotationCode.Description = oldString;
            Assert.AreEqual(annotationCode.Description, oldString);
        }

        /// <summary>
        /// A test for <see cref="AnnotationCode.EcgString"/>
        /// </summary>
        [Test]
        public void EcgStringTest()
        {
            var annotationCode = AnnotationCode.Pace;
            
            // Setup
            var oldString = annotationCode.String;
            
            // generic string
            var testString = "Test String";
            
            // get set test
            annotationCode.EcgString = testString; // set
            Assert.AreEqual(annotationCode.EcgString, testString); // get

            // Parse test
            var parseResult1 = AnnotationCode.ParseEcgString(testString);
            Assert.AreEqual(parseResult1, annotationCode);

            // null string
            testString = null;
            // get set test
            annotationCode.EcgString = testString; // set
            Assert.AreEqual(annotationCode.EcgString, string.Empty); //get

            // ROLLBACK
            annotationCode.EcgString = oldString;
            parseResult1 = AnnotationCode.ParseEcgString(testString);
            var parseResult2 = AnnotationCode.ParseEcgString(oldString);
            Assert.AreNotEqual(parseResult1, annotationCode);
            Assert.AreEqual(parseResult2, annotationCode);
        }
    }
}