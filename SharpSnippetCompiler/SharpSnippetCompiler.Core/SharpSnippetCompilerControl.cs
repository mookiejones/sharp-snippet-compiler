// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
// </file>

using System.Windows.Forms;
using ICSharpCode.SharpDevelop.DefaultEditor.Gui.Editor;
using ICSharpCode.TextEditor;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public partial class SharpSnippetCompilerControl : UserControl
	{
        private readonly SharpDevelopTextAreaControl _textEditor;
		
		public SharpSnippetCompilerControl()
		{
			InitializeComponent();
			
			_textEditor = new SharpDevelopTextAreaControl();
			_textEditor.Dock = DockStyle.Fill;
			Controls.Add(_textEditor);
		}
		
		public TextEditorControl TextEditor => _textEditor;

        public void LoadFile(string fileName)
		{
			_textEditor.LoadFile(fileName);
		}
		
		public void Save()
		{
			_textEditor.SaveFile(_textEditor.FileName);
		}
	}
}
