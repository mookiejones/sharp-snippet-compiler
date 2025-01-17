// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
// </file>

using System;
using System.IO;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Internal.Templates;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public class SnippetCompilerProject : CompilableProject
	{
        private static readonly string DefaultSnippetSource = "using System;\r\n\r\n" +
                                                              "public class Program\r\n" +
                                                              "{\r\n" +
                                                              "\t[STAThread]\r\n" +
                                                              "\tstatic void Main(string[] args)\r\n" +
                                                              "\t{\r\n" +
                                                              "\t}\r\n" +
                                                              "}";

        private SnippetCompilerProject() : base(new Solution())
		{
			Create();			
		}

        private static string SnippetFileName => GetFullFileName("Snippet.cs");

        private static string SnippetProjectFileName => GetFullFileName("Snippet.csproj");

        public override LanguageProperties LanguageProperties => LanguageProperties.None;

        public override string Language => "C#";

        private const string DefaultTargetsFile = @"$(MSBuildBinPath)\Microsoft.CSharp.Targets";

		protected override void Create(ProjectCreateInformation information)
		{
			AddImport(DefaultTargetsFile, null);
			
			// Add import before base.Create call - base.Create will call AddOrRemoveExtensions, which
			// needs to change the import when the compact framework is targeted.
			base.Create(information);
			
			SetProperty("Debug", null, "CheckForOverflowUnderflow", "True",
			            PropertyStorageLocations.ConfigurationSpecific, true);
			SetProperty("Release", null, "CheckForOverflowUnderflow", "False",
			            PropertyStorageLocations.ConfigurationSpecific, true);
			
			SetProperty("Debug", null, "DefineConstants", "DEBUG;TRACE",
			            PropertyStorageLocations.ConfigurationSpecific, false);
			SetProperty("Release", null, "DefineConstants", "TRACE",
			            PropertyStorageLocations.ConfigurationSpecific, false);
		}
		
		public override ItemType GetDefaultItemType(string fileName)
		{
			if (string.Equals(Path.GetExtension(fileName), ".cs", StringComparison.OrdinalIgnoreCase)) {
				return ItemType.Compile;
			} else {
				return base.GetDefaultItemType(fileName);
			}
		}
		
		public static void Load()
		{
			CreateSnippetProject();
			CreateSnippetFile();
			ProjectService.LoadProject(SnippetProjectFileName);			
		}

        private void Create()
		{
			var info = new ProjectCreateInformation
            {
                Solution = new Solution(),
                OutputProjectFileName = Path.Combine(PropertyService.ConfigDirectory, "SharpSnippet.exe"),
                ProjectName = "SharpSnippet"
            };
            Create(info);			
			Parent = info.Solution;
		}
		
		/// <summary>
		/// Loads the snippet project or creates one if it does not already exist.
		/// </summary>
        private static void CreateSnippetProject()
		{
			var fileName = SnippetProjectFileName;
			if (!File.Exists(fileName)) {
							
				// Add single snippet file to project.
				var project = new SnippetCompilerProject();
				var item = new FileProjectItem(project, ItemType.Compile, "Snippet.cs");
				ProjectService.AddProjectItem(project, item);
				
				project.Save(fileName);
			}
		}
	
		/// <summary>
		/// Loads the snippet file or creates one if it does not already exist. 
		/// </summary>
        private static void CreateSnippetFile()
		{
			var fileName = SnippetFileName;
			if (!File.Exists(fileName)) {
				LoggingService.Info($"Creating Snippet.cs file: {fileName}");
				using (var snippetFile = File.CreateText(fileName)) {
					snippetFile.Write(DefaultSnippetSource);
				}
			}
		}
		
		/// <summary>
		/// All snippet compiler files are stored loaded from the config directory so this
		/// method prefixes the filename with this path.
		/// </summary>
		public static string GetFullFileName(string fileName)
		{
			return Path.Combine(PropertyService.ConfigDirectory, fileName);
		}
	}
}
