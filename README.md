WfdbCsharpWrapper
=================
The Wfdb C# Wrapper Library is a .NET library written in C# that encapsulates the WFDB -Waveform Database native interface library. This class library has been developed to allow simplified access to the Waveform databases from any .NET language while offering .NET coding style and standards. It has been tested using both Microsoft's .NET framework and Mono Framework on Linux and Windows Operating Systems.

The code bundle comes with a small set of unit tests along with some sample code that has been ported from the native C bundle. The samples and tests help developers get used to the classes and the underlying architecture of the library.

At this point, the library wraps all the native C functions and their corresponding data structures and allows making calls to the native library using both procedural and OOP coding styles.

Multithreaded access to the signals' data is not supported because of the limitations of the native library.

The source code documentation comes from the WFDB Programmer's Guide by George B Moody.

For more information on the WFDB Library please visit the official website:

http://physionet.org/
