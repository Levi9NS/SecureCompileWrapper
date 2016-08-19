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

        // Gets full name of variable type in format Namespace.TypaName.
        // For now only works for  PredefinedTypeSyntax (example: System.Int32) and  NullableTypeSyntax(example: System.Int32).
        // For now DOESN'T WORK for GenericNameSyntax (example: Nullable<int>) and IdentifierNameSyntax (example: SomeUserDefinedClass). 
        private string GetVariableTypeName<T> (T variable) where T : PredefinedTypeSyntax, NullableTypeSyntax, GenericNameSyntax, IdentifierNameSyntax
        {
            if(variable != null)
            {
                SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                TypeInfo variableModelInfo = semanticModel.GetTypeInfo(nullable);
                return variableModelInfo.Type.ToDisplayString(symbolDisplayFormat);
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