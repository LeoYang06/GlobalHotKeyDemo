﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interop;

namespace GlobalHotKeyDemo
{
    /// <summary>
    /// 主窗体交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();

        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
        }

        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }

        /// <summary>
        /// 打开快捷键设置窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenHotkeySetting_Click(object sender, RoutedEventArgs e)
        {
            var win = HotKeySettingsWindow.CreateInstance();
            if (!win.IsVisible)
            {
                win.ShowDialog();
            }
            else
            {
                win.Activate();
            }
        }

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;
            MessageBoxResult mbResult = MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}是否要改变这些快捷键？", failList), "提示", MessageBoxButton.YesNo);
            // 弹出热键设置窗体
            var win = HotKeySettingsWindow.CreateInstance();
            if (mbResult == MessageBoxResult.Yes)
            {
                if (!win.IsVisible)
                {
                    win.ShowDialog();
                }
                else
                {
                    win.Activate();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            var hotkeySetting = new EHotKeySetting();
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    if (sid == m_HotKeySettings[EHotKeySetting.全屏])
                    {
                        hotkeySetting = EHotKeySetting.全屏;
                        //TODO 执行全屏操作
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.截图])
                    {
                        hotkeySetting = EHotKeySetting.截图;
                        //TODO 执行截图操作
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.播放])
                    {
                        hotkeySetting = EHotKeySetting.播放;
                        //TODO ......
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.前进])
                    {
                        hotkeySetting = EHotKeySetting.前进;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.后退])
                    {
                        hotkeySetting = EHotKeySetting.后退;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.保存])
                    {
                        hotkeySetting = EHotKeySetting.保存;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.打开])
                    {
                        hotkeySetting = EHotKeySetting.打开;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.新建])
                    {
                        hotkeySetting = EHotKeySetting.新建;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.删除])
                    {
                        hotkeySetting = EHotKeySetting.删除;
                    }
                    MessageBox.Show(string.Format("触发【{0}】快捷键", hotkeySetting));
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

    }
}
