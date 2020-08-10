﻿using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Threading;
using Vos;

namespace vosbox
{
    class VosBox : Application
    {
        public VosBox(Process process) : base(process) { }
        public override int Main(string[] args)
        {
            string currentDirectory = "/";
            FileSystem fs = this.Process.OS.FileSystem;
            this.Process.Shell.WriteLine("Hello from VOS");
            while (true)
            {
                this.Process.Shell.Write(currentDirectory + " > ");
                string line = this.Process.Shell.ReadLine();
                string[] a = line.Split(' ');
                if (a[0] == "ls")
                {
                    foreach (string file in this.Process.OS.FileSystem.EnumerateEntries("/"))
                    {
                        this.Process.Shell.WriteLine(file);
                    }
                }
                else if (a[0] == "cd")
                {
                    currentDirectory += a[1];
                }
                else if (a[0] == "csc")
                {
                    this.Process.Shell.WriteLine("VOS compiler");
                    string code = fs.ReadAllText(currentDirectory + a[1]);
                    CSharpCodeProvider provider = new CSharpCodeProvider();
                    CompilerParameters parameters = new CompilerParameters();
                    parameters.OutputAssembly = "bin/ls";
                    parameters.ReferencedAssemblies.Add("voskernel.exe");
                    parameters.GenerateInMemory = false;
                    parameters.GenerateExecutable = false;
                    CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
                }
            }
            return 0;
        }
    }
}
