using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace Client.Trigger
{
    public class Trigger
    {
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public string Regex { get; set; }
        private Regex _compiledRegex { get; set; }
        [JsonIgnoreAttribute]
        public Regex RegexCompiled
        {
            get
            {
                if (_compiledRegex == null)
                {
                    _compiledRegex = new Regex(Regex, RegexOptions.Compiled);
                }
                return _compiledRegex;
            }
            set
            {
                _compiledRegex = value;
            }
        }
        public string Code { get; set; }
        private Func<string, string> _compiledCode { get; set; }
        [JsonIgnoreAttribute]
        public Func<string, string> CompiledCode
        {
            get
            {
                if (_compiledCode == null)
                {
                    _compiledCode = CompileMethod(Code);
                }
                return _compiledCode;
            }
            set
            {
                _compiledCode = value;
            }
        }


        private Func<string, string> CompileMethod(string method)
        {
            CodeDomProvider provider = CSharpCodeProvider.CreateProvider("c#");
            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.CompilerOptions = "/optimize";
            string assemblyContainingNotDynamicClass = Assembly.GetExecutingAssembly().Location;
            options.ReferencedAssemblies.Add(assemblyContainingNotDynamicClass);
            CompilerResults results = provider.CompileAssemblyFromSource(options, new[] { method });

            Assembly assembly = null;
            if (!results.Errors.HasErrors)
            {
                assembly = results.CompiledAssembly;
            }
            else
            {
                string errorMessage = string.Format("Error generating method: {0}" + Environment.NewLine + "Error message: {1}", method, results.Errors[0].ErrorText);
                throw new Exception(errorMessage);
            }


            Type type = assembly.GetType("DynamicClass");
            MethodInfo methodInfo = type.GetMethod("EvaluateDynamic");

            return (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), methodInfo);
        }
    }
}
