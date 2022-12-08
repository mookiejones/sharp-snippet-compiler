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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.SharpSnippetCompiler
{
    public sealed class WorkbenchWindow : IWorkbenchWindow
    {
        private readonly TabControl _tabControl;
        private readonly SnippetTabPage _tabPage;

        public WorkbenchWindow(TabControl tabControl, SnippetTabPage tabPage)
        {
            _tabControl = tabControl;
            _tabPage = tabPage;
        }

        public event EventHandler ActiveViewContentChanged;
        public event EventHandler WindowSelected;
        public event EventHandler WindowDeselected;
        public event EventHandler TitleChanged;
        public event EventHandler CloseEvent;

        public string Title => throw new NotImplementedException();

        public bool IsDisposed => throw new NotImplementedException();

        public IViewContent ActiveViewContent { get; set; }

        public Icon Icon
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IList<IViewContent> ViewContents => throw new NotImplementedException();

        public void SwitchView(int viewNumber)
        {
            throw new NotImplementedException();
        }

        public bool CloseWindow(bool force)
        {
            throw new NotImplementedException();
        }

        public void SelectWindow()
        {
            _tabControl.SelectedTab = _tabPage;
            OnWindowSelected(EventArgs.Empty);
        }

        public void RedrawContent()
        {
            throw new NotImplementedException();
        }

        public void OnWindowSelected(EventArgs e)
        {
            if (WindowSelected != null)
            {
                WindowSelected(this, e);
            }
        }

        public void OnWindowDeselected(EventArgs e)
        {
            if (WindowDeselected != null)
            {
                WindowDeselected(this, e);
            }
        }

        private void OnActiveViewContentChanged(EventArgs e)
        {
            if (ActiveViewContentChanged != null)
            {
                ActiveViewContentChanged(this, e);
            }
        }

        private void OnTitleChanged(EventArgs e)
        {
            if (TitleChanged != null)
            {
                TitleChanged(this, e);
            }
        }

        private void OnCloseEvent(EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent(this, e);
            }
        }
    }
}