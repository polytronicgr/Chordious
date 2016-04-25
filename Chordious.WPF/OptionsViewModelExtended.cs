﻿// 
// OptionsViewModelExtended.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class OptionsViewModelExtended : OptionsViewModel, IIdle
    {
        #region Rendering

        public int SelectedRenderBackgroundIndex
        {
            get
            {
                return (int)RenderBackground;
            }
            set
            {
                RenderBackground = (Background)(value);
            }
        }

        public ObservableCollection<string> RenderBackgrounds
        {
            get
            {
                return ObservableEnums.GetBackgrounds();
            }
        }

        private Background RenderBackground
        {
            get
            {
                Background result;

                if (Enum.TryParse<Background>(GetSetting("app.renderbackground"), out result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                SetSetting("app.renderbackground", value);
                RaisePropertyChanged("SelectedRenderBackgroundIndex");
            }
        }

        public int SelectedEditorRenderBackgroundIndex
        {
            get
            {
                return (int)EditorRenderBackground;
            }
            set
            {
                EditorRenderBackground = (Background)(value);
            }
        }

        public ObservableCollection<string> EditorRenderBackgrounds
        {
            get
            {
                return ObservableEnums.GetBackgrounds();
            }
        }

        private Background EditorRenderBackground
        {
            get
            {
                Background result;

                if (Enum.TryParse<Background>(GetSetting("diagrameditor.renderbackground"), out result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                SetSetting("diagrameditor.renderbackground", value);
                RaisePropertyChanged("SelectedEditorRenderBackgroundIndex");
            }
        }

        #endregion

        #region Updates

        public int SelectedReleaseChannelIndex
        {
            get
            {
                return (int)ReleaseChannel;
            }
            set
            {
                ReleaseChannel = (ReleaseChannel)(value);
            }
        }

        public ObservableCollection<string> ReleaseChannels
        {
            get
            {
                return GetReleaseChannels();
            }
        }

        private ReleaseChannel ReleaseChannel
        {
            get
            {
                ReleaseChannel result;

                if (Enum.TryParse<ReleaseChannel>(GetSetting("app.releasechannel"), out result))
                {
                    return result;
                }

                return ReleaseChannel.Official;
            }
            set
            {
                SetSetting("app.releasechannel", value);
                RaisePropertyChanged("SelectedReleaseChannelIndex");
            }
        }

        public bool CheckUpdateOnStart
        {
            get
            {
                bool result;

                if (Boolean.TryParse(GetSetting("app.checkupdateonstart"), out result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                SetSetting("app.checkupdateonstart", value);
                RaisePropertyChanged("CheckUpdateOnStart");
            }
        }

        public string LastUpdateCheck
        {
            get
            {
                DateTime lastCheck = UpdateUtils.GetLastUpdateCheck();

                if (lastCheck == DateTime.MinValue)
                {
                    return "Never";
                }

                return lastCheck.ToLocalTime().ToString("G");
            }
        }

        public RelayCommand CheckForUpdatesAsync
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        IsIdle = false;
                        await UpdateUtils.UpdateCheckAsync(true, true);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        IsIdle = true;
                        RaisePropertyChanged("LastUpdateCheck");
                    }
                }, () =>
                {
                    return !UpdateUtils.IsCheckingforUpdate;
                });
            }
        }

        #endregion

        public OptionsViewModelExtended() : base()
        {
            IsIdle = true;
        }

        private static ObservableCollection<string> GetReleaseChannels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Official");
            collection.Add("Preview");

            return collection;
        }

        public override void RefreshProperties()
        {
            base.RefreshProperties();
            RaisePropertyChanged("SelectedRenderBackgroundIndex");
            RaisePropertyChanged("SelectedEditorRenderBackgroundIndex");
            RaisePropertyChanged("SelectedReleaseChannelIndex");
            RaisePropertyChanged("CheckUpdateOnStart");
            RaisePropertyChanged("LastUpdateCheck");
        }
    }
}
