using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Path = System.IO.Path;

namespace GUI_for_ATS;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Settings? settings;

    public MainWindow()
    {
        InitializeComponent();

        this.Title = Assembly.GetExecutingAssembly().GetName().Name;

        settings = Settings.LoadSettings();

        ShowAllData();
    }

    static string MaskString(string input)
    {
        int visibleCount = 3;

        if (input.Length <= visibleCount)
        {
            return new string('*', input.Length);
        }

        return string.Concat(input.AsSpan(0, visibleCount), new string('*', input.Length - visibleCount));
    }

    private void ShowAllData()
    {
        if (settings != null)
        {
            TextBoxAzureClientId.Text = MaskString(Helper.Decrypt(settings.AzureClientId));
            TextBoxAzureTenantId.Text = MaskString(Helper.Decrypt(settings.AzureTenantId));
            TextBoxAzureClientSecret.Text = MaskString(Helper.Decrypt(settings.AzureClientSecret));

            TextBoxSigntoolPath.Text = settings.SigntoolPath;
            TextBoxSigntoolPath.ToolTip = settings.SigntoolPath;

            TextBoxDlibDllPath.Text = settings.AzureCodeSigningDlibDllPath;
            TextBoxDlibDllPath.ToolTip = settings.AzureCodeSigningDlibDllPath;

            TextBoxTimestampServer.Text = settings.TimeStampServer;
            TextBoxTimestampServer.ToolTip = settings.TimeStampServer;

            TextBoxMetadataJson.Text = settings.MetadataJsonPath;
            TextBoxMetadataJson.ToolTip = settings.MetadataJsonPath;

            if (File.Exists(settings.LastFilePath))
            {
                LabelFileName.Content = Path.GetFileName(settings.LastFilePath);
                LabelFileName.ToolTip = Path.GetFileName(settings.LastFilePath);

                LabelFilePath.Content = Path.GetFullPath(settings.LastFilePath);
                LabelFilePath.ToolTip = Path.GetFullPath(settings.LastFilePath);

                ImageFileIcon.Source = Helper.GetFileIcon(settings.LastFilePath);

                ImageFileIcon.Source ??= Helper.CreatePlaceholderIcon();

                ButtonRemoveFile.Visibility = Visibility.Visible;
            }
            else
            {
                LabelFileName.Content = "";
                LabelFileName.ToolTip = "";

                LabelFilePath.Content = "";
                LabelFilePath.ToolTip = "";

                ImageFileIcon.Source = null;

                ButtonRemoveFile.Visibility = Visibility.Hidden;
            }
        }
        else
        {
            TextBoxAzureClientId.Text = "";
            TextBoxAzureTenantId.Text = "";
            TextBoxAzureClientSecret.Text = "";

            TextBoxSigntoolPath.Text = "";
            TextBoxSigntoolPath.ToolTip = "";

            TextBoxDlibDllPath.Text = "";
            TextBoxDlibDllPath.ToolTip = "";

            TextBoxTimestampServer.Text = "";
            TextBoxTimestampServer.ToolTip = "";

            TextBoxMetadataJson.Text = "";
            TextBoxMetadataJson.ToolTip = "";

            LabelFileName.Content = "";
            LabelFileName.ToolTip = "";

            LabelFilePath.Content = "";
            LabelFilePath.ToolTip = "";

            ImageFileIcon.Source = null;

            ButtonRemoveFile.Visibility = Visibility.Hidden;

            settings = Settings.LoadSettings();
        }
    }

    private void ButtonEnterAzureClientId_Click(object? sender, RoutedEventArgs? e)
    {
        CustomInputBox inputBox = new("AZURE_CLIENT_ID:", "Please enter the \"Application (client) ID\"", "");
        bool? result = inputBox.ShowDialog();

        if (result == true)
        {
            if (inputBox.UserInput != null)
            {
                if (settings != null)
                {
                    if (inputBox.UserInput == "")
                    {
                        if (settings.AzureClientId != "")
                        {
                            if (MessageBox.Show("Are you sure you want to delete the AZURE_CLIENT_ID value?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                settings.AzureClientId = inputBox.UserInput;
                                Keyboard.ClearFocus();
                            }
                        }
                    }
                    else
                    {
                        settings.AzureClientId = Helper.Encrypt(inputBox.UserInput);
                    }

                    settings.Save();
                    ShowAllData();
                }
                else
                {
                    Console.WriteLine("Settings is null.");
                }
            }
        }
    }

    private void ButtonEnterAzureTenantId_Click(object? sender, RoutedEventArgs? e)
    {
        CustomInputBox inputBox = new("AZURE_TENANT_ID:", "Please enter the \"Directory (tenant) ID\"", "");
        bool? result = inputBox.ShowDialog();

        if (result == true)
        {
            if (inputBox.UserInput != null)
            {
                if (settings != null)
                {
                    if (inputBox.UserInput == "")
                    {
                        if (settings.AzureTenantId != "")
                        {
                            if (MessageBox.Show("Are you sure you want to delete the AZURE_TENANT_ID value?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                settings.AzureTenantId = inputBox.UserInput;
                                Keyboard.ClearFocus();
                            }
                        }
                    }
                    else
                    {
                        settings.AzureTenantId = Helper.Encrypt(inputBox.UserInput);
                    }

                    settings.Save();
                    ShowAllData();
                }
                else
                {
                    Console.WriteLine("Settings is null.");
                }
            }
        }
    }

    private void ButtonEnterAzureClientSecret_Click(object? sender, RoutedEventArgs? e)
    {
        CustomInputBox inputBox = new("AZURE_CLIENT_SECRET:", "Please enter the \"secret value\"", "");
        bool? result = inputBox.ShowDialog();

        if (result == true)
        {
            if (inputBox.UserInput != null)
            {
                if (settings != null)
                {
                    if (inputBox.UserInput == "")
                    {
                        if (settings.AzureClientSecret != "")
                        {
                            if (MessageBox.Show("Are you sure you want to delete the AZURE_CLIENT_SECRET value?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                settings.AzureClientSecret = inputBox.UserInput;
                                Keyboard.ClearFocus();
                            }
                        }
                    }
                    else
                    {
                        settings.AzureClientSecret = Helper.Encrypt(inputBox.UserInput);
                    }

                    settings.Save();
                    ShowAllData();
                }
                else
                {
                    Console.WriteLine("Settings is null.");
                }
            }
        }
    }

    private void ButtonEditSigntoolPath_Click(object sender, RoutedEventArgs e)
    {
        if (settings != null)
        {
            string path = settings.SigntoolPath;

            CustomInputBox inputBox = new("The path to signtool.exe file:", "Please enter the full path to signtool.exe file.", path);
            bool? result = inputBox.ShowDialog();

            if (result == true)
            {
                if (inputBox.UserInput != null)
                {
                    settings.SigntoolPath = inputBox.UserInput.Trim('"').Trim();
                    settings.Save();
                    ShowAllData();
                }
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonSelectSigntoolPath_Click(object? sender, RoutedEventArgs? e)
    {
        if (settings != null)
        {
            string? path;

            if (settings.SigntoolPath != "")
            {
                path = Path.GetDirectoryName(settings.SigntoolPath);
            }
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            OpenFileDialog openFileDialog = new()
            {
                Title = "Select signtool.exe",
                Filter = "Signtool|signtool.exe|Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = path
            };

            if (openFileDialog.ShowDialog() == true)
            {
                settings.SigntoolPath = openFileDialog.FileName;
                settings.Save();
                ShowAllData();
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonEditDlibDllPath_Click(object sender, RoutedEventArgs e)
    {
        if (settings != null)
        {
            string path = settings.AzureCodeSigningDlibDllPath;

            CustomInputBox inputBox = new("The path to Azure.CodeSigning.Dlib.dll file:", "Please enter the full path to Azure.CodeSigning.Dlib.dll file.", path);
            bool? result = inputBox.ShowDialog();

            if (result == true)
            {
                if (inputBox.UserInput != null)
                {
                    settings.AzureCodeSigningDlibDllPath = inputBox.UserInput.Trim('"').Trim();
                    settings.Save();
                    ShowAllData();
                }
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonSelectDlibDllPath_Click(object? sender, RoutedEventArgs? e)
    {
        if (settings != null)
        {
            string? path;

            if (settings.AzureCodeSigningDlibDllPath != "")
            {
                path = Path.GetDirectoryName(settings.AzureCodeSigningDlibDllPath);
            }
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            OpenFileDialog openFileDialog = new()
            {
                Title = "Please select the Azure.CodeSigning.Dlib.dll file.",
                Filter = "Azure CodeSigning Dlib|Azure.CodeSigning.Dlib.dll|Dynamic Link Library Files (*.dll)|*.dll|All Files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = path
            };

            if (openFileDialog.ShowDialog() == true)
            {
                settings.AzureCodeSigningDlibDllPath = openFileDialog.FileName;
                settings.Save();
                ShowAllData();
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonEditTimestamp_Click(object? sender, RoutedEventArgs? e)
    {
        if (settings != null)
        {
            string initialTimeStampServer = settings.TimeStampServer;

            CustomInputBox inputBox = new("Timestamp server:", "Please enter the timestamp server URL.", initialTimeStampServer);
            bool? result = inputBox.ShowDialog();

            if (result == true)
            {
                if (inputBox.UserInput != null)
                {
                    settings.TimeStampServer = inputBox.UserInput.Trim('"').Trim();
                    settings.Save();
                    ShowAllData();
                }
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonDefaultTimestamp_Click(object sender, RoutedEventArgs e)
    {
        if (settings != null)
        {
            settings.TimeStampServer = "http://timestamp.acs.microsoft.com";
            settings.Save();
            ShowAllData();
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonEditMetadataJson_Click(object sender, RoutedEventArgs e)
    {
        if (settings != null)
        {
            CustomInputBox inputBox = new("The path to metadata.json file:", "Please enter the full path to your metadata.json file.", settings.MetadataJsonPath);
            bool? result = inputBox.ShowDialog();

            if (result == true)
            {
                if (inputBox.UserInput != null)
                {
                    settings.MetadataJsonPath = inputBox.UserInput.Trim('"').Trim();
                    settings.Save();
                    ShowAllData();
                }
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonSelectMetadataJson_Click(object? sender, RoutedEventArgs? e)
    {

        if (settings != null)
        {
            string? path;

            if (settings.MetadataJsonPath != "")
            {
                path = Path.GetDirectoryName(settings.MetadataJsonPath);
            }
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            OpenFileDialog openFileDialog = new()
            {
                Title = "Please select the metadata.json file.",
                Filter = "Metadata JSON|metadata.json|JavaScript Object Notation Files (*.json)|*.json|All Files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = path
            };

            if (openFileDialog.ShowDialog() == true)
            {
                settings.MetadataJsonPath = openFileDialog.FileName;
                settings.Save();
                ShowAllData();
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonSelectFile_Click(object? sender, RoutedEventArgs? e)
    {
        if (settings != null)
        {
            string? path;

            if (settings.LastFilePath != "")
            {
                path = Path.GetDirectoryName(settings.LastFilePath);
            }
            else
            {
                if (settings.LastPath != "")
                {
                    path = settings.LastPath;
                }
                else
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
            }

            OpenFileDialog openFileDialog = new()
            {
                Title = "Please select the file to be signed.",
                Filter = "Signable Files (*.exe, *.dll, *.msi, *.sys, *.cab, *.cat, *.ocx, *.xpi, *.ps1, *.psm1, *.vsix, *.nupkg, *.appx, *.msix)|" +
                 "*.exe;*.dll;*.msi;*.sys;*.cab;*.cat;*.ocx;*.xpi;*.ps1;*.psm1;*.vsix;*.nupkg;*.appx;*.msix|" +
                 "Executable Files (*.exe, *.dll, *.ocx)|*.exe;*.dll;*.ocx|" +
                 "Installation Files (*.msi, *.cab, *.appx, *.msix)|*.msi;*.cab;*.appx;*.msix|" +
                 "PowerShell Scripts (*.ps1, *.psm1)|*.ps1;*.psm1|" +
                 "All Files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = path
            };

            if (openFileDialog.ShowDialog() == true)
            {
                settings.LastFilePath = openFileDialog.FileName;
                settings.Save();
                ShowAllData();
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonRemoveFile_Click(object sender, RoutedEventArgs e)
    {
        if (settings != null)
        {
            if (settings.LastFilePath != null)
            {
                string? tempString = Path.GetDirectoryName(settings.LastFilePath);

                if (tempString != null)
                {
                    settings.LastPath = tempString;
                }
            }

            settings.LastFilePath = "";
            settings.Save();
            ShowAllData();
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void ButtonSign_Click(object sender, RoutedEventArgs e)
    {
        ShowAllData();

        if (AreAllDataValid(true))
        {
            RunCommand();
        }
        else
        {
            // After the user has been given the opportunity to correct all data, everything will be verified again, and if any issues remain, an error message will be displayed.
            if (AreAllDataValid(false))
            {
                RunCommand();
            }
            else
            {
                MessageBox.Show("Please double-check all data and file paths to proceed with the signing process.", "Data Check Before Signing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private bool AreAllDataValid(bool runSolution)
    {
        if (DoesFileExist(runSolution) &
            IsAzureClientIdEntered(runSolution) &
            IsAzureTenantIdEntered(runSolution) &
            IsAzureClientSecretEntered(runSolution) &
            DoesSigntoolExist(runSolution) &
            DoesDlibPathExist(runSolution) &
            IsTimestampServerAvailable(runSolution) &
            DoesMetadataJsonPathExist(runSolution))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DoesFileExist(bool runSolution)
    {
        if (settings != null)
        {
            if (File.Exists(settings.LastFilePath))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    MessageBox.Show("In the next step, please select the file to be signed.", "No file was selected to sign or the file no longer exists.", MessageBoxButton.OK, MessageBoxImage.Information);
                    ButtonSelectFile_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool IsAzureClientIdEntered(bool runSolution)
    {
        if (settings != null)
        {
            if (!System.String.IsNullOrEmpty(settings.AzureClientId))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    ButtonEnterAzureClientId_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool IsAzureTenantIdEntered(bool runSolution)
    {
        if (settings != null)
        {
            if (!System.String.IsNullOrEmpty(settings.AzureTenantId))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    ButtonEnterAzureTenantId_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool IsAzureClientSecretEntered(bool runSolution)
    {
        if (settings != null)
        {
            if (!System.String.IsNullOrEmpty(settings.AzureClientSecret))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    ButtonEnterAzureClientSecret_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool DoesSigntoolExist(bool runSolution)
    {
        if (settings != null)
        {
            if (File.Exists(settings.SigntoolPath))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    string message = "Please select the signtool.exe file in the next step.";

                    if (settings.SigntoolPath == "")
                    {
                        MessageBox.Show(message, "The signtool.exe file is required for signing.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "The signtool.exe file does not exist.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    ButtonSelectSigntoolPath_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool DoesDlibPathExist(bool runSolution)
    {
        if (settings != null)
        {
            if (File.Exists(settings.AzureCodeSigningDlibDllPath))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    string message = "Please select the Azure.CodeSigning.Dlib.dll file in the next step.";

                    if (settings.AzureCodeSigningDlibDllPath == "")
                    {
                        MessageBox.Show(message, "The Azure.CodeSigning.Dlib.dll file is required for signing.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "The Azure.CodeSigning.Dlib.dll file does not exist.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    ButtonSelectDlibDllPath_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool IsTimestampServerAvailable(bool runSolution)
    {
        if (settings != null)
        {
            string url = settings.TimeStampServer;

            using HttpClient client = new();
            string caption = "The timestamp server is unavailable.";

            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (runSolution)
                    {
                        MessageBox.Show(settings.TimeStampServer, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        ButtonEditTimestamp_Click(null, null);
                    }

                    return false;
                }
            }
            catch (Exception es)
            {
                if (runSolution)
                {
                    MessageBox.Show(es.Message + "\n\n" + settings.TimeStampServer, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    ButtonEditTimestamp_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private bool DoesMetadataJsonPathExist(bool runSolution)
    {
        if (settings != null)
        {
            if (File.Exists(settings.MetadataJsonPath))
            {
                return true;
            }
            else
            {
                if (runSolution)
                {
                    string message = "Please select the metadata.json file in the next step.";

                    if (settings.MetadataJsonPath == "")
                    {
                        MessageBox.Show(message, "The metadata.json file is required for signing.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "The metadata.json file does not exist.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    ButtonSelectMetadataJson_Click(null, null);
                }

                return false;
            }
        }
        else
        {
            Console.WriteLine("Settings is null.");
            return false;
        }
    }

    private async void RunCommand()
    {
        if (settings != null)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });

                string plainAzureClientId = Helper.Decrypt(settings.AzureClientId);
                string plainAzureTenantId = Helper.Decrypt(settings.AzureTenantId);
                string plainAzureClientSecret = Helper.Decrypt(settings.AzureClientSecret);
                string envVariablen = "set \"AZURE_CLIENT_ID=" + plainAzureClientId + 
                                      "\" && set \"AZURE_TENANT_ID=" + plainAzureTenantId + 
                                      "\" && set \"AZURE_CLIENT_SECRET=" + plainAzureClientSecret + "\" && ";
                string pathSigntool = envVariablen + "\"" + TextBoxSigntoolPath.Text + "\"";
                string comandPart1 = " sign /v /debug /fd SHA256 /tr ";
                string timeStampServer = "\"" + TextBoxTimestampServer.Text + "\"";
                string commandPart2 = " /td SHA256 /dlib ";
                string dlibDllPath = "\"" + TextBoxDlibDllPath.Text + "\" ";
                string commandPart3 = " /dmdf ";
                string metadataJsonPath = "\"" + TextBoxMetadataJson.Text + "\" ";
                string fileToSignPath = "\"" + settings.LastFilePath + "\"";

                string command = pathSigntool + comandPart1 + timeStampServer + commandPart2 + dlibDllPath + commandPart3 + metadataJsonPath + fileToSignPath;

                Dispatcher.Invoke(() => OutputBox.Document.Blocks.Clear());

                ProcessStartInfo psi = new()
                {
                    FileName = "cmd.exe",
                    Arguments = "/C \"" + command + "\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                await Task.Run(() =>
                {
                    using Process process = new() { StartInfo = psi };

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput(e.Data);
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput(e.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AppendOutput(ex.Message));
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }
        else
        {
            Console.WriteLine("Settings is null.");
        }
    }

    private void AppendOutput(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        Dispatcher.Invoke(() =>
        {
            Paragraph paragraph = new()
            {
                LineHeight = 1,
            };

            if (text.Contains("Number of files successfully Signed:"))
            {
                Match match = Regex.Match(text, @"\d+$");

                if (match.Success)
                {
                    int number = int.Parse(match.Value);

                    if(number > 0)
                    {
                        paragraph.Inlines.Add(new Run(text) { Foreground = Brushes.Green});
                    }
                    else
                    {
                        paragraph.Inlines.Add(new Run(text));
                    }
                }
                else
                {
                    paragraph.Inlines.Add(new Run(text));
                }
            }
            else
            {
                if (text.Contains("Number of warnings:"))
                {
                    Match match = Regex.Match(text, @"\d+$");

                    if (match.Success)
                    {
                        int number = int.Parse(match.Value);

                        if (number > 0)
                        {
                            paragraph.Inlines.Add(new Run(text) { Foreground = Brushes.Goldenrod});
                        }
                        else
                        {
                            paragraph.Inlines.Add(new Run(text));
                        }
                    }
                    else
                    {
                        paragraph.Inlines.Add(new Run(text));
                    }
                }
                else
                {
                    if (text.Contains("Number of errors:"))
                    {
                        Match match = Regex.Match(text, @"\d+$");

                        if (match.Success)
                        {
                            int number = int.Parse(match.Value);

                            if (number > 0)
                            {
                                paragraph.Inlines.Add(new Run(text) { Foreground = Brushes.OrangeRed});
                            }
                            else
                            {
                                paragraph.Inlines.Add(new Run(text));
                            }
                        }
                        else
                        {
                            paragraph.Inlines.Add(new Run(text));
                        }
                    }
                    else
                    {
                        if (text.Contains("SignTool Error:") || text.Contains("Unhandled managed exception"))
                        {
                            paragraph.Inlines.Add(new Run(text) { Foreground = Brushes.OrangeRed });
                        }
                        else
                        {
                            paragraph.Inlines.Add(new Run(text));
                        }
                    }
                }
            }

            OutputBox.Document.Blocks.Add(paragraph);
            OutputBox.ScrollToEnd();
        });
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
}