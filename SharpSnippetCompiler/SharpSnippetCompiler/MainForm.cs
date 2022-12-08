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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Commands;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Project.Commands;
using ICSharpCode.SharpSnippetCompiler.Core;
using ICSharpCode.TextEditor;

namespace ICSharpCode.SharpSnippetCompiler
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		public Control ErrorList {
			get {
				if (errorsTabPage.Controls.Count > 0) {
					return errorsTabPage.Controls[0];
				}
				return null;
			}
			set {
				errorsTabPage.Controls.Clear();
				value.Dock = DockStyle.Fill;
				errorsTabPage.Controls.Add(value);
			}
		}

		public Control OutputList {
			get {
				if (outputTabPage.Controls.Count > 0) {
					return outputTabPage.Controls[0];
				}
				return null;
			}
			set {
				outputTabPage.Controls.Clear();
				value.Dock = DockStyle.Fill;
				outputTabPage.Controls.Add(value);
			}
		}
		
		/// <summary>
		/// Gets the active text editor control.
		/// </summary>
		public TextEditorControl TextEditor => ActiveSnippetTabPage.SnippetCompilerControl.TextEditor;

        public SnippetTabPage ActiveSnippetTabPage => fileTabControl.SelectedTab as SnippetTabPage;

        public IViewContent LoadFile(string fileName)
		{
			// Create a new tab page.
			var snippetControl = new SharpSnippetCompilerControl();
			snippetControl.Dock = DockStyle.Fill;
			var tabPage = new SnippetTabPage(snippetControl);
			tabPage.Text = Path.GetFileName(fileName);

			fileTabControl.TabPages.Add(tabPage);
			
			// Load file
			snippetControl.LoadFile(fileName);
			snippetControl.Focus();
			
			var window = new WorkbenchWindow(fileTabControl, tabPage);
			var view = new MainViewContent(fileName, snippetControl, window);
			WorkbenchSingleton.Workbench.ShowView(view);
			
			UpdateActiveView(view);
			
			return view;
		}
		
		public void ActivateErrorList()
		{
			tabControl.SelectedIndex = 0;
		}

		public void ActivateOutputList()
		{
			tabControl.SelectedIndex = 1;
		}

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveAll();
			Close();
		}

        private void BuildCurrentToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveAll();
			var buildSnippet = new BuildSnippetCommand(ProjectService.CurrentProject);
            buildSnippet.Run();
		}

        private void RunToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveAll();
			var execute = new Execute();
			execute.Run();
		}

        private void ContinueToolStripMenuItemClick(object sender, EventArgs e)
		{
			var continueCommand = new ContinueDebuggingCommand();
			continueCommand.Run();
		}

        private void StepOverToolStripMenuItemClick(object sender, EventArgs e)
		{
			var stepCommand = new StepDebuggingCommand();
			stepCommand.Run();
		}

        private void StepIntoToolStripMenuItemClick(object sender, EventArgs e)
		{
			var stepCommand = new StepIntoDebuggingCommand();
			stepCommand.Run();
		}

        private void StepOutToolStripMenuItemClick(object sender, EventArgs e)
		{
			var stepCommand = new StepOutDebuggingCommand();
			stepCommand.Run();
		}

        private void StopToolStripMenuItemClick(object sender, EventArgs e)
		{
			var stopCommand = new StopDebuggingCommand();
			stopCommand.Run();
		}

        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
		{
			var undo = new Undo();
			undo.Run();
		}

        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
		{
			var redo = new Redo();
			redo.Run();
		}

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
		{
			var cut = new Cut();
			cut.Run();
		}

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			var copy = new Copy();
			copy.Run();
		}

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			var paste = new Paste();
			paste.Run();
		}

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			var delete = new Delete();
			delete.Run();
		}

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
		{
			var selectAll = new SelectAll();
			selectAll.Run();
		}

        private void ReferencesToolStripMenuItemClick(object sender, EventArgs e)
		{
			var project = ProjectService.CurrentProject;
			using (var referenceDialog = new SelectReferenceDialog(project)) {
				
				// Add existing project references to dialog.
				var existingReferences = GetReferences(project);
				AddReferences(referenceDialog, existingReferences);

				var result = referenceDialog.ShowDialog();
				if (result == DialogResult.OK) {

					var selectedReferences = referenceDialog.ReferenceInformations;

                    // Add new references.
                    foreach (ReferenceProjectItem reference in selectedReferences) {
                        if (!existingReferences.Contains(reference)) {
                            ProjectService.AddProjectItem(project, reference);
                        }
                    }
                    
                    // Remove any references removed in the select reference dialog.
					foreach (var existingReference in existingReferences) {
						if (!selectedReferences.Contains(existingReference)) {
							ProjectService.RemoveProjectItem(project, existingReference);
						}
					}
										
					project.Save();
				}
			}
		}

        private List<ReferenceProjectItem> GetReferences(IProject project)
		{
			var references = new List<ReferenceProjectItem>();
			foreach (var item in project.Items) {
                if (item is ReferenceProjectItem reference) {
					references.Add(reference);
				}
			}
			return references;
		}

        private void AddReferences(ISelectReferenceDialog dialog, List<ReferenceProjectItem> references)
		{
			foreach (var reference in references) {
				dialog.AddReference(reference.Include, "Gac", reference.FileName, reference);
			}
		}

        private void UpdateActiveView(IViewContent view)
		{
            if (!(WorkbenchSingleton.Workbench is Workbench workbench)) return;
            workbench.ActiveViewContent = view;
            workbench.ActiveContent = view;
        }

        private void FileTabControlSelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateActiveView();
		}

        private void UpdateActiveView()
		{
			if (ActiveSnippetTabPage != null) {
				var control = ActiveSnippetTabPage.SnippetCompilerControl;
				foreach (var view in WorkbenchSingleton.Workbench.ViewContentCollection) {
					if (view.Control == control) {
						UpdateActiveView(view);
						return;
					}
				}
			} else {
				UpdateActiveView(null);
			}
		}

        private void SaveAll()
		{
			foreach (SnippetTabPage tabPage in fileTabControl.TabPages) {
				tabPage.SnippetCompilerControl.Save();
			}
		}

        private void FileNewToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (var dialog = new NewFileDialog()) {
				dialog.FileName = GetNewFileName();
				if (dialog.ShowDialog() == DialogResult.OK) {
					var fileName = dialog.FileName;
					using (var file = File.CreateText(fileName)) {
						file.Write(String.Empty);
					}
					LoadFile(fileName);
					AddFileToProject(fileName);
				}
			}
		}

        private string GetNewFileName()
		{
			var fileName = SnippetCompilerProject.GetFullFileName("Snippet1.cs");
			var baseFolder = Path.GetDirectoryName(fileName);
			var count = 1;
			while (File.Exists(fileName))
            {
                count++;
                if (baseFolder != null) 
                    fileName = Path.Combine(baseFolder, $"Snippet{count}.cs");
            }
			return fileName;
		}

        private void FileOpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog()) {
				dialog.CheckFileExists = true;
				if (dialog.ShowDialog() == DialogResult.OK) {
					foreach (var fileName in dialog.FileNames) {
						LoadFile(fileName);
						AddFileToProject(fileName);
					}
				}
			}
		}

        private void AddFileToProject(string fileName)
		{
			var project = ProjectService.CurrentProject;
			var item = new FileProjectItem(project, ItemType.Compile, fileName);
			ProjectService.AddProjectItem(project, item);
			project.Save();
		}

        private void FileCloseToolStripMenuItemClick(object sender, EventArgs e)
		{
			var activeTabPage = ActiveSnippetTabPage;
			if (activeTabPage != null) {
				var snippetControl = activeTabPage.SnippetCompilerControl;
				snippetControl.Save();
				var fileName = ActiveSnippetTabPage.SnippetCompilerControl.TextEditor.FileName;
				var project = ProjectService.CurrentProject;
				var item = project.FindFile(fileName);
				if (item != null) {
					ProjectService.RemoveProjectItem(project, item);
					project.Save();
					
					foreach (var view in WorkbenchSingleton.Workbench.ViewContentCollection) {
						if (view.Control == snippetControl) {
							WorkbenchSingleton.Workbench.CloseContent(view);
							break;
						}
					}
						
					fileTabControl.TabPages.Remove(activeTabPage);
					activeTabPage.Dispose();
				}
			}
		}
	}
}
