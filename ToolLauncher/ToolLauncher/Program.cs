using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLauncher
{
    class Program
    {
        private const string PATH_TOOL = @"C:\Users\vagrant\source\repos\WindowsFormsApp1\WindowsFormsApp1\bin\Debug\WindowsFormsApp1.exe";
        private const string TOOL_ARGUMENTS = @"www.northwindtraders.com aaa bbb --no-proxy-server=";

        private const string PATH_LOG_FILE = @"launch.log";

        private const int LOG_Count_Direction = 1;
        private const int LOG_Max_Size_Roll_Backups = 5;
        private static readonly Encoding LOG_ENCODING = Encoding.UTF8;
        private const log4net.Appender.RollingFileAppender.RollingMode LOG_Rolling_Style = log4net.Appender.RollingFileAppender.RollingMode.Size;
        private const int LOG_Max_File_Size = 3072000;
        private const string LOG_LAYOUT = @"%d [%t] %-5p %type - %m%n";

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger("Log");

        static void Main(string[] args)
        {
            InitLogger();
            LaunchTool();
            LogLaunchInfo();
        }

        private static void LogLaunchInfo()
        {
            string machine = Environment.MachineName;
            string user = Environment.UserName;
            logger.Info("Machine Name:" + machine + ", User Name:" + user);
        }

        private static void LaunchTool()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(PATH_TOOL);
            startInfo.Arguments = TOOL_ARGUMENTS;

            Process.Start(startInfo);
        }

        private static void InitLogger()
        {
            // 出力先のAppenderを作成します
            // ファイル出力で、日付やファイルサイズで新規ファイルを作成するタイプ
            var Appender = new log4net.Appender.RollingFileAppender()
            {
                // 出力するファイル名
                File = PATH_LOG_FILE,
                // ファイル追記モード （Falseだと上書き）
                AppendToFile = true,
                // ログファイル名を固定にする
                StaticLogFileName = true,
                // ログファイル名のローテーション番号の順番
                CountDirection = LOG_Count_Direction,
                // ログファイルの最大世代
                MaxSizeRollBackups = LOG_Max_Size_Roll_Backups,
                // 文字コード指定
                Encoding = LOG_ENCODING,
                // ログを新規ファイルに切替える条件
                RollingStyle = LOG_Rolling_Style,
                // 最大ファイルサイズ
                MaxFileSize = LOG_Max_File_Size,
                //ログのフォーマット
                Layout = new log4net.Layout.PatternLayout(LOG_LAYOUT),
            };
            // 記述した設定を有効にする　忘れないように！
            Appender.ActivateOptions();

            var log = (log4net.Repository.Hierarchy.Logger)logger.Logger;
            // 出力レベルの設定 デフォルトはALLなのでコメントアウト
            //log.Level = log4net.Core.Level.All;
            // 出力先を追加します
            log.AddAppender(Appender);

            // 設定を有効にする 忘れないように！
            log.Hierarchy.Configured = true;
        }
    }
}
