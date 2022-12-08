// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.IO;
using System.Resources;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Commands;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public sealed class SharpSnippetCompilerManager
	{
        private SharpSnippetCompilerManager()
		{
		}
		
		public static void Init()
		{
			var manager = new SharpSnippetCompilerManager();
			var exe = manager.GetType().Assembly;
			
			var rootPath = Path.GetDirectoryName(exe.Location);
			var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SharpSnippetCompiler");
            if (rootPath != null)
            {
                var dataDirectory = Path.Combine(rootPath, "data");

                var startup = new CoreStartup("SharpSnippetCompiler")
                {
                    ConfigDirectory = configDirectory,
                    DataDirectory = dataDirectory,
                    PropertiesName = "SharpSnippetCompiler"
                };

                startup.StartCoreServices();
						
                ResourceService.RegisterNeutralStrings(new ResourceManager("Resources.StringResources", exe));
                ResourceService.RegisterNeutralImages(new ResourceManager("Resources.BitmapResources", exe));

                StringParser.RegisterStringTagProvider(new SharpDevelopStringTagProvider());
			
                var addInFolder = Path.Combine(rootPath, "AddIns");
                startup.AddAddInsFromDirectory(addInFolder);
                startup.RunInitialization();
            }
        }
	}
}
