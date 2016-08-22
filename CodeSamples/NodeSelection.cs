// Add nugget package Microsoft.CodeAnalysis 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

namespace SecureCompileWrapper
{
    public class NodeSelection
    {
        private string _assemblyName;
        private MetadataReference[] _metadataReferences;
        private CSharpCompilation _compilation;
        private SemanticModel _semanticModel;
        private CompilationUnitSyntax _root; 

        public NodeSelection(string sourceCode)
        {
            if(sourceCode != null)
            {
                _assemblyName = Path.GetRandomFileName();

                _metadataReferences = new MetadataReference[]{
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
                };

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(txInput.Text);

                _compilation = CSharpCompilation.Create (
                        assemblyName,
                        syntaxTrees: new[] { syntaxTree },
                        references: references,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                _semanticModel = ompilation.GetSemanticModel(_syntaxTree);

                _root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            }
            else
            {
                throw new ArgumentNullException(nameof(sourceCode));
            }
        }

        private List<T> Select<T>() where T: PredefinedTypeSyntax, NullableTypeSyntax, GenericNameSyntax, IdentifierNameSyntax
        {
            return root.DescendantNodes().OfType<T>().ToList<T>;
        }

        private List<VariableDeclarationSyntax> SelectVariables()
        {
            return _root.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList<VariableDeclarationSyntax>;
        }

        
        private string GetVariableTypeName (VariableDeclarationSyntax variable) 
        {
            string result = string.Empty;
            SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

            if (variable != null)
            {
                SymbolInfo simbolInfo = semanticModel.GetSymbolInfo(referenceVariable.Type);

                if(simbolInfo.Symbol != null)
                {
                    if (variable is GenericNameSyntax)
                    {
                        result = symbolInfo.Symbol.ToDisplayString();
                    }
                    else
                    {
                       result = symbolInfo.Symbol.ToDisplayString(symbolDisplayFormat);
                    }
                }
             
                return result;
            }
            else
            {
                throw new ArgumentNullException(nameof(variable));
            }
        }

        public List<string> GetTypeNamesForSyntaxType<T>() where T : PredefinedTypeSyntax, NullableTypeSyntax, GenericNameSyntax, IdentifierNameSyntax
        {
            List<string> result = new List<string>();
            List<T> variables = Select<T>();

            if (variables != null && variables.Count > 0)
            {
                foreach(var variable in variables)
                {
                    result.Add(GetVariableTypeName<T>(variable));
                }
            }

            return result;
        }
    }
}