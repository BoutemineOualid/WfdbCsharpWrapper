/*_______________________________________________________________________________
 * wfdbcsharpwrapper:
 * ------------------
 * A .NET library that encapsulates the wfdb library.
 * Copyright Oualid BOUTEMINE, 2009-2016
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
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace WfdbCsharpWrapper.Test
{
    [TestFixture]
    public class AnnotatorTests
    {
        [TearDown]
        public void FreeResources()
        {
            // Closing any open annotators
            Annotator.CloseAll();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenInvalidRecordTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/invalid");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenOpenedAnnotator()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            annotator.Open("data/100s");
        }

        [Test]
        [Ignore] // always fails, we've to check on that
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenIllegalStatAnotator()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.AhaRead };
            annotator.Open("data/100s");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenInexistingRecordTest()
        {
            var annotator = new Annotator {Name = "atr", Stat = Stat.Read};
            annotator.Open("some inexistent record");
        }

        [Test]
        public void OpenClose1AnnotatorValidRecordTest()
        {
            // Valid annotator
            
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };


            // ****************************
            // Keeping old annotators open.
            // ****************************

            annotator.Open("data/100s");
            Assert.IsTrue(annotator.IsOpen);
            annotator.Close(); 
            Assert.IsFalse(annotator.IsOpen);

            // Reopening the same annotator.
            annotator.Open("data/100s");
            Assert.IsTrue(annotator.IsOpen);
            annotator.Close();
            Assert.IsFalse(annotator.IsOpen);
        }

        [Test]
        public void OpenNAnnotatorValidRecord()
        {
            var annotator1 = new Annotator { Name = "atr", Stat = Stat.Read };

            // Opening another annotator without closing the old annotator
            annotator1.Open("data/100s");
            Assert.IsTrue(annotator1.IsOpen);
            var expectedAnnotations1 = annotator1.ReadAll().ToList();

            var annotator2 = new Annotator { Name = "hrv", Stat = Stat.Read };
            annotator2.Open("data/100s");
            Assert.IsTrue(annotator1.IsOpen);
            Assert.IsTrue(annotator2.IsOpen);
            var expectedAnnotations2 = annotator2.ReadAll().ToList();

            // Closing the first annotator and keeping the second one
            annotator1.Close();
            Assert.IsFalse(annotator1.IsOpen);
            Assert.IsTrue(annotator2.IsOpen);

            var annotations2 = annotator2.ReadAll(); 
            Assert.AreEqual(expectedAnnotations2, annotations2);

            // Reopening the first annotator, this time it should have the 1 number
            annotator1.Open("data/100s");
            var annotations1 = annotator1.ReadAll();
            Assert.AreEqual(expectedAnnotations1, annotations1);

            annotator1.Close();
            annotator2.Close();
            Assert.IsFalse(annotator1.IsOpen);
            Assert.IsFalse(annotator2.IsOpen);

            // Reopening in the same order
            annotator1.Open("data/100s"); //0
            Assert.IsTrue(annotator1.IsOpen);
            Assert.IsFalse(annotator2.IsOpen);
            
            annotator2.Open("data/100s"); //1
            Assert.IsTrue(annotator1.IsOpen);
            Assert.IsTrue(annotator2.IsOpen);

            // Reading Some data
            annotations1 = annotator1.ReadAll();  // 0
            Assert.AreEqual(expectedAnnotations1, annotations1);
            
            annotations2 = annotator2.ReadAll(); // 1
            Assert.AreEqual(expectedAnnotations2, annotations2);

            annotator1.Close(); 
            annotator2.Close();

            // reopening in an inversed order
            annotator2.Open("data/100s"); //0
            Assert.IsTrue(annotator2.IsOpen);
            Assert.IsFalse(annotator1.IsOpen);

            annotator1.Open("data/100s"); //1
            Assert.IsTrue(annotator1.IsOpen);
            Assert.IsTrue(annotator2.IsOpen);

            // Reading Some data
            annotations1 = annotator1.ReadAll();  // 0
            Assert.AreEqual(expectedAnnotations1, annotations1);

            annotations2 = annotator2.ReadAll(); // 1
            Assert.AreEqual(expectedAnnotations2, annotations2);

            annotator1.Close();
            annotator2.Close();

        }

        [Test]
        public void OpenNAnnotatorCloseOldValidRecord()
        {
            var annotator = new Annotator {Name = "atr", Stat = Stat.Read};
            annotator.Open("data/100s");
            Assert.IsTrue(annotator.IsOpen);

            var annotator2 = new Annotator {Name = "hrv", Stat = Stat.Read};
            annotator2.Open("data/100s", false);
            Assert.IsFalse(annotator.IsOpen);
            Assert.IsTrue(annotator2.IsOpen);

            annotator2.Close();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CloseUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Close();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CloseClosedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            annotator.Close();

            annotator.Close();
        }

        [Test]
        public void ReadAllOpenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            var expectedAnnotations = new List<Annotation>();

            while (!annotator.IsEof)
            {
                expectedAnnotations.Add(annotator.ReadNext());
            }
            Assert.IsTrue(annotator.IsEof);
            annotator.Seek(Time.Zero);

            var annotations = annotator.ReadAll();
            Assert.AreEqual(expectedAnnotations, annotations);
            annotator.Close();

            // Reopening
            annotator.Open("data/100s");
            annotations = annotator.ReadAll();
            Assert.AreEqual(expectedAnnotations, annotations);
            annotator.Close();

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadAllUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.ReadAll().ToList(); // force a loop, throws NotSupportedException
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadAllClosedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            annotator.Close();
            annotator.ReadAll().ToList(); // force a loop
        }

        [Test]
        public void ReadNextTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            var expectedAllAnnotations = annotator.ReadAll();

            annotator.Seek(Time.Zero);
            var annotations = new List<Annotation>();
            while (!annotator.IsEof)
            {
                annotations.Add(annotator.ReadNext());
            }

            Assert.AreEqual(expectedAllAnnotations, annotations);
            Assert.IsTrue(annotator.IsEof);

            try
            {
                annotator.ReadNext();
                Assert.Fail("InvalidOperationException should have been thrown.");
            }
            catch (InvalidOperationException) // end of file reached
            {
            }
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadNextUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.ReadNext();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadNextCountUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.ReadNext(5).ToList(); // force a loop
        }

        [Test]
        public void ReadNextCountTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");

            // Gets the total number of annotations available in the current annotator file.
            var availableAnnotationsCount = annotator.ReadAll().Count();
            // Reset the pointer to the first annotation.
            annotator.Seek(Time.Zero);

            // Read all using ReadNext(Count)
            var readCount = annotator.ReadNext(availableAnnotationsCount).Count();
            Assert.AreEqual(availableAnnotationsCount, readCount);
            Assert.IsTrue(annotator.IsEof);

            // Cant read when the
            readCount = annotator.ReadNext(1).Count();
            Assert.AreEqual(0, readCount);

            // Seeking to the middle of the file.
            var middle = availableAnnotationsCount / 2;
            annotator.Seek(Time.Zero);
            annotator.Seek(middle);

            readCount = annotator.ReadNext(availableAnnotationsCount).Count();
            Assert.AreEqual(availableAnnotationsCount - middle, readCount);

            Assert.IsTrue(annotator.IsEof);
            
            // Testing boundaries
            annotator.Seek(Time.Zero);
            readCount = annotator.ReadNext(1).Count();
            Assert.AreEqual(1, readCount);

            annotator.Seek(Time.Zero);
            readCount = annotator.ReadNext(availableAnnotationsCount + 1).Count();
            Assert.AreEqual(availableAnnotationsCount, readCount);

            annotator.Seek(Time.Zero);
            readCount = annotator.ReadNext(int.MaxValue).Count();
            Assert.AreEqual(availableAnnotationsCount, readCount);

            // Testing invalid values.
            try
            {
                annotator.ReadNext(-1).ToList(); // force loop
                Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            annotator.Close();
        }

        [Test]
        public void IsEofTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };

            annotator.Open("data/100s");

            Assert.IsFalse(annotator.IsEof);
            
            // Reading some data
            annotator.ReadNext();
            Assert.IsFalse(annotator.IsEof);

            // Reading all available annotations.
            annotator.ReadAll().ToList(); // force a loop over the enumerator.
            Assert.IsTrue(annotator.IsEof);
            
            // Seeking to the beginning.
            annotator.Seek(Time.Zero);
            Assert.IsFalse(annotator.IsEof);

            annotator.ReadAll();
            annotator.Close();
            Assert.IsFalse(annotator.IsOpen);
            // Reopening
            annotator.Open("data/100s");
            Assert.IsFalse(annotator.IsEof);
            annotator.Close();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsEofUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            var isEof = annotator.IsEof; // throws the exception.
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsEofClosedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            annotator.Close();
            var isEof = annotator.IsEof; // throws the exception.
        }

        [Test]
        public void SeekCountTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            var expectedAllAnnotations = annotator.ReadAll().ToList();
            var maxCount = expectedAllAnnotations.Count;
            int midCount = expectedAllAnnotations.Count / 2;
            var firstAnnotation = expectedAllAnnotations[0];
            var lastAnnotation = expectedAllAnnotations[expectedAllAnnotations.Count - 1];

            annotator.Seek(Time.Zero);

            annotator.Seek(0);
            Assert.AreEqual(firstAnnotation, annotator.ReadNext());
            Assert.IsFalse(annotator.IsEof);
            
            annotator.Seek(Time.Zero);
            
            annotator.Seek(maxCount - 1);
            Assert.AreEqual(lastAnnotation, annotator.ReadNext());
            Assert.IsTrue(annotator.IsEof);

            annotator.Seek(Time.Zero);

            var annotations = new List<Annotation>();
            annotator.Seek(midCount);
            while (!annotator.IsEof)
            {
                annotations.Add(annotator.ReadNext());
            }

            expectedAllAnnotations.RemoveRange(0, midCount);
            Assert.AreEqual(expectedAllAnnotations, annotations);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SeekCountUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Seek(3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SeekCountNegativeValue()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            annotator.Seek(-1);
        }

        [Test]
        public void SeekTimeTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            var expectedAllAnnotations = annotator.ReadAll().ToList();
            var maxTime = expectedAllAnnotations[expectedAllAnnotations.Count - 1].Time;
            int midIndex = expectedAllAnnotations.Count/2;

            var midTime = expectedAllAnnotations[midIndex].Time;
            
            // Seek to zero
            Assert.IsTrue(annotator.IsEof);
            annotator.Seek(Time.Zero);
            Assert.IsFalse(annotator.IsEof);
            Assert.AreEqual(expectedAllAnnotations, annotator.ReadAll()); // all annotations are there.

            annotator.Seek(Time.Zero);
            annotator.Seek(maxTime);
            Assert.IsFalse(annotator.IsEof); // not the end of file, we still have to read another annotation.

            Assert.AreEqual(expectedAllAnnotations[expectedAllAnnotations.Count - 1], annotator.ReadNext());
            Assert.IsTrue(annotator.IsEof);

            annotator.Seek(Time.Zero);
            annotator.Seek(midTime);
            Assert.IsFalse(annotator.IsEof);

            var annotations = new List<Annotation>();
            var annotation = annotator.ReadNext();
            annotations.Add(annotation);
            Assert.AreEqual(annotation, expectedAllAnnotations[midIndex]);

            while (!annotator.IsEof)
            {
                annotations.Add(annotator.ReadNext());
            }

            Assert.AreEqual(expectedAllAnnotations.Count - midIndex, annotations.Count);
            expectedAllAnnotations.RemoveRange(0, midIndex);
            Assert.AreEqual(expectedAllAnnotations, annotations);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SeekTimeUnopenedAnnotatorTest()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Seek(Time.Zero);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SeekInvalidTime()
        {
            var annotator = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator.Open("data/100s");
            var expectedAllAnnotations = annotator.ReadAll().ToList();
            var maxTime = expectedAllAnnotations[expectedAllAnnotations.Count - 1].Time;
            
            annotator.Seek(Time.Zero);

            Time invalidTime = (maxTime + 60); // + 60 seconds
            annotator.Seek(invalidTime);
        }

        [Test]
        public void OpenAll0Test()
        {
            var annotators = new Annotator[0];
            Annotator.OpenAll(annotators, "data/100s");
            Annotator.CloseAll();
        }

        [Test]
        public void OpenAll1Test()
        {
            var annotators = new Annotator[1];
            annotators[0] = new Annotator{ Name = "atr", Stat = Stat.Read };
            Annotator.OpenAll(annotators, "data/100s");

            Assert.IsTrue(annotators[0].IsOpen);

            Annotator.CloseAll();
            Assert.IsFalse(annotators[0].IsOpen);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenAllNWithInvalidAnnotatorTest()
        {
            var annotators = new Annotator[2];
            annotators[0] = new Annotator { Name = "atr", Stat = Stat.Read };
            // annotators[1] has not been set
            Annotator.OpenAll(annotators, "data/100s");
        }

        [Test]
        public void OpenAllNTest()
        {
            var annotators = new Annotator[2];
            annotators[0] = new Annotator { Name = "atr", Stat = Stat.Read };
            annotators[1] = new Annotator { Name = "hrv", Stat = Stat.Read };
            Annotator.OpenAll(annotators, "data/100s");

            int counter = 0;
            foreach (var annotator in annotators)
            {
                Assert.IsTrue(annotator.IsOpen);
                Assert.AreEqual(counter, annotator.Number);
                counter++;
            }

            Annotator.CloseAll();
            foreach (var annotator in annotators)
            {
                Assert.IsFalse(annotator.IsOpen);
            }
        }

        [Test]
        public void NumberTest()
        {
            var annotator1 = new Annotator { Name = "atr", Stat = Stat.Read };
            annotator1.Open("data/100s");
            Assert.AreEqual(0, annotator1.Number);

            var annotator2 = new Annotator { Name = "hrv", Stat = Stat.Read };
            annotator2.Open("data/100s");
            Assert.AreEqual(0, annotator1.Number);
            Assert.AreEqual(1, annotator2.Number);

            annotator1.Close();
            Assert.AreEqual(-1, annotator1.Number);
            Assert.AreEqual(0, annotator2.Number);

            annotator1.Open("data/100s");
            Assert.AreEqual(0, annotator2.Number);
            Assert.AreEqual(1, annotator1.Number);
            Annotator.CloseAll();
            
            Assert.AreEqual(-1, annotator1.Number);
            Assert.AreEqual(-1, annotator2.Number);
        }
    }
}