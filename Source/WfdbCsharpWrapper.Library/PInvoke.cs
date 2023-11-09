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

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "adumuv")]
        public static extern int adumuv(int s, int a);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "aduphys")]
        public static extern double aduphys(int s, int a);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "anndesc")]
        public static extern IntPtr anndesc(int code);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "annopen")]
        public static extern int annopen(string record, [In, Out] Annotator[] aiarray, int nann);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "annopen")]
        public static extern int annopen(string record, ref Annotator ai, int nann);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "annstr")]
        public static extern IntPtr annstr(int code);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "calopen")]
        public static extern int calopen(string file);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "datstr")]
        public static extern IntPtr datstr(int t);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ecgstr")]
        public static extern IntPtr ecgstr(int code);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "findsig")]
        public static extern int findsig(string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "flushcal")]
        public static extern void flushcal();




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getafreq")]
        public static extern double getafreq();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getann")]
        public static extern int getann(int an, ref Annotation annot);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getbasecount")]
        public static extern double getbasecount();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getcal")]
        public static extern int getcal(string desc, string units, ref CalibrationInfo cal);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getcfreq")]
        public static extern double getcfreq();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getframe")]
        public static extern int getframe(int vec);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getifreq")]
        public static extern double getifreq();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getinfo")]
        public static extern IntPtr getinfo(string record);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getspf")]
        public static extern int getspf();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getvec")]
        public static extern int getvec([In, Out]Sample[] vec);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getwfdb")]
        public static extern IntPtr getwfdb();




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "iannclose")]
        public static extern void iannclose(int an);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "iannsettime")]
        public static extern int iannsettime(int t);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "isgsettime")]
        public static extern int isgsettime(int sgroup, long t);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "isigopen")]
        public static extern int isigopen(string record, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "isigsettime")]
        public static extern int isigsettime(long t);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "mstimstr")]
        public static extern IntPtr mstimstr(long t);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "muvadu")]
        public static extern int muvadu(int s, int v);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "newcal")]
        public static extern int newcal(string file);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "newheader")]
        public static extern int newheader(string record);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "oannclose")]
        public static extern void oannclose(int an);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "osigfopen")]
        public static extern int osigfopen(Signal[] siarray, int nsig);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "osigopen")]
        public static extern int osigopen(string record, [In, Out] Signal[] siarray, int nsig);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "physadu")]
        public static extern int physadu(int s, double v);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "putann")]
        public static extern int putann(int an, ref Annotation annot);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "putcal")]
        public static extern int putcal(ref CalibrationInfo cal);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "putinfo")]
        public static extern int putinfo(string info);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "putvec")]
        public static extern int putvec(int vec);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sampfreq")]
        public static extern double sampfreq(string record);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sample")]
        public static extern int sample(int s, long t);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sample_valid")]
        public static extern int sample_valid();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setafreq")]
        public static extern void setafreq(double f);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setanndesc")]
        public static extern int setanndesc(int code, string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setannstr")]
        public static extern int setannstr(int code, string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setbasecount")]
        public static extern void setbasecount(double count);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setbasetime")]
        public static extern int setbasetime(string time);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setcfreq")]
        public static extern void setcfreq(double freq);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setecgstr")]
        public static extern int setecgstr(int code, string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setgvmode")]
        public static extern void setgvmode(int mode);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setheader")]
        public static extern int setheader(string record, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setibsize")]
        public static extern int setibsize(int size);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setifreq")]
        public static extern void setifreq(double f);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setmsheader")]
        public static extern int setmsheader(string record, [In, Out] string[] snarray, int nsegments);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setobsize")]
        public static extern int setobsize(int size);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setsampfreq")]
        public static extern int setsampfreq(double freq);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setwfdb")]
        public static extern void setwfdb(string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "strann")]
        public static extern byte strann(string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "strdat")]
        public static extern int strdat(string str);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "strecg")]
        public static extern byte strecg(string code);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "strtim")]
        public static extern int strtim(string str);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "timstr")]
        public static extern IntPtr timstr(long t);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "tnextvec")]
        public static extern int tnextvec(int s, long t);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ungetann")]
        public static extern int ungetann(int an,ref Annotation annot);




        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_ammap")]
        public static extern byte wfdb_ammap(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_annpos")]
        public static extern int wfdb_annpos(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_isann")]
        public static extern bool wfdb_isann(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_isqrs")]
        public static extern bool wfdb_isqrs(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_mamap")]
        public static extern int wfdb_mamap(int c, int s);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_map1")]
        public static extern byte wfdb_map1(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_map2")]
        public static extern byte wfdb_map2(int c);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_setannpos")]
        public static extern int wfdb_setannpos(int c, int p);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_setisqrs")]
        public static extern bool wfdb_setisqrs(int c, int isQrs);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_setmap1")]
        public static extern int wfdb_setmap1(int c, int c2);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdb_setmap2")]
        public static extern int wfdb_setmap2(int c, int c2);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbcflags")]
        public static extern IntPtr wfdbcflags();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbdefwfdb")]
        public static extern IntPtr wfdbdefwfdb();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbdefwfdbcal")]
        public static extern IntPtr wfdbdefwfdbcal();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdberror")]
        public static extern IntPtr wfdberror();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbfile")]
        public static extern IntPtr wfdbfile(string type, string record);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbflush")]
        public static extern void wfdbflush();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbgetskew")]
        public static extern int wfdbgetskew(int s);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbgetstart")]
        public static extern int wfdbgetstart(int s);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbinit")]
        public static extern int wfdbinit(string record, [In, Out] Annotator[] aiarray, int nann, [In, Out] Signal[] siarray, int nsig);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbldflags")]
        public static extern IntPtr wfdbldflags();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbmemerr")]
        public static extern void wfdbmemerr(int exitOnError);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbputprolog")]
        public static extern long wfdbputprolog(string prolog, long bytes, int s);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbquiet")]
        public static extern void wfdbquiet();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbquit")]
        public static extern void wfdbquit();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbsetiskew")]
        public static extern void wfdbsetiskew(int s, int skew);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbsetskew")]
        public static extern void wfdbsetskew(int s, int skew);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbsetstart")]
        public static extern void wfdbsetstart(int s, long bytes);

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbverbose")]
        public static extern void wfdbverbose();

        [DllImport(WfdbPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "wfdbversion")]
        public static extern IntPtr wfdbversion();

    }
}