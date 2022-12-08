// SharpDevelop samples
// Copyright (c) 2010, AlphaSierraPapa
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list
//   of conditions and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list
//   of conditions and the following disclaimer in the documentation and/or other materials
//   provided with the distribution.
//
// - Neither the name of the SharpDevelop team nor the names of its contributors may be used to
//   endorse or promote products derived from this software without specific prior written
//   permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
// IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpSnippetCompiler.Core;

namespace ICSharpCode.SharpSnippetCompiler
{
	public sealed class Program
	{
        private MainForm _mainForm;
		
		[STAThread]
        private static void Main()
		{
			var program = new Program();
			program.Run();
		}

        private void Run()
		{						
			SharpSnippetCompilerManager.Init();
		
			_mainForm = new MainForm();
			
			// Force creation of the debugger before workbench is created.
            // ReSharper disable once UnusedVariable
            var debugger = DebuggerService.CurrentDebugger;
			
			var workbench = new Workbench(_mainForm);
			WorkbenchSingleton.InitializeWorkbench(workbench, new WorkbenchLayout());
			var errorList = workbench.GetPad(typeof(ErrorListPad));
			errorList.CreatePad();
			_mainForm.ErrorList = errorList.PadContent.Control;			
			
			var outputList = workbench.GetPad(typeof(CompilerMessageView));
			outputList.CreatePad();
			_mainForm.OutputList = outputList.PadContent.Control;
			
			_mainForm.Visible = true;
			
			SnippetCompilerProject.Load();
			var project = GetCurrentProject();
			ProjectService.CurrentProject = project;
			LoadFiles(project);
			
			ParserService.StartParserThread();
			
			//WorkbenchSingleton.SafeThreadAsyncCall(new Action<Program>(LoadFile));
							
			try {
				Application.Run(_mainForm);
			} finally {
				try {
					// Save properties
					//PropertyService.Save();
				} catch (Exception ex) {
					MessageService.ShowError(ex, "Properties could not be saved.");
				}
			}
		}

        private void LoadFiles(IProject project)
        {
            var projectItems = project
                .Items
                .OfType<FileProjectItem>()
                .Where(o => File.Exists(o.FileName));

            foreach (var projectItem in projectItems)
                _mainForm.LoadFile(projectItem.FileName);
		}

        private IProject GetCurrentProject()=> ProjectService.OpenSolution.Projects.FirstOrDefault();

	}
}
