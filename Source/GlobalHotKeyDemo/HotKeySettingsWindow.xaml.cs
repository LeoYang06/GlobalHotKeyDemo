using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GlobalHotKeyDemo
{
    /// <summary>
    /// 快捷键设置窗体交互逻辑
    /// </summary>
    public partial class HotKeySettingsWindow : Window
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static HotKeySettingsWindow m_Instance;

        private ObservableCollection<HotKeyModel> m_HotKeyList = new ObservableCollection<HotKeyModel>();
        /// <summary>
        /// 快捷键设置项集合
        /// </summary>
        public ObservableCollection<HotKeyModel> HotKeyList
        {
            get { return m_HotKeyList; }
            set { m_HotKeyList = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HotKeySettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 创建系统参数设置窗体实例
        /// </summary>
        /// <returns></returns>
        public static HotKeySettingsWindow CreateInstance()
        {
            return m_Instance ?? (m_Instance = new HotKeySettingsWindow());
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            InitHotKey();
        }

        /// <summary>
        /// 初始化快捷键
        /// </summary>
        private void InitHotKey()
        {
            var list = HotKeySettingsManager.Instance.LoadDefaultHotKey();
            list.ToList().ForEach(x => HotKeyList.Add(x));
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(HotKeyList))
                return;
            //TODO 保存设置
            this.Close();
        }

        /// <summary>
        /// 窗体关闭后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Closed_1(object sender, EventArgs e)
        {
            m_Instance = null;
        }

    }
}
