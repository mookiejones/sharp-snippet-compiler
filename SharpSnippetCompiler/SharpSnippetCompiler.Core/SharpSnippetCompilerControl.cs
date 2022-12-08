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
		SharpDevelopTextAreaControl textEditor;
		
		public SharpSnippetCompilerControl()
		{
			InitializeComponent();
			
			textEditor = new SharpDevelopTextAreaControl();
			textEditor.Dock = DockStyle.Fill;
			this.Controls.Add(textEditor);
		}
		
		public TextEditorControl TextEditor {
			get { return textEditor; }
		}
				
		public void LoadFile(string fileName)
		{
			textEditor.LoadFile(fileName);
		}
		
		public void Save()
		{
			textEditor.SaveFile(textEditor.FileName);
		}
	}
}
