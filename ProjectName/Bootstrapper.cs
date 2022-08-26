//        佛祖保佑       永无BUG


//                            _ooOoo_
//                           o8888888o
//                           88" . "88
//                           (| -_- |)
//                            O\ = /O
//                        ____/`---'\____
//                      .   ' \\| |// `.
//                       / \\||| : |||// \
//                     / _||||| -:- |||||- \
//                       | | \\\ - /// | |
//                     | \_| ''\---/'' | |
//                      \ .-\__ `-` ___/-. /
//                   ___`. .' /--.--\ `. . __
//                ."" '< `.___\_<|>_/___.' >'"".
//               | | : `- \`.;`\ _ /`;.`/ - ` : | |
//                 \ \ `-. \_ __\ /__ _/ .-` / /
//         ======`-.____`-.___\_____/___.-`____.-'======
//                            `=---='
//
//         .............................................
//                  佛祖镇楼                  BUG辟易
//          佛曰:
//                  写字楼里写字间，写字间里程序员；
//                  程序人员写程序，又拿程序换酒钱。
//                  酒醒只在网上坐，酒醉还来网下眠；
//                  酒醉酒醒日复日，网上网下年复年。
//                  但愿老死电脑间，不愿鞠躬老板前；
//                  奔驰宝马贵者趣，公交自行程序员。
//                  别人笑我忒疯癫，我笑自己命太贱；
//                  不见满街漂亮妹，哪个归得程序员？
using Caliburn.Micro;
using ControlzEx.Theming;
using ProjectName.Core;
using ProjectName.i18N;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Seasail.i18N;

namespace ProjectName
{
    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;
        private ITranslater _translater;
        private Options _options;
        //初始化
        public Bootstrapper()
        {
            Initialize();
            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        //重写Configure
        protected override void Configure()
        {
            var aggregateCatalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x))
     .OfType<ComposablePartCatalog>());

            _container = new CompositionContainer(aggregateCatalog);
            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);
            //初始化配置文件
            InitializeConfig();
            batch.AddExportedValue(_options);
            //batch.AddExportedValue<AppSettings>(_config);
            //batch.AddExportedValue<DeviceService>(new DeviceService());
            //batch.AddExportedValue<Seasail.Core.Control.Views.Dialog.MessageBoxView>
            //初始化语言
            InitializeCulture();
            batch.AddExportedValue(_translater);
            _container.Compose(batch);
        }

        private void InitialzeDataAccess(CompositionBatch batch)
        {

        }

        protected override object GetInstance(Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = _container.GetExportedValues<object>(contract);
            Seasail.Data.Check.NotNull(exports, $"Could not locate any instances of contract {contract}.");
            return exports.First();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetExportedValues<object>(
                AttributedModelServices.GetContractName(service));
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //加载启动动画
            LoadSplashScreen();

            // 自定义视图、视图模型查找
            ViewLocator.LocateTypeForModelType = LocateTypeForModelType;

            // 初始化自定义的值替换
            InitSpecialValues();

            // 解决控件时间显示不是本地格式的问题
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // 初始化显示主题
            InitializeTheme();

            //设置显示主界面
            DisplayRootViewFor<IShell>();
            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }



        /// <summary>
        /// 加载开机界面
        /// </summary>
        private void LoadSplashScreen()
        {
            ////在资源文件中定义了SplashScreen，不再需要手动启动开机动画
            //string splashScreenPngPath = "Resources/SplashScreen.png";
            //SplashScreen s = new SplashScreen(splashScreenPngPath);
            //s.Show(true,true);
            //s.Close(TimeSpan.FromSeconds(3));
        }
        private void InitializeTheme()
        {
            Theme theme = ThemeManager.Current.Themes.FirstOrDefault(p => p.Name == _options.Theme);
            if (theme != null && theme != ThemeManager.Current.DetectTheme())
                ThemeManager.Current.ChangeTheme(Application.Current, theme.Name);
        }

        /// <summary>
        /// 在这里添加我自已的Caliburn.Micro绑定变量
        /// </summary>
        private void InitSpecialValues()
        {
            //MessageBinder.SpecialValues.Add("$clickedItem",
            //    c => (c.EventArgs as ItemClickEventArgs)?.ClickedItem);
        }

        // 定位视图类型，支持派生类继承父视图
        private static Type LocateTypeForModelType(Type modelType, DependencyObject displayLocation, object context)
        {
            var viewTypeName = modelType.FullName;

            if (Caliburn.Micro.View.InDesignMode)
            {
                viewTypeName = ViewLocator.ModifyModelTypeAtDesignTime(viewTypeName);
            }

            viewTypeName = viewTypeName.Substring(0, viewTypeName.IndexOf('`') < 0
                ? viewTypeName.Length
                : viewTypeName.IndexOf('`'));

            var viewTypeList = ViewLocator.TransformName(viewTypeName, context);
            var viewType = AssemblySource.FindTypeByNames(viewTypeList);
            if (viewType == null)
            {
                Trace.TraceWarning("View not found. Searched: {0}.", string.Join(", ", viewTypeList.ToArray()));

                if (modelType.BaseType != null)
                {
                    return ViewLocator.LocateTypeForModelType(modelType.BaseType, displayLocation, context);
                }
            }

            return viewType;
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }
        private void InitializeCulture()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            if (!string.IsNullOrEmpty(_options.Culture))
            {
                ci = CultureInfo.GetCultureInfo(_options.Culture);
            }
            Utils.LocalUtil.SwitchCulture(ci);
            _translater = new Translater();
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void InitializeConfig()
        {
            _options = Options.Load();
            if (_options == null)
                _options = new Options();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _options.Save();
            base.OnExit(sender, e);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Seasail.Logging.LogManager.Error(e.Exception);
            Application.Current.Shutdown(-1);
        }
    }
}
