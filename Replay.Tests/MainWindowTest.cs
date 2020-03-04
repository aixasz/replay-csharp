﻿using Replay.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xunit;

namespace Replay.Tests
{
    public class MainWindowTest
    {
        [WpfFact]
        public void MainWindow_Initialization_WithoutError()
        {
            var window = new MainWindow();
            var vm = window.DataContext as WindowViewModel;
            Assert.NotNull(vm);

            for(var i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                if(vm.Foreground != null
                    && vm.Background != null)
                {
                    // pass
                    return;
                }
            }
            Assert.True(false); // failure, we could not initialize
        }
    }
}
