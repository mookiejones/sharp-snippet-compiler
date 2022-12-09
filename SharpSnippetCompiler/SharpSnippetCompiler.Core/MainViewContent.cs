// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.DefaultEditor.Gui.Editor;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public sealed class MainViewContent : IViewContent, ITextEditorControlProvider, IClipboardHandler, IUndoHandler, IPositionable, IParseInformationListener, IEditable
	{
        private readonly SharpSnippetCompilerControl _snippetControl;
        private readonly SnippetFile _file;
		
		public MainViewContent(string fileName, SharpSnippetCompilerControl snippetControl, IWorkbenchWindow workbenchWindow)
		{
			_file = new SnippetFile(fileName);
			_snippetControl = snippetControl;
			TextEditorControl = snippetControl.TextEditor;
			WorkbenchWindow = workbenchWindow;
			WorkbenchWindow.ActiveViewContent = this;
		}
		
		public event EventHandler TabPageTextChanged;
		public event EventHandler TitleNameChanged;
		public event EventHandler Disposed;		
		public event EventHandler IsDirtyChanged;
		
		public bool EnableUndo => TextEditorControl.EnableUndo;

        public bool EnableRedo => TextEditorControl.EnableRedo;

        public void Undo()
		{
			TextEditorControl.Undo();
		}
		
		public void Redo()
		{
			TextEditorControl.Redo();
		}
	
		public bool EnableCut => TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCut;

        public bool EnableCopy => TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCopy;

        public bool EnablePaste => TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.EnablePaste;

        public bool EnableDelete => TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableDelete;

        public bool EnableSelectAll => TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableSelectAll;

        public void Cut()
		{
			TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null, null);
		}
		
		public void Copy()
		{
			TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
		}
		
		public void Paste()
		{
			TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
		}
		
		public void Delete()
		{
			TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
		}
		
		public void SelectAll()
		{
			TextEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(null, null);
		}
				
		public TextEditorControl TextEditorControl { get; }

        public Control Control => _snippetControl;

        public IWorkbenchWindow WorkbenchWindow { get; set; }

        public string TabPageText => throw new NotImplementedException();

        public string TitleName => throw new NotImplementedException();

        public IList<OpenedFile> Files => throw new NotImplementedException();

        public OpenedFile PrimaryFile => _file;

        public string PrimaryFileName => _file.FileName;

        public bool IsDisposed => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsViewOnly => throw new NotImplementedException();

        public ICollection<IViewContent> SecondaryViewContents => throw new NotImplementedException();

        public bool IsDirty => throw new NotImplementedException();

        public void RedrawContent()
		{
			throw new NotImplementedException();
		}
		
		public void Save(OpenedFile file, Stream stream)
		{
			throw new NotImplementedException();
		}
		
		public void Load(OpenedFile file, Stream stream)
		{
			throw new NotImplementedException();
		}
		
		public INavigationPoint BuildNavPoint()
		{
			throw new NotImplementedException();
		}
		
		public bool SupportsSwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView)
		{
			throw new NotImplementedException();
		}
		
		public bool SupportsSwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView)
		{
			throw new NotImplementedException();
		}
		
		public void SwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView)
		{
			throw new NotImplementedException();
		}
		
		public void SwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView)
		{
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
		}
		
		public IDocument GetDocumentForFile(OpenedFile file)
		{
			return null;
		}		
		
		public void JumpTo(int line, int column)
		{
			TextEditorControl.ActiveTextAreaControl.JumpTo(line, column);
			
//			// we need to delay this call here because the text editor does not know its height if it was just created
//			WorkbenchSingleton.SafeThreadAsyncCall(
//				delegate {
//					textEditor.ActiveTextAreaControl.CenterViewOn(
//						line, (int)(0.3 * textEditor.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount));
//				});
		}
		
		public int Line => TextEditorControl.ActiveTextAreaControl.Caret.Line;

        public int Column => TextEditorControl.ActiveTextAreaControl.Caret.Column;

        public void ParseInformationUpdated(ParseInformation parseInfo)
		{
			if (TextEditorControl.TextEditorProperties.EnableFolding) {
				WorkbenchSingleton.SafeThreadAsyncCall(ParseInformationUpdatedInvoked, parseInfo);
			}
		}

        private delegate string GetTextHelper();
	
		public string Text {
			get {
				if (WorkbenchSingleton.InvokeRequired) {
					return (string)WorkbenchSingleton.MainForm.Invoke(new GetTextHelper(GetText));
					//return WorkbenchSingleton.SafeThreadFunction<string>(GetText);
				} else {
					return GetText();

				}
			}
			set {
				if (WorkbenchSingleton.InvokeRequired) {
					WorkbenchSingleton.SafeThreadCall(SetText, value);
				} else {
					SetText(value);
				}
			}
		}

        private void OnTabPageTextChanged(EventArgs e)
		{
			if (TabPageTextChanged != null) {
				TabPageTextChanged(this, e);
			}
		}

        private void OnTitleNameChanged(EventArgs e)
		{
			if (TitleNameChanged != null) {
				TitleNameChanged(this, e);
			}
		}

        private void OnDisposed(EventArgs e)
		{
			if (Disposed != null) {
				Disposed(this, e);
			}
		}

        private void OnIsDirtyChanged(EventArgs e)
		{
			if (IsDirtyChanged != null) {
				IsDirtyChanged(this, e);
			}
		}

        private void ParseInformationUpdatedInvoked(ParseInformation parseInfo)
		{
			try {
				TextEditorControl.Document.FoldingManager.UpdateFoldings(_file.FileName, parseInfo);
				TextEditorControl.ActiveTextAreaControl.TextArea.Refresh(TextEditorControl.ActiveTextAreaControl.TextArea.FoldMargin);
				TextEditorControl.ActiveTextAreaControl.TextArea.Refresh(TextEditorControl.ActiveTextAreaControl.TextArea.IconBarMargin);
			} catch (Exception ex) {
				MessageService.ShowError(ex);
			}
		}

        private string GetText()
		{
			return TextEditorControl.Document.TextContent;
		}

        private void SetText(string value)
		{
			TextEditorControl.Document.Replace(0, TextEditorControl.Document.TextLength, value);
		}		
	}
}
