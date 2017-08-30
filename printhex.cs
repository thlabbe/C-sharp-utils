using System;
using System.IO;

/**
* Application for printing file to console as hexadecimal.
*/
public class PrintHex {
    
    public static void Main(String[] args) {
        if(args.Length!=2) {
            PrintUsage();
            return;
        }
        
        IEmitter emitter;
        if(args[0] == "-h") {
            emitter = new HexEmitter();
        } else if(args[0] == "-s") {
            emitter = new PerLineEmitter();
        } else {
            PrintUsage();
            return;
        }
        
        try {
            
            string path = args[1];
            
            if(!File.Exists(path)) {
                Console.WriteLine("File does not exist: {0}", path);
                return;
            }
            
            using(FileStream fs = File.OpenRead(path)) {
                while(true) {
                    int b = fs.ReadByte();
                    if(b == -1) {
                        break;
                    }
                    emitter.PrintByte(b);
                }
                emitter.Flush();
            }
            
        } catch(Exception e) {
            Console.WriteLine(e);
        }
    }
    
    public static void PrintUsage() {
        Console.WriteLine("usage: PrintHex.exe -<switch> <filename>");
        Console.WriteLine("  -h  hex dump");
        Console.WriteLine("  -s  one byte per line");
    }
    
}

/**
* Interface for formatting byte output.
*/
public interface IEmitter {
    /**Called for every byte*/
    void PrintByte(int b);
    /**Called after class is finished with*/
    void Flush();
}

/**
* Prints one byte per line as character, decimal and hex value.
*/
public class PerLineEmitter : IEmitter {
    public void PrintByte(int b) {
        char ch = Util.ToSafeAscii(b);
        Console.WriteLine(ch+"\t"+b+"\t"+Util.ToHexString(b));
    }
    public void Flush() {}
}

/**
* Prints multiple bytes per line followed by characters.
*/
public class HexEmitter : IEmitter {
    private const int NBUFF = 16;
    private int buffered = 0;
    private int[] bytes = new int[NBUFF];
    public void PrintByte(int b) {
        bytes[buffered] = b;
        buffered++;
        if(buffered == NBUFF) {
            Flush();
        }
    }
    public void Flush() {
        if(buffered<=0) {
            return;
        }
        
        for(int i=0; i<NBUFF; i++) {
            if(i >= buffered) {
                Console.Write("   ");
            } else {
                string hex = Util.ToHexString(bytes[i]);
                Console.Write(hex);
                Console.Write(" ");
            }
        }
        
        Console.Write("  ");
        
        for(int i=0; i<NBUFF; i++) {
            if(i >= buffered) {
                Console.Write(" ");
            } else {
                char ch = Util.ToSafeAscii(bytes[i]);
                Console.Write(ch);
            }
        }
        
        Console.WriteLine();
        
        buffered = 0;
    }
}

/**
* Utility methods.
*/
public class Util {
    
    /**
    * Converts a byte to a hexadecimal string value.
    */
    public static string ToHexString(int b) {
        const int mask1 = 0x0F;
        const int mask2 = 0xF0;
        
        string ret = "";
        
        int c1 = (b & mask1);
        int c2 = (b & mask2) >> 4;
        
        ret = ret + ToHexChar(c2) + ToHexChar(c1);
        return ret;
    }
    
    /**
    * Converts the given byte to a hex character.
    */
    private static char ToHexChar(int b) {
        const int ascii_zero = 48;
        const int ascii_a = 65;
        
        if(b>=0 && b<=9) {
            return (char) (b + ascii_zero);
        }
        if(b>=10 && b<=15) {
            return (char) (b + ascii_a - 10);
        }
        return '?';
    }
    
    /**
    * Converts the byte to a visible ASCII character or
    * underscore if it is not.
    */
    public static char ToSafeAscii(int b) {
        if(b>=32 && b<=126) {
            return (char) b;
        }
        return '_';
    }
    
}
