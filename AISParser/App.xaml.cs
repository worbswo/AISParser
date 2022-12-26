using AISParser.View;
using AISParser.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace AISParser
{
	/// <summary>
	/// App.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class App : Application
	{
		//최초 시작 View
		private MainWindow _main;
		//최초 시작 ViewModel
		private MainWindowViewModel _mainVM;
		/// <summary>
		/// 중복 실행 방지용
		/// </summary>
		private Mutex mutex;
		public App()
		{
			// 리소스 포함
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
			// 전역 예외 처리
			this.Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(DispatcherUnhandledException);
			this.Dispatcher.UnhandledExceptionFilter += new DispatcherUnhandledExceptionFilterEventHandler(DispatcherUnhandledExceptionFilter);
		}

		/// <summary>
		/// 애플리케이션 도메인 어셈블리 분석시 처리하기
		/// </summary>
		/// <param name="sender">이벤트 발생자</param>
		/// <param name="e">이벤트 인자</param>
		/// <returns>어셈블리</returns>
		static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			//AssemblyFile 이름
			var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
			//Load form Embedded Resources - This Function is not called if the Assembly is in the Application Folder
			var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
			if (resources.Count() > 0)
			{
				var resourceName = resources.First();
				using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
				{
					if (stream == null)
					{ return null; }
					var block = new byte[stream.Length];
					stream.Read(block, 0, block.Length);
					return Assembly.Load(block);
				}
			}
			return null;
		}

		/// <summary>
		/// 디스패처 미처리 예외 필터 처리하기
		/// </summary>
		/// <param name="sender">이벤트 발생자</param>
		/// <param name="e">이벤트 인자</param>
		private void DispatcherUnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
		{
			// true를 설정하면 응용 프로그램이 비정상 종료되지 않으나 false를 설정하면 응용 프로그램이 비정상 종료된다.
			e.RequestCatch = true;
		}

		/// <summary>
		/// 디스패처 미처리 예외 처리하기
		/// </summary>
		/// <param name="sender">이벤트 발생자</param>
		/// <param name="e">이벤트 인자</param>
		private new void DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			string errorMsg = string.Format("아래와 같이 예상치 않은 오류가 발생하였습니다.\n\n프로그램을 종료 하시겠습니까?\n\n{0}\n", e.Exception.Message);
			errorMsg += Environment.NewLine;
			MessageBoxResult result = MessageBox.Show(errorMsg, "오류발생", MessageBoxButton.OKCancel, MessageBoxImage.Stop);
			e.Handled = true;
			// Exits the program when the user clicks Abort.
			if (result == MessageBoxResult.OK)
			{
				Application.Current.MainWindow.Close();
			}
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			//프로그램 쓰레드 이름 지정
			Application.Current.Dispatcher.Thread.Name = "Main";
			bool created = false;
			mutex = new Mutex(true, "AISParser", out created);
			//2번째 파라미터를 통해 Mutex를 생성하지 못했을 경우
			if (!created)
			{
				MessageBox.Show("프로그램이 이미 실행 중 입니다.", "AISParser", MessageBoxButton.OK, MessageBoxImage.Warning);
				Shutdown();
			}
			else
			{
				_main = new MainWindow();

				_mainVM = new MainWindowViewModel();
				_main.DataContext = _mainVM;
				//Closing 이벤트 추가
				_main.Closing += (o, eArgs) =>
				{
					if (MainWindowViewModel.Running)
					{
						eArgs.Cancel = true;
						Thread exitThread = new Thread(() =>
						{
							if (App.Current != null)
							{
								App.Current.Dispatcher.Invoke(() =>
								{
									_mainVM.Exit();
									App.Current.Shutdown();
								});
							}
						});
						exitThread.Start();
					}
				};
				_main.Show();
			}
		}

	}
}