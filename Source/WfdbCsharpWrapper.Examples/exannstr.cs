using System;
using System.Runtime.InteropServices;

namespace WfdbCsharpWrapper.Examples
{
    public class exannstr
    {
        static void Main(string[] args)
        {
            UsingPInvoke();
            UsingWrapperClasses();
            Console.ReadLine();
        }

        private static void UsingWrapperClasses()
        {
            Console.WriteLine("Code\tMnemonic\tDescription\n");
            for (int i = 1; i <= AnnotationCode.ACMax; i++)
            {
                Console.WriteLine("{0}\t{1}", i, ((AnnotationCode)i).ToString()); // or .String;
                if (!string.IsNullOrEmpty(((AnnotationCode)i).Description))
                    Console.WriteLine("\t{0}", ((AnnotationCode)i).Description);
                Console.WriteLine();
            }            
        }

        private static void UsingPInvoke()
        {
            Console.WriteLine("Code\tMnemonic\tDescription\n");
            for (int i = 1; i <= AnnotationCode.ACMax; i++)
            {
                Console.WriteLine("{0}\t{1}", i, PInvoke.annstr(i));
                string description = Marshal.PtrToStringAuto(PInvoke.anndesc(i));
                if (!string.IsNullOrEmpty(description))
                    Console.WriteLine("\t{0}", description);
                Console.WriteLine();
            }            
        }
    }
}