// SharpDevelop samples
// Copyright (c) 2008, AlphaSierraPapa
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
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpSnippetCompiler.Core.Properties;

namespace ICSharpCode.SharpSnippetCompiler
{
	public sealed class WorkbenchLayout : IWorkbenchLayout
	{
        public event EventHandler ActiveWorkbenchWindowChanged;
				
		public IWorkbenchWindow ActiveWorkbenchWindow => throw new NotImplementedException();

        public object ActiveContent => throw new NotImplementedException();

        public void Attach(IWorkbench workbench)
		{
			throw new NotImplementedException();
		}
		
		public void Detach()
		{
			throw new NotImplementedException();
		}
		
		public void ShowPad(PadDescriptor content)
		{
			Console.WriteLine(Resources.ShowPad_not_implemented);
		}
		
		public void ActivatePad(PadDescriptor content)
		{
			ActivatePad(content.Class);
		}
		
		public void ActivatePad(string fullyQualifiedTypeName)
		{
			Console.WriteLine(Resources.ActivatePad_not_implemented);
			if (fullyQualifiedTypeName.EndsWith("ErrorListPad")) {
				var mainForm = WorkbenchSingleton.MainForm as MainForm;
				mainForm?.ActivateErrorList();
			} else if (fullyQualifiedTypeName.EndsWith("CompilerMessageView")) {
				var mainForm = WorkbenchSingleton.MainForm as MainForm;
				mainForm?.ActivateOutputList();			
			}
		}
		
		public void HidePad(PadDescriptor content)
		{
			throw new NotImplementedException();
		}
		
		public void UnloadPad(PadDescriptor content)
		{
			throw new NotImplementedException();
		}
		
		public bool IsVisible(PadDescriptor padContent)
		{
			return false;
		}
		
		public void RedrawAllComponents()
		{
			throw new NotImplementedException();
		}

        public IWorkbenchWindow ShowView(IViewContent content, bool switchToOpenedView)
		{
			throw new NotImplementedException();
		}
			
		public void LoadConfiguration()
		{
			Console.WriteLine(Resources.LoadConfiguration_not_implemented);
		}
		
		public void StoreConfiguration()
		{
			Console.WriteLine(Resources.WorkbenchLayout_StoreConfiguration_WorkbenchLayout_StoreConfiguration_not_implemented);
		}

        private void OnActiveWorkbenchWindowChanged(EventArgs e)
		{
			if (ActiveWorkbenchWindowChanged != null) {
				ActiveWorkbenchWindowChanged(this, e);
			}
		}		
	}
}
