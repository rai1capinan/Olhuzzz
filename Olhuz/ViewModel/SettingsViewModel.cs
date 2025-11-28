using Microsoft.Maui.Controls;

namespace Olhuz.ViewModel
{
    public partial class SettingsPage : ContentPage
    {
        // Variáveis (Model Fake)
        private bool _isScreenReaderEnabled = true;
        private double _speechSpeed = 1.0;
        private string _voiceType = "Masculino";
        private string _currentTheme = "Claro";

        // Cores
        private Color _activeButtonColor;
        private Color _inactiveButtonColor;
        private Color _activeTextColor = Colors.White;
        private Color _inactiveTextColor;

        // === CONTROLES CRIADOS VIA C# ===
        private Switch ScreenReaderSwitch;
        private Slider VolumeSlider;
        private HorizontalStackLayout SpeedButtonsLayout;
        private HorizontalStackLayout VoiceTypeButtonsLayout;
        private HorizontalStackLayout ThemeButtonsLayout;

        public SettingsPage()
        {
            Title = "Configurações";

            LoadButtonColorsFromResources();
            BuildUI(); // ← Criamos toda tela aqui
            LoadCurrentSettings();
        }

        // ====================================================================
        // CONSTRUÇÃO DA UI SEM XAML
        // ====================================================================
        private void BuildUI()
        {
            // === Switch Reader ===
            ScreenReaderSwitch = new Switch();
            ScreenReaderSwitch.Toggled += OnScreenReaderToggled;

            // === Volume ===
            VolumeSlider = new Slider { Minimum = 0, Maximum = 1 };
            VolumeSlider.ValueChanged += OnVolumeSliderValueChanged;

            // === Botões de velocidade ===
            SpeedButtonsLayout = new HorizontalStackLayout { Spacing = 10 };
            SpeedButtonsLayout.Children.Add(BuildSpeedButton("0.5x", "0.5"));
            SpeedButtonsLayout.Children.Add(BuildSpeedButton("1.0x", "1.0"));
            SpeedButtonsLayout.Children.Add(BuildSpeedButton("1.5x", "1.5"));
            SpeedButtonsLayout.Children.Add(BuildSpeedButton("2.0x", "2.0"));

            // === Botões de tipo de voz ===
            VoiceTypeButtonsLayout = new HorizontalStackLayout { Spacing = 10 };
            VoiceTypeButtonsLayout.Children.Add(BuildVoiceButton("Masculino"));
            VoiceTypeButtonsLayout.Children.Add(BuildVoiceButton("Feminino"));

            // === Botões de tema ===
            ThemeButtonsLayout = new HorizontalStackLayout { Spacing = 10 };
            ThemeButtonsLayout.Children.Add(BuildThemeButton("Claro"));
            ThemeButtonsLayout.Children.Add(BuildThemeButton("Escuro"));

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 20,
                    Children =
                    {
                        new Label { Text = "Leitor de Tela:" },
                        ScreenReaderSwitch,

                        new Label { Text = "Velocidade da Fala:" },
                        SpeedButtonsLayout,

                        new Label { Text = "Tipo de Voz:" },
                        VoiceTypeButtonsLayout,

                        new Label { Text = "Volume:" },
                        VolumeSlider,

                        new Label { Text = "Tema:" },
                        ThemeButtonsLayout
                    }
                }
            };
        }

        // ====================================================================
        // FACTORY DE BOTÕES
        // ====================================================================
        private Button BuildSpeedButton(string text, string speed)
        {
            return new Button
            {
                Text = text,
                CommandParameter = speed
            }.Apply(btn => btn.Clicked += OnSpeechSpeedClicked);
        }

        private Button BuildVoiceButton(string voice)
        {
            return new Button
            {
                Text = voice,
                CommandParameter = voice
            }.Apply(btn => btn.Clicked += OnVoiceTypeClicked);
        }

        private Button BuildThemeButton(string theme)
        {
            return new Button
            {
                Text = theme,
                CommandParameter = theme
            }.Apply(btn => btn.Clicked += OnThemeClicked);
        }

        // ====================================================================
        // CORES
        // ====================================================================
        private void LoadButtonColorsFromResources()
        {
            _activeButtonColor = GetResourceColor("SevenBlue", Color.FromArgb("#007AFF"));
            _inactiveButtonColor = Colors.White;

            _inactiveTextColor = _activeButtonColor;
            _activeTextColor = Colors.White;
        }

        private Color GetResourceColor(string key, Color defaultColor)
        {
            if (Application.Current.Resources.TryGetValue(key, out object resourceValue)
                && resourceValue is Color color)
            {
                return color;
            }
            return defaultColor;
        }

        // ====================================================================
        // CARREGAMENTO INICIAL
        // ====================================================================
        private void LoadCurrentSettings()
        {
            ScreenReaderSwitch.IsToggled = _isScreenReaderEnabled;
            VolumeSlider.Value = 0.5;

            UpdateSpeechSpeedButtons(_speechSpeed);
            UpdateVoiceTypeButtons(_voiceType);
            UpdateThemeButtons(_currentTheme);
        }

        // ====================================================================
        // EVENTOS
        // ====================================================================
        private void OnScreenReaderToggled(object sender, ToggledEventArgs e)
        {
            _isScreenReaderEnabled = e.Value;
        }

        private void OnSpeechSpeedClicked(object sender, EventArgs e)
        {
            if (sender is Button btn &&
                double.TryParse(btn.CommandParameter.ToString(), out double speed))
            {
                _speechSpeed = speed;
                UpdateSpeechSpeedButtons(speed);
            }
        }

        private void OnVoiceTypeClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _voiceType = btn.CommandParameter.ToString();
                UpdateVoiceTypeButtons(_voiceType);
            }
        }

        private void OnVolumeSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // nada especial aqui
        }

        private void OnThemeClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _currentTheme = btn.CommandParameter.ToString();
                UpdateThemeButtons(_currentTheme);
            }
        }

        // ====================================================================
        // ATUALIZAÇÃO VISUAL
        // ====================================================================
        private void UpdateSpeechSpeedButtons(double activeSpeed)
        {
            foreach (var child in SpeedButtonsLayout.Children)
            {
                if (child is Button btn &&
                    double.TryParse(btn.CommandParameter.ToString(), out double speed))
                {
                    ApplyButtonState(btn, speed == activeSpeed);
                }
            }
        }

        private void UpdateVoiceTypeButtons(string activeVoice)
        {
            foreach (var child in VoiceTypeButtonsLayout.Children)
            {
                if (child is Button btn)
                {
                    ApplyButtonState(btn, btn.CommandParameter.ToString() == activeVoice);
                }
            }
        }

        private void UpdateThemeButtons(string activeTheme)
        {
            foreach (var child in ThemeButtonsLayout.Children)
            {
                if (child is Button btn)
                {
                    ApplyButtonState(btn, btn.CommandParameter.ToString() == activeTheme);
                }
            }
        }

        private void ApplyButtonState(Button btn, bool active)
        {
            btn.BackgroundColor = active ? _activeButtonColor : _inactiveButtonColor;
            btn.TextColor = active ? _activeTextColor : _inactiveTextColor;
        }
    }

    // Extensão útil
    public static class ObjectExtensions
    {
        public static T Apply<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}
