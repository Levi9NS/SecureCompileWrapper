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

namespace SecureCompileWrapper
{
    public class CodeAnalyzer
    {
        private string _assemblyName;
        private MetadataReference[] _metadataReferences;
        private CSharpCompilation _compilation;
        private SemanticModel _semanticModel;
        private CompilationUnitSyntax _root;
        private SyntaxTree _syntaxTree;
        private SymbolDisplayFormat _variableSymbolDisplayFormat;
        private SymbolDisplayFormat _methodSymbolDisplayFormt;
        private Configuration _configuration;

        /// <summary>
        /// Simple code analyzer.
        /// Each instance provides information only for code that is passed to contructor, 
        /// to avoid creation of syntax tree and semantic model each time user wants list of used variable types or methods.
        /// </summary>
        /// <param name="code">Code that should be analyzed.</param>
        public CodeAnalyzer(string code, Configuration configuration = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            else
            {
                _configuration = configuration;
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

                // More information on SymbolDisplayFormat can be found at http://source.roslyn.io/#Microsoft.CodeAnalysis/SymbolDisplay/SymbolDisplayFormat.cs,9ba04f3b6d792331
                _variableSymbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                _methodSymbolDisplayFormt = new SymbolDisplayFormat(
                                                    globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
                                                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                                                    propertyStyle: SymbolDisplayPropertyStyle.ShowReadWriteDescriptor,
                                                    localOptions: SymbolDisplayLocalOptions.IncludeType,
                                                    genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeVariance,
                                                    memberOptions:
                                                        SymbolDisplayMemberOptions.IncludeParameters |
                                                        SymbolDisplayMemberOptions.IncludeContainingType |
                                                        SymbolDisplayMemberOptions.IncludeType |
                                                        SymbolDisplayMemberOptions.IncludeExplicitInterface,
                                                    kindOptions:
                                                        SymbolDisplayKindOptions.IncludeMemberKeyword,
                                                    parameterOptions:
                                                        SymbolDisplayParameterOptions.IncludeOptionalBrackets |
                                                        SymbolDisplayParameterOptions.IncludeDefaultValue |
                                                        SymbolDisplayParameterOptions.IncludeParamsRefOut |
                                                        SymbolDisplayParameterOptions.IncludeExtensionThis |
                                                        SymbolDisplayParameterOptions.IncludeType |
                                                        SymbolDisplayParameterOptions.IncludeName,
                                                    miscellaneousOptions:
                                                        SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
                                                        SymbolDisplayMiscellaneousOptions.UseErrorTypeSymbolName
                );
            }
        }

