using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_Sharp_Server
{
    class Protocol
    {
        // the delimiter
        //private char[] DELIMITER = { (char)27, (char)27 };
        /// <summary>
        /// A simple package for each Protocol, its let you store 
        /// header info and body:
        /// Name
        /// ID
        /// and last Body
        /// </summary>
        public class Package
        {
            string playerID;
            public string Name { get{ return playerID;}}
            int id;
            public int ID { get {return id;}}
            string body;
            public string Body { get { return body;}}
            public Package(string name, int id, string body)
            {
                this.playerID = name;
                if (id != -1)
                    this.id = id;
                else
                    this.id = System.Threading.Thread.CurrentThread.ManagedThreadId;
                this.body = body;
            }
        }
        /// <summary>
        /// Makes a package from a string
        /// </summary>
        /// <param name="s">The string you want in package body</param>
        /// <returns>A package string with Name, ID and body</returns>
        public string MakePackage(string s)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--ServerSent-- " + "0..0.." + s);
            Console.ForegroundColor = ConsoleColor.White;
            return "0..0.." + s;
        }
        /// <summary>
        /// Convert String to package
        /// </summary>
        /// <param name="s">The string you want in the body</param>
        /// <returns>A full made packet!</returns>
        public Package GetPackage(string s)
        {
            if (s.Length > 6)
            {
                string[] tmp = s.Split("..".ToCharArray());
                try
                {
                    return new Package(tmp[0], Convert.ToInt32(tmp[2]), tmp[4]);
                }
                catch
                {
                    Console.WriteLine("System Error");
                    return new Package("Ermac", 0, "This is an spam");
                }
            }
            else
            {
                return new Package("Ermac", 0, "This is an spam");
            }
        }
    }
}
