using Microsoft.CSharp;
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
                if (string.IsNullOrEmpty(line)) continue;
                string[] a = line.Split(' ');
                if (a[0] == "ls")
                {
                    foreach (string file in this.Process.OS.FileSystem.EnumerateEntries(currentDirectory))
                    {
                        this.Process.Shell.WriteLine(file);
                    }
                }
                else if (a[0] == "cd")
                {
                    if (a[1][0] == '/')
                        currentDirectory = a[1];
                    else
                        currentDirectory = string.Format("{0}/{1}", currentDirectory, a[1]).Replace("//", "/");
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