        /// <summary>
        /// Extracts all used variable types from syntax tree. 
        /// Nullable<T> is resolved as System.T?, for example Nullable<int> is resolved as System.Int32?. 
        /// </summary>
        /// <returns>List of all used types in format Namespace.Typename.</returns>
        public List<string> GetUsedVariableTypes()
        {
            return SelectAndAnalyze<VariableDeclarationSyntax>((result, syntaxExpression) => {
                SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression.Type);
                if (syntaxExpression.Type is GenericNameSyntax && symbolInfo.Symbol.Name != "Nullable")
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString());
                }
                else
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString(_variableSymbolDisplayFormat));
                }
            });
        }

        /// <summary>
        /// Creates list of method names defined or invoked in code. Method name is in format "ReturnValue Namespace.MethodName(Parameters)".
        /// Constructors are ommited.
        /// </summary>
        /// <returns>List of method names.</returns>
        public List<string> GetNamesForDefinedOrCalledMethods()
        {
            return SelectAndAnalyze<ExpressionSyntax>((result, syntaxExpression) => {
                SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression);

                if (symbolInfo.Symbol is IMethodSymbol && symbolInfo.Symbol.Name != ".ctor")
                {
                    result.Add(symbolInfo.Symbol.ToDisplayString(_methodSymbolDisplayFormt));
                }
            });
        }

        /// <summary>
        /// Enacpsulates most of logic needed to extract variables and methods.
        /// </summary>
        /// <typeparam name="T">Type of nodes to select.</typeparam>
        /// <param name="action">Specific logic for extraction of methods and variable types.</param>
        /// <returns>List of method names or variable types.</returns>
        private List<string> SelectAndAnalyze<T>(Action<List<string>, T> action) where T : CSharpSyntaxNode
        {
            if (this._root == null || this._semanticModel == null)
            {
                throw new Exception("Value of _root or _semanticModel fields cannot be null.");
            }

            List<T> syntaxSelection = _root.DescendantNodes().OfType<T>().ToList();
            List<string> result = new List<string>();

            if (syntaxSelection.Count > 0)
            {
                foreach (var syntaxExpression in syntaxSelection)
                {
                    SymbolInfo symbolInfo = _semanticModel.GetSymbolInfo(syntaxExpression);
                    action(result, syntaxExpression);
                }
            }

            return result;
        }

        /// <summary>
        /// Utility method for counting accourances of specific syntax parts.
        /// One example of usage is when you want to check how many types were defined in code,
        /// you can pass TypeDeclarationSyntax as type parameter, and depending on result decide to execute that code or not.
        /// </summary>
        /// <typeparam name="T">Type parameter derived from CSharpSyntaxNode.</typeparam>
        /// <returns>Count of accourances of specific syntax type in given code.</returns>
        public int Count<T>() where T : CSharpSyntaxNode
        {
            return _root.DescendantNodes().OfType<T>().Count();
        }

        /// <summary>
        /// Determines is given code safe to compile, according to provided configuration.
        /// </summary>
        /// <returns>Result of precompilation analysis in form of instance of PrecompilationAnalysisResult class.</returns>
        public PrecompilationAnalysisResult PerformPrecompilationAnalysis()
        {
            PrecompilationAnalysisResult result = new PrecompilationAnalysisResult();

            if (_configuration == null)
            {
                // In case when _configuration is not supplied, it is assumed that there are no precompilation constraints. 
                result.AllowedVariableTypesCheckResult.IsUseOfUnallowedSyntaxElementsDetected = false;
                result.AllowedMethodDefinitionsAndCallsCheck.IsUseOfUnallowedSyntaxElementsDetected = false;
            }
            else
            {
                // Used variable types analysis
                result.AllowedVariableTypesCheckResult = this.PerformSyntaxElementAnalysis(this.GetUsedVariableTypes(), _configuration.AllowedVariableTypes);
                // Used method definition or calls analysis
                result.AllowedMethodDefinitionsAndCallsCheck = this.PerformSyntaxElementAnalysis(this.GetNamesForDefinedOrCalledMethods(), _configuration.AllowedMethodDefinitionsAndCalls);
            }

            return result;
        }

        /// <summary>
        /// Performs analysis of used syntax elements comparing it to allowed ones.
        /// If list of allowed syntax elements has null value, it is assumed that there is no constraints regarding that syntax element group,
        /// in that case it will be determined that no unallowed syntax elements where detected, and the list of used unallowed elements will be empty.
        /// </summary>
        /// <param name="usedSyntaxElements">Syntax elements that were used in code.</param>
        /// <param name="allowedSyntaxElements">Allowed syntax elements.</param>
        /// <returns>Result in form of AllowedSyntaxElementCheckResult object.</returns>
        private AllowedSyntaxElementCheckResult PerformSyntaxElementAnalysis(List<string> usedSyntaxElements, List<string> allowedSyntaxElements)
        {
            AllowedSyntaxElementCheckResult result = new AllowedSyntaxElementCheckResult();

            if(allowedSyntaxElements != null)
            {
                result = this.AreSyntaxElementsAllowed(usedSyntaxElements, allowedSyntaxElements);
            }
            else
            {
                result.IsUseOfUnallowedSyntaxElementsDetected = false;
            }

            return result;
        }

        /// <summary>
        /// Cheks if syntax element can be found in given list of allowed syntax elements.
        /// </summary>
        /// <param name="syntaxElement">Element that should be checked.</param>
        /// <param name="allowedSyntaxElements">List of syntax elements against which syntax element check is performed.</param>
        /// <returns>Indication if syntax element is allowed.</returns>
        private bool IsSyntaxElementAllowed(string syntaxElement, List<string> allowedSyntaxElements)
        {
            bool result = false;

            foreach (var allowedSyntaxElement in allowedSyntaxElements)
            {
                if (syntaxElement == allowedSyntaxElement)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Compares list of used syntax element against compatible list of alowed syntax elements.
        /// </summary>
        /// <param name="usedSyntaxElements">List of strings that represents used syntax elements.</param>
        /// <param name="allowedSyntaxElements">List of strings that represents allowed syntax elements</param>
        /// <returns>False if even one of used syntax elements is not in the list of allowed syntax elements, otherwise true.</returns>
        private AllowedSyntaxElementCheckResult AreSyntaxElementsAllowed(List<string> usedSyntaxElements, List<string> allowedSyntaxElements)
        {
            AllowedSyntaxElementCheckResult result = new AllowedSyntaxElementCheckResult { IsUseOfUnallowedSyntaxElementsDetected = false };

            foreach (string usedSyntaxElement in usedSyntaxElements)
            {
                if (IsSyntaxElementAllowed(usedSyntaxElement, allowedSyntaxElements) == false)
                {
                    result.IsUseOfUnallowedSyntaxElementsDetected = true;
                    result.UsedUnalowedSyntaxElements.Add(usedSyntaxElement);
                }
            }

            return result;
        }
    }

    public class AllowedSyntaxElementCheckResult
    {
        public bool IsUseOfUnallowedSyntaxElementsDetected { get; set; } = true;
        public List<string> UsedUnalowedSyntaxElements { get; set; } = new List<string>();
    }

    public class PrecompilationAnalysisResult
    {
        public AllowedSyntaxElementCheckResult AllowedVariableTypesCheckResult { get; set; }
        public AllowedSyntaxElementCheckResult AllowedMethodDefinitionsAndCallsCheck { get; set; }
    }
}