using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

namespace CodeAnalyzer
{
    public class CodeAnalyzer
    {
        private string _assemblyName;
        private MetadataReference[] _metadataReferences;
        private CSharpCompilation _compilation;
        private SemanticModel _semanticModel;
        private CompilationUnitSyntax _root;
        private SyntaxTree _syntaxTree;
        private SymbolDisplayFormat _symbolDisplayFormat;

        /// <summary>
        /// Simple code analyzer.
        /// Each instance provides information only for code that is passed to contructor, 
        /// to avoid creation of syntax tree and semantic model each time user wants list of used variable types or methods.
        /// </summary>
        /// <param name="code">Code that should be analyzed.</param>
        public CodeAnalyzer (string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            else
            {
                _assemblyName = Path.GetRandomFileName();

                _metadataReferences = new MetadataReference[]{
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
                };

                 _syntaxTree = CSharpSyntaxTree.ParseText(code);

                _compilation = CSharpCompilation.Create(
                       _assemblyName,
                       syntaxTrees: new[] { _syntaxTree },
                       references: _metadataReferences,
                       options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                _semanticModel = _compilation.GetSemanticModel(_syntaxTree);

                _root = (CompilationUnitSyntax)_syntaxTree.GetRoot();

                _symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                
            }
        }

        /// <summary>
        /// Extracts all used variable types from syntax tree. 
        /// Nullable<T> is resolved as System.T?, for example Nullable<int> is resolved as System.Int32?. 
        /// </summary>
        /// <returns>List of all used types in format Namespace.Typename.</returns>
        public List<string> GetVariableTypes()
        {
            return SelectAndAnalyze<VariableDeclarationSyntax>((result, syntaxExpression) => {
                SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression.Type);
                if (syntaxExpression.Type is GenericNameSyntax && symbolInfo.Symbol.Name != "Nullable")
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString());
                }
                else
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString(_symbolDisplayFormat));
                }
            });
        }

        /// <summary>
        /// Creates list of method names used in code. Method name is in format Namespace.MethodName(Parameters).
        /// Constructors are ommited.
        /// </summary>
        /// <returns>List of method names.</returns>
        public List<string> GetMethodNames()
        {
           return  SelectAndAnalyze<ExpressionSyntax>((result, syntaxExpression) => {
               SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression);
      
               if (symbolInfo.Symbol is IMethodSymbol && symbolInfo.Symbol.Name != ".ctor")
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString());
                }
           });
        }

        /// <summary>
        /// Enacpsulates most of logic needed to extract variables and methods.
        /// </summary>
        /// <typeparam name="T">Type of nodes to select.</typeparam>
        /// <param name="action">Specific logic for extraction of methods and variable types.</param>
        /// <returns>List of method names or variable types.</returns>
        private List<string> SelectAndAnalyze<T>(Action<List<string>, T> action) where T: CSharpSyntaxNode
        {
            if (this._root == null && this._semanticModel == null)
            {
                throw new Exception("Value of _root or _semanticModel fields cannot be null.");
            }

            List<T> syntaxSelection = _root.DescendantNodes().OfType<T>().ToList();
            List<string> result = new List<string>();

            if(syntaxSelection.Count > 0)
            {
                foreach(var syntaxExpression in syntaxSelection)
                {
                    SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression);
                    action(result, syntaxExpression);
                }
            }
           
            return result;
        }
    }
}
