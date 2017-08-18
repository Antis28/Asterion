#define Debug
#define framewok_4_0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Asterion.ConvertSRTtoTXT
{
    class ModelSRTtoTXT
    {
        internal bool isRunning;
        private bool isAllFiles;
        private string[] pathFileNames;
        private string pathDirectory;
        private List<string> pathToInputFiles;

        // Объявляем событие
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        /// <summary>
        /// Обрабатываются все файлы в папке
        /// </summary>
        public void SwitchOnAllFiles()
        {
            pathFileNames = null;
            isAllFiles = true;
        }

        /// <summary>
        /// Обрабатываются только выбранные файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>
        public void SwitchOnSelectedFiles( string[] pathFileNames )
        {
            this.pathFileNames = pathFileNames;
            isAllFiles = false;
        }

        public void BeginStartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SrtToTxt";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
        //------------- private -----------------------------//
        private void Start()
        {
            ExtractPathsFiles(pathDirectory); // получить адреса файлов

            StreamReader sReader = new StreamReader(pathToInputFiles[0]);

        }
        /// <summary>
        /// Получение полных путей файлов
        /// </summary>
        /// <param name="pathDirectory"></param>
        private void ExtractPathsFiles( string pathDirectory )
        {
            pathToInputFiles = new List<string>();
            if( isAllFiles )
            {
                pathFileNames = FilterExtension(pathDirectory);
            }
            foreach( var item in pathFileNames )
            {
                pathToInputFiles.Add(item);
            }
            //OnMaxValue(pathToInputFiles.Count);
        }

        public string[] FilterExtension( string pathDirectory )
        {
        #if framewok_4_0
                    //For .NET 4.0 and later, 
                    var files = Directory.EnumerateFiles(pathDirectory, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s =>
                    s.EndsWith(".SRT", StringComparison.OrdinalIgnoreCase)
                    );
        #endif
        #if framewok_do_4_0
                    //For earlier versions of .NET,
                    var files = Directory.GetFiles("C:\\path", "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".srt"));
        #endif
            return files.ToArray<string>();
        }
    }
}
