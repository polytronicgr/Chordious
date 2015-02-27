﻿// 
// AppViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
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
using System.IO;
using System.Reflection;

using GalaSoft.MvvmLight;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public delegate Stream GetConfigStream();

    public class AppViewModel : ViewModelBase
    {
        public static AppViewModel Instance { get; private set; }

        internal ConfigFile DefaultConfig { get; private set; }

        public bool DefaultConfigLoaded
        {
            get
            {
                return _defaultConfigLoaded;
            }
            private set
            {
                _defaultConfigLoaded = value;
                RaisePropertyChanged("DefaultConfigLoaded");
            }
        }
        private bool _defaultConfigLoaded = false;

        private GetConfigStream _loadDefaultConfigStream;

        internal ConfigFile UserConfig { get; private set; }

        public bool UserConfigLoaded
        {
            get
            {
                return _userConfigLoaded;
            }
            private set
            {
                _userConfigLoaded = value;
                RaisePropertyChanged("UserConfigLoaded");
            }
        }
        private bool _userConfigLoaded = false;

        private GetConfigStream _loadUserConfigStream;

        private GetConfigStream _saveUserConfigStream;

        public string UserConfigPath
        {
            get
            {
                return _userConfigPath;
            }
            private set
            {
                _userConfigPath = value;
                RaisePropertyChanged("UserConfigPath");
            }
        }
        private string _userConfigPath = "";

        public static void Init(Assembly assembly, GetConfigStream loadDefaultConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, string userConfigPath = "")
        {
            if (null != Instance)
            {
                throw new NotSupportedException();
            }

            Instance = new AppViewModel(assembly, loadDefaultConfigStream, loadUserConfigStream, saveUserConfigStream, userConfigPath);
        }

        private AppViewModel(Assembly assembly, GetConfigStream loadDefaultConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, string userConfigPath)
        {
            if (null == assembly)
            {
                throw new ArgumentNullException("assembly");
            }

            if (null == loadDefaultConfigStream)
            {
                throw new ArgumentNullException("loadDefaultConfigStream");
            }

            if (null == loadUserConfigStream)
            {
                throw new ArgumentNullException("loadUserConfigStream");
            }

            if (null == saveUserConfigStream)
            {
                throw new ArgumentNullException("saveUserConfigStream");
            }

            AppInfo.Assembly = assembly;
            _loadDefaultConfigStream = loadDefaultConfigStream;
            _loadUserConfigStream = loadUserConfigStream;
            _saveUserConfigStream = saveUserConfigStream;
            UserConfigPath = userConfigPath;

            DefaultConfig = new ConfigFile("Default");
            UserConfig = new ConfigFile(DefaultConfig, "User");
        }

        public void LoadDefaultConfig()
        {
            using (Stream inputStream = _loadDefaultConfigStream())
            {
                DefaultConfig.LoadFile(inputStream);
            }
            DefaultConfig.MarkAsReadOnly();
            DefaultConfigLoaded = true;
        }

        public void LoadUserConfig()
        {
            using (Stream inputStream = _loadUserConfigStream())
            {
                UserConfig.LoadFile(inputStream);
            }
            UserConfigLoaded = true;
        }

        public void SaveUserConfig()
        {
            using (Stream outputStream = _saveUserConfigStream())
            {
                UserConfig.SaveFile(outputStream);
            }
        }

        public void Close()
        {
            SaveUserConfig();
        }
    }
}