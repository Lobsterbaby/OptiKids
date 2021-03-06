﻿using System.Windows;
using System.Windows.Input;
using JuliusSweetland.OptiKids.Models;
using JuliusSweetland.OptiKids.Services;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace JuliusSweetland.OptiKids.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAudioService audioService;
        private readonly IInputService inputService;
        private readonly InteractionRequest<NotificationWithAudioService> managementWindowRequest;
        private readonly ICommand managementWindowRequestCommand;
        private readonly ICommand quitCommand;

        public MainWindow(IAudioService audioService,
            IInputService inputService)
        {
            InitializeComponent();

            this.audioService = audioService;
            this.inputService = inputService;

            managementWindowRequest = new InteractionRequest<NotificationWithAudioService>();
            managementWindowRequestCommand = new DelegateCommand(RequestManagementWindow);
            quitCommand = new DelegateCommand(Quit);

            //Setup key binding (Alt-M and Shift-Alt-M) to open settings
            InputBindings.Add(new KeyBinding
            {
                Command = managementWindowRequestCommand,
                Modifiers = ModifierKeys.Alt,
                Key = Key.M
            });
            InputBindings.Add(new KeyBinding
            {
                Command = managementWindowRequestCommand,
                Modifiers = ModifierKeys.Shift | ModifierKeys.Alt,
                Key = Key.M
            });
        }

        public InteractionRequest<NotificationWithAudioService> ManagementWindowRequest { get { return managementWindowRequest; } }
        public ICommand ManagementWindowRequestCommand { get { return managementWindowRequestCommand; } }
        public ICommand QuitCommand { get { return quitCommand; } }

        private void RequestManagementWindow()
        {
            inputService.RequestSuspend();
            ManagementWindowRequest.Raise(
                new NotificationWithAudioService
                {
                    AudioService = audioService,
                },
                _ => inputService.RequestResume());
        }

        private void Quit()
        {
            if (MessageBox.Show(Properties.Resources.QUIT_MESSAGE, Properties.Resources.QUIT, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
