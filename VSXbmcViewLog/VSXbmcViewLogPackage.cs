using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using EnvDTE80;
using System.IO;
using Microsoft.VisualStudio.CommandBars;

namespace Heni.VSXbmcViewLog
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidVSXbmcViewLogPkgString)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.SolutionExists)]
    public sealed class VSXbmcViewLogPackage : Package
    {
        private const string FilePath = @"C:\Users\Heni\AppData\Roaming\XBMC\xbmc.log";
        OutputWindowPane _owP;
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public VSXbmcViewLogPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        SelectionEvents _selectionEvents;

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            // Pass the applicationObject member variable to the code example.
            // Create a new FileSystemWatcher and set its properties.
            var watcher = new LogWatcher(FilePath)
            {
                Path = Path.GetDirectoryName(FilePath),
                NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.Size),
                Filter = Path.GetFileName(FilePath),
                EnableRaisingEvents = true
            };
            //Set the directory of the file to monitor 
            //Raise events when the LastWrite or Size attribute is changed
            //Filter out events for only this file
            var dte = (DTE2)GetService(typeof(DTE));
            var events = dte.Events;
            _selectionEvents = events.SelectionEvents;
            _selectionEvents.OnChange += _textEditorEvents_OnChange;
            // Add a new pane to the Output window.
            _owP = dte.ToolWindows.OutputWindow.OutputWindowPanes.Add("Xbmc Log");
            watcher.TextChanged += watcher_TextChanged;
            base.Initialize();
        }

        void _textEditorEvents_OnChange()
        {
            var fileInfo = new FileInfo(FilePath);
            if (fileInfo.Exists)
                fileInfo.Refresh();
        }

        void watcher_TextChanged(object sender, LogWatcherEventArgs e)
        {
            _owP.OutputString(e.Contents);
        }

        #endregion


    }
}
