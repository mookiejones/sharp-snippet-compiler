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
	public class MainViewContent : IViewContent, ITextEditorControlProvider, IClipboardHandler, IUndoHandler, IPositionable, IParseInformationListener, IEditable
	{
        private IWorkbenchWindow _workbenchWindow;
        private TextEditorControl _textEditor;
        private SharpSnippetCompilerControl _snippetControl;
        private SnippetFile _file;
		
		public MainViewContent(string fileName, SharpSnippetCompilerControl snippetControl, IWorkbenchWindow workbenchWindow)
		{
			_file = new SnippetFile(fileName);
			this._snippetControl = snippetControl;
			this._textEditor = snippetControl.TextEditor;
			this._workbenchWindow = workbenchWindow;
			this._workbenchWindow.ActiveViewContent = this;
		}
		
		public event EventHandler TabPageTextChanged;
		public event EventHandler TitleNameChanged;
		public event EventHandler Disposed;		
		public event EventHandler IsDirtyChanged;
		
		public bool EnableUndo {
			get { return _textEditor.EnableUndo; }
		}
		
		public bool EnableRedo {
			get { return _textEditor.EnableRedo; }
		}
		
		public void Undo()
		{
			_textEditor.Undo();
		}
		
		public void Redo()
		{
			_textEditor.Redo();
		}
	
		public bool EnableCut {
			get { return _textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCut; }
		}
		
		public bool EnableCopy {
			get { return _textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCopy; }
		}
		
		public bool EnablePaste {
			get { return _textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnablePaste; }
		}
		
		public bool EnableDelete {
			get { return _textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableDelete; }
		}
		
		public bool EnableSelectAll {
			get { return _textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableSelectAll; }
		}
		
		public void Cut()
		{
			_textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null, null);
		}
		
		public void Copy()
		{
			_textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
		}
		
		public void Paste()
		{
			_textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
		}
		
		public void Delete()
		{
			_textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
		}
		
		public void SelectAll()
		{
			_textEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(null, null);
		}
				
		public TextEditorControl TextEditorControl {
			get { return _textEditor; }
		}
		
		public Control Control {
			get { return _snippetControl; }
		}
		
		public IWorkbenchWindow WorkbenchWindow {
			get { return _workbenchWindow; }
			set { _workbenchWindow = value; }
		}
		
		public string TabPageText {
			get {
				throw new NotImplementedException();
			}
		}
		
		public string TitleName {
			get {
				throw new NotImplementedException();
			}
		}
		
		public IList<OpenedFile> Files {
			get {
				throw new NotImplementedException();
			}
		}
		
		public OpenedFile PrimaryFile {
			get { return _file; }
		}
		
		public string PrimaryFileName {
			get { return _file.FileName; }
		}
		
		public bool IsDisposed {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool IsReadOnly {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool IsViewOnly {
			get {
				throw new NotImplementedException();
			}
		}
		
		public ICollection<IViewContent> SecondaryViewContents {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool IsDirty {
			get {
				throw new NotImplementedException();
			}
		}
		
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
			_textEditor.ActiveTextAreaControl.JumpTo(line, column);
			
//			// we need to delay this call here because the text editor does not know its height if it was just created
//			WorkbenchSingleton.SafeThreadAsyncCall(
//				delegate {
//					textEditor.ActiveTextAreaControl.CenterViewOn(
//						line, (int)(0.3 * textEditor.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount));
//				});
		}
		
		public int Line {
			get { return _textEditor.ActiveTextAreaControl.Caret.Line; }
		}
		
		public int Column {
			get { return _textEditor.ActiveTextAreaControl.Caret.Column; }
		}
		
		public void ParseInformationUpdated(ParseInformation parseInfo)
		{
			if (_textEditor.TextEditorProperties.EnableFolding) {
				WorkbenchSingleton.SafeThreadAsyncCall(ParseInformationUpdatedInvoked, parseInfo);
			}
		}
		
		public delegate string GetTextHelper();
	
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
		
		protected virtual void OnTabPageTextChanged(EventArgs e)
		{
			if (TabPageTextChanged != null) {
				TabPageTextChanged(this, e);
			}
		}
		
		protected virtual void OnTitleNameChanged(EventArgs e)
		{
			if (TitleNameChanged != null) {
				TitleNameChanged(this, e);
			}
		}
		
		protected virtual void OnDisposed(EventArgs e)
		{
			if (Disposed != null) {
				Disposed(this, e);
			}
		}
		
		protected virtual void OnIsDirtyChanged(EventArgs e)
		{
			if (IsDirtyChanged != null) {
				IsDirtyChanged(this, e);
			}
		}

        private void ParseInformationUpdatedInvoked(ParseInformation parseInfo)
		{
			try {
				_textEditor.Document.FoldingManager.UpdateFoldings(_file.FileName, parseInfo);
				_textEditor.ActiveTextAreaControl.TextArea.Refresh(_textEditor.ActiveTextAreaControl.TextArea.FoldMargin);
				_textEditor.ActiveTextAreaControl.TextArea.Refresh(_textEditor.ActiveTextAreaControl.TextArea.IconBarMargin);
			} catch (Exception ex) {
				MessageService.ShowError(ex);
			}
		}

        private string GetText()
		{
			return _textEditor.Document.TextContent;
		}

        private void SetText(string value)
		{
			_textEditor.Document.Replace(0, _textEditor.Document.TextLength, value);
		}		
	}
}
