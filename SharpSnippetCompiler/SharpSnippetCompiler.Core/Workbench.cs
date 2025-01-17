using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpSnippetCompiler.Core.Properties;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public sealed class Workbench : IWorkbench
	{
        private static readonly string ViewContentPath = "/SharpDevelop/Workbench/Pads";

        private readonly List<PadDescriptor> _padDescriptors = new List<PadDescriptor>();
        private readonly List<IViewContent> _views = new List<IViewContent>();

        public event EventHandler ActiveWorkbenchWindowChanged;
		public event EventHandler ActiveViewContentChanged;		
		public event EventHandler ActiveContentChanged;		
		public event ViewContentEventHandler ViewOpened;		
		public event ViewContentEventHandler ViewClosed;
		public event KeyEventHandler ProcessCommandKey;

		public Workbench(Form mainForm)
		{
			MainForm = mainForm;
		}
		
		public Form MainForm { get; }

        public string Title {
			get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
		
		public ICollection<IViewContent> ViewContentCollection => _views;

        public ICollection<IViewContent> PrimaryViewContents => _views.AsReadOnly();

        public IList<IWorkbenchWindow> WorkbenchWindowCollection => throw new NotImplementedException();

        public IList<PadDescriptor> PadContentCollection => _padDescriptors;

        public IWorkbenchWindow ActiveWorkbenchWindow {
			get {
				if (ActiveViewContent != null) {
					return ActiveViewContent.WorkbenchWindow;
				}
				return null;
			}
		}
		
		public IViewContent ActiveViewContent { get; set; }

        public object ActiveContent { get; set; }

        public IWorkbenchLayout WorkbenchLayout { get; set; }

        public bool IsActiveWindow => throw new NotImplementedException();

        public void ShowView(IViewContent content)
		{
			_views.Add(content);
			OnViewOpened(new ViewContentEventArgs(content));
		}
		
		public void ShowPad(PadDescriptor content)
		{
			throw new NotImplementedException();
		}
		
		public void ShowView(IViewContent content, bool switchToOpenedView)
		{
			throw new NotImplementedException();
		}		
		
		public void UnloadPad(PadDescriptor content)
		{
			throw new NotImplementedException();
		}
		
		public PadDescriptor GetPad(Type type)
		{
			foreach (var pad in _padDescriptors) {
				if (pad.Class == type.FullName) {
					return pad;
				}
			}
			return null;			
		}
		
		public void CloseContent(IViewContent content)
		{
			if (_views.Contains(content)) {
				_views.Remove(content);
			}
			
			content.Dispose();
		}
		
		public void CloseAllViews()
		{
			throw new NotImplementedException();
		}
		
		public void RedrawAllComponents()
		{
			throw new NotImplementedException();
		}
		
		public ICSharpCode.Core.Properties CreateMemento()
		{
			throw new NotImplementedException();
		}
		
		public void SetMemento(ICSharpCode.Core.Properties memento)
		{
			Console.WriteLine(Resources.SetMemento_not_implemented);
		}
		
		public void UpdateRenderer()
		{
			Console.WriteLine(Resources.UpdateRenderer_not_implemented);
		}
		
		public void Initialize()
		{
			try {
				var contents = AddInTree.GetTreeNode(ViewContentPath).BuildChildItems(this);
				foreach (PadDescriptor content in contents) {
					if (content != null) {
						_padDescriptors.Add(content);
					}
				}
			} catch (TreePathNotFoundException) {}			
		}

        private void OnActiveWorkbenchWindowChanged(EventArgs e)
		{
			if (ActiveWorkbenchWindowChanged != null) {
				ActiveWorkbenchWindowChanged(this, e);
			}
		}

        private void OnActiveViewContentChanged(EventArgs e)
		{
			if (ActiveViewContentChanged != null) {
				ActiveViewContentChanged(this, e);
			}
		}

        private void OnActiveContentChanged(EventArgs e)
		{
			if (ActiveContentChanged != null) {
				ActiveContentChanged(this, e);
			}
		}

        private void OnViewOpened(ViewContentEventArgs e)
		{
			if (ViewOpened != null) {
				ViewOpened(this, e);
			}
		}

        private void OnViewClosed(ViewContentEventArgs e)
		{
			if (ViewClosed != null) {
				ViewClosed(this, e);
			}
		}

        private void OnProcessCommandKey(KeyEventArgs e)
		{
			if (ProcessCommandKey != null) {
				ProcessCommandKey(this, e);
			}
		}		
	}
}
