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
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper
{
    public class PInvoke
    {
        internal const string WfdbPath = "wfdb";

        [DllImport(WfdbPath, EntryPoint = "adumuv")]
        public static extern int adumuv(int s, int a);

        [DllImport(WfdbPath, EntryPoint = "aduphys")]
        public static extern double aduphys(int s, int a);

        [DllImport(WfdbPath, EntryPoint = "anndesc")]
        public static extern IntPtr anndesc(int code);

        [DllImport(WfdbPath, EntryPoint = "annopen")]
        public static extern int annopen(string record, [In, Out] Annotator[] aiarray, int nann);

        [DllImport(WfdbPath, EntryPoint = "annopen")]
        public static extern int annopen(string record, ref Annotator ai, int nann);

        [DllImport(WfdbPath, EntryPoint = "annstr")]
        public static extern IntPtr annstr(int code);




        [DllImport(WfdbPath, EntryPoint = "calopen")]
        public static extern int calopen(string file);




        [DllImport(WfdbPath, EntryPoint = "datstr")]
        public static extern IntPtr datstr(int t);




        [DllImport(WfdbPath, EntryPoint = "ecgstr")]
        public static extern IntPtr ecgstr(int code);




        [DllImport(WfdbPath, EntryPoint = "findsig")]
        public static extern int findsig(string str);

        [DllImport(WfdbPath, EntryPoint = "flushcal")]
        public static extern void flushcal();




        [DllImport(WfdbPath, EntryPoint = "getafreq")]
        public static extern double getafreq();

        [DllImport(WfdbPath, EntryPoint = "getann")]
        public static extern int getann(int an, ref Annotation annot);

        [DllImport(WfdbPath, EntryPoint = "getbasecount")]
        public static extern double getbasecount();

        [DllImport(WfdbPath, EntryPoint = "getcal")]
        public static extern int getcal(string desc, string units, ref CalibrationInfo cal);

        [DllImport(WfdbPath, EntryPoint = "getcfreq")]
        public static extern double getcfreq();

        [DllImport(WfdbPath, EntryPoint = "getframe")]
        public static extern int getframe(int vec);

        [DllImport(WfdbPath, EntryPoint = "getifreq")]
        public static extern double getifreq();

        [DllImport(WfdbPath, EntryPoint = "getinfo")]
        public static extern IntPtr getinfo(string record);

        [DllImport(WfdbPath, EntryPoint = "getspf")]
        public static extern int getspf();

        [DllImport(WfdbPath, EntryPoint = "getvec")]
        public static extern int getvec([In, Out]Sample[] vec);

        [DllImport(WfdbPath, EntryPoint = "getwfdb")]
        public static extern IntPtr getwfdb();




        [DllImport(WfdbPath, EntryPoint = "iannclose")]
        public static extern void iannclose(int an);

        [DllImport(WfdbPath, EntryPoint = "iannsettime")]
        public static extern int iannsettime(int t);

        [DllImport(WfdbPath, EntryPoint = "isgsettime")]
        public static extern int isgsettime(int sgroup, long t);

        [DllImport(WfdbPath, EntryPoint = "isigopen")]
        public static extern int isigopen(string record, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, EntryPoint = "isigsettime")]
        public static extern int isigsettime(long t);




        [DllImport(WfdbPath, EntryPoint = "mstimstr")]
        public static extern IntPtr mstimstr(long t);

        [DllImport(WfdbPath, EntryPoint = "muvadu")]
        public static extern int muvadu(int s, int v);




        [DllImport(WfdbPath, EntryPoint = "newcal")]
        public static extern int newcal(string file);

        [DllImport(WfdbPath, EntryPoint = "newheader")]
        public static extern int newheader(string record);




        [DllImport(WfdbPath, EntryPoint = "oannclose")]
        public static extern void oannclose(int an);

        [DllImport(WfdbPath, EntryPoint = "osigfopen")]
        public static extern int osigfopen(Signal[] siarray, int nsig);

        [DllImport(WfdbPath, EntryPoint = "osigopen")]
        public static extern int osigopen(string record, [In, Out] Signal[] siarray, int nsig);




        [DllImport(WfdbPath, EntryPoint = "physadu")]
        public static extern int physadu(int s, double v);

        [DllImport(WfdbPath, EntryPoint = "putann")]
        public static extern int putann(int an, ref Annotation annot);

        [DllImport(WfdbPath, EntryPoint = "putcal")]
        public static extern int putcal(ref CalibrationInfo cal);

        [DllImport(WfdbPath, EntryPoint = "putinfo")]
        public static extern int putinfo(string info);

        [DllImport(WfdbPath, EntryPoint = "putvec")]
        public static extern int putvec(int vec);




        [DllImport(WfdbPath, EntryPoint = "sampfreq")]
        public static extern double sampfreq(string record);

        [DllImport(WfdbPath, EntryPoint = "sample")]
        public static extern int sample(int s, long t);

        [DllImport(WfdbPath, EntryPoint = "sample_valid")]
        public static extern int sample_valid();

        [DllImport(WfdbPath, EntryPoint = "setafreq")]
        public static extern void setafreq(double f);

        [DllImport(WfdbPath, EntryPoint = "setanndesc")]
        public static extern int setanndesc(int code, string str);

        [DllImport(WfdbPath, EntryPoint = "setannstr")]
        public static extern int setannstr(int code, string str);

        [DllImport(WfdbPath, EntryPoint = "setbasecount")]
        public static extern void setbasecount(double count);

        [DllImport(WfdbPath, EntryPoint = "setbasetime")]
        public static extern int setbasetime(string time);

        [DllImport(WfdbPath, EntryPoint = "setcfreq")]
        public static extern void setcfreq(double freq);

        [DllImport(WfdbPath, EntryPoint = "setecgstr")]
        public static extern int setecgstr(int code, string str);

        [DllImport(WfdbPath, EntryPoint = "setgvmode")]
        public static extern void setgvmode(int mode);

        [DllImport(WfdbPath, EntryPoint = "setheader")]
        public static extern int setheader(string record, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, EntryPoint = "setibsize")]
        public static extern int setibsize(int size);

        [DllImport(WfdbPath, EntryPoint = "setifreq")]
        public static extern void setifreq(double f);

        [DllImport(WfdbPath, EntryPoint = "setmsheader")]
        public static extern int setmsheader(string record, [In, Out] string[] snarray, int nsegments);

        [DllImport(WfdbPath, EntryPoint = "setobsize")]
        public static extern int setobsize(int size);

        [DllImport(WfdbPath, EntryPoint = "setsampfreq")]
        public static extern int setsampfreq(double freq);

        [DllImport(WfdbPath, EntryPoint = "setwfdb")]
        public static extern void setwfdb(string str);

        [DllImport(WfdbPath, EntryPoint = "strann")]
        public static extern byte strann(string str);

        [DllImport(WfdbPath, EntryPoint = "strdat")]
        public static extern int strdat(string str);

        [DllImport(WfdbPath, EntryPoint = "strecg")]
        public static extern byte strecg(string code);

        [DllImport(WfdbPath, EntryPoint = "strtim")]
        public static extern int strtim(string str);




        [DllImport(WfdbPath, EntryPoint = "timstr")]
        public static extern IntPtr timstr(long t);

        [DllImport(WfdbPath, EntryPoint = "tnextvec")]
        public static extern int tnextvec(int s, long t);




        [DllImport(WfdbPath, EntryPoint = "ungetann")]
        public static extern int ungetann(int an,ref Annotation annot);




        [DllImport(WfdbPath, EntryPoint = "wfdb_ammap")]
        public static extern byte wfdb_ammap(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_annpos")]
        public static extern int wfdb_annpos(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_isann")]
        public static extern bool wfdb_isann(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_isqrs")]
        public static extern bool wfdb_isqrs(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_mamap")]
        public static extern int wfdb_mamap(int c, int s);

        [DllImport(WfdbPath, EntryPoint = "wfdb_map1")]
        public static extern byte wfdb_map1(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_map2")]
        public static extern byte wfdb_map2(int c);

        [DllImport(WfdbPath, EntryPoint = "wfdb_setannpos")]
        public static extern int wfdb_setannpos(int c, int p);

        [DllImport(WfdbPath, EntryPoint = "wfdb_setisqrs")]
        public static extern bool wfdb_setisqrs(int c, int isQrs);

        [DllImport(WfdbPath, EntryPoint = "wfdb_setmap1")]
        public static extern int wfdb_setmap1(int c, int c2);

        [DllImport(WfdbPath, EntryPoint = "wfdb_setmap2")]
        public static extern int wfdb_setmap2(int c, int c2);

        [DllImport(WfdbPath, EntryPoint = "wfdbcflags")]
        public static extern IntPtr wfdbcflags();

        [DllImport(WfdbPath, EntryPoint = "wfdbdefwfdb")]
        public static extern IntPtr wfdbdefwfdb();

        [DllImport(WfdbPath, EntryPoint = "wfdbdefwfdbcal")]
        public static extern IntPtr wfdbdefwfdbcal();

        [DllImport(WfdbPath, EntryPoint = "wfdberror")]
        public static extern IntPtr wfdberror();

        [DllImport(WfdbPath, EntryPoint = "wfdbfile")]
        public static extern IntPtr wfdbfile(string type, string record);

        [DllImport(WfdbPath, EntryPoint = "wfdbflush")]
        public static extern void wfdbflush();

        [DllImport(WfdbPath, EntryPoint = "wfdbgetskew")]
        public static extern int wfdbgetskew(int s);

        [DllImport(WfdbPath, EntryPoint = "wfdbgetstart")]
        public static extern int wfdbgetstart(int s);

        [DllImport(WfdbPath, EntryPoint = "wfdbinit")]
        public static extern int wfdbinit(string record, [In, Out] Annotator[] aiarray, int nann, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, EntryPoint = "wfdbldflags")]
        public static extern IntPtr wfdbldflags();

        [DllImport(WfdbPath, EntryPoint = "wfdbmemerr")]
        public static extern void wfdbmemerr(int exitOnError);

        [DllImport(WfdbPath, EntryPoint = "wfdbputprolog")]
        public static extern long wfdbputprolog(string prolog, long bytes, int s);

        [DllImport(WfdbPath, EntryPoint = "wfdbquiet")]
        public static extern void wfdbquiet();

        [DllImport(WfdbPath, EntryPoint = "wfdbquit")]
        public static extern void wfdbquit();

        [DllImport(WfdbPath, EntryPoint = "wfdbsetiskew")]
        public static extern void wfdbsetiskew(int s, int skew);

        [DllImport(WfdbPath, EntryPoint = "wfdbsetskew")]
        public static extern void wfdbsetskew(int s, int skew);

        [DllImport(WfdbPath, EntryPoint = "wfdbsetstart")]
        public static extern void wfdbsetstart(int s, long bytes);

        [DllImport(WfdbPath, EntryPoint = "wfdbverbose")]
        public static extern void wfdbverbose();

        [DllImport("wfdb", EntryPoint = "wfdbversion")]
        public static extern IntPtr wfdbversion();

    }
}