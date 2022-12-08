using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpSnippetCompiler.Core.Properties;

namespace ICSharpCode.SharpSnippetCompiler.Core
{
	public class Workbench : IWorkbench
	{
        private readonly static string ViewContentPath = "/SharpDevelop/Workbench/Pads";

        private Form _mainForm;
        private List<PadDescriptor> _padDescriptors = new List<PadDescriptor>();
        private List<IViewContent> _views = new List<IViewContent>();
        private IWorkbenchLayout _workbenchLayout;
        private IViewContent _activeViewContent;
        private object _activeContent;
		
		public event EventHandler ActiveWorkbenchWindowChanged;
		public event EventHandler ActiveViewContentChanged;		
		public event EventHandler ActiveContentChanged;		
		public event ViewContentEventHandler ViewOpened;		
		public event ViewContentEventHandler ViewClosed;
		public event KeyEventHandler ProcessCommandKey;

		public Workbench(Form mainForm)
		{
			this._mainForm = mainForm;
		}
		
		public Form MainForm {
			get { return _mainForm; }
		}
		
		public string Title {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public ICollection<IViewContent> ViewContentCollection {
			get { return _views; }
		}
		
		public ICollection<IViewContent> PrimaryViewContents {
			get { return _views.AsReadOnly(); }
		}
		
		public IList<IWorkbenchWindow> WorkbenchWindowCollection {
			get {
				throw new NotImplementedException();
			}
		}
		
		public IList<PadDescriptor> PadContentCollection {
			get { return _padDescriptors; }
		}
		
		public IWorkbenchWindow ActiveWorkbenchWindow {
			get {
				if (_activeViewContent != null) {
					return _activeViewContent.WorkbenchWindow;
				}
				return null;
			}
		}
		
		public IViewContent ActiveViewContent {
			get { return _activeViewContent; }
			set { _activeViewContent = value; }
		}
		
		public object ActiveContent {
			get { return _activeContent; }
			set { _activeContent = value; }
		}
		
		public IWorkbenchLayout WorkbenchLayout {
			get { return _workbenchLayout; }
			set { _workbenchLayout = value; }
		}
		
		public bool IsActiveWindow {
			get {
				throw new NotImplementedException();
			}
		}
		
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
		
		protected virtual void OnActiveWorkbenchWindowChanged(EventArgs e)
		{
			if (ActiveWorkbenchWindowChanged != null) {
				ActiveWorkbenchWindowChanged(this, e);
			}
		}

		protected virtual void OnActiveViewContentChanged(EventArgs e)
		{
			if (ActiveViewContentChanged != null) {
				ActiveViewContentChanged(this, e);
			}
		}

		protected virtual void OnActiveContentChanged(EventArgs e)
		{
			if (ActiveContentChanged != null) {
				ActiveContentChanged(this, e);
			}
		}

		protected virtual void OnViewOpened(ViewContentEventArgs e)
		{
			if (ViewOpened != null) {
				ViewOpened(this, e);
			}
		}

		protected virtual void OnViewClosed(ViewContentEventArgs e)
		{
			if (ViewClosed != null) {
				ViewClosed(this, e);
			}
		}

		protected virtual void OnProcessCommandKey(KeyEventArgs e)
		{
			if (ProcessCommandKey != null) {
				ProcessCommandKey(this, e);
			}
		}		
	}
}
