﻿// 
// ChordQualityEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordQualityEditorViewModel : NamedIntervalEditorViewModel
    {
        public override string Title
        {
            get
            {
                return IsNew ? Strings.ChordQualityEditorNewTitle : (ReadOnly ? Strings.ChordQualityEditorEditReadOnlyTitle : Strings.ChordQualityEditorEditTitle);
            }
        }

        public override string NameToolTip
        {
            get
            {
                return Strings.ChordQualityEditorNameToolTip;
            }
        }

        public override string IntervalsToolTip
        {
            get
            {
                return Strings.ChordQualityEditorIntervalsToolTip;
            }
        }

        public override string ExampleToolTip
        {
            get
            {
                return Strings.ChordQualityEditorExampleToolTip;
            }
        }

        public string AbbreviationLabel
        {
            get
            {
                return Strings.ChordQualityEditorAbbreviationLabel;
            }
        }

        public string AbbreviationToolTip
        {
            get
            {
                return Strings.ChordQualityEditorAbbreviationToolTip;
            }
        }

        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                _abbreviation = value;
                RaisePropertyChanged("Abbreviation");
                Accept.RaiseCanExecuteChanged();
            }
        }
        private string _abbreviation;

        private Action<string, string, int[]> _callback;

        public ChordQualityEditorViewModel(Action<string, string, int[]> callback) : base()
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            _callback = callback;
        }

        public ChordQualityEditorViewModel(string name, string abbreviation, int[] intervals, bool readOnly, Action<string, string, int[]> callback) : base(name, intervals, readOnly)
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            _abbreviation = abbreviation;
            _callback = callback;
        }

        protected override void OnAccept()
        {
            _callback(Name, Abbreviation, GetIntervalArray());
        }
    }
}
