using CliWrap;
using CliWrap.Buffered;
using System.IO;
using CliWrap.Exceptions;
using System.Windows.Controls;
using System.Windows.Media;

namespace EzCollege.Helpers
{
    public class DockerHelper
    {
        private static readonly string _containerName = "ai_service";

        public static async Task<string> RunDockerInstallScript()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string scriptPath = Path.Combine(basePath, "../../../Scripts", "GetDocker.ps1");
            BufferedCommandResult scriptResult;

            try
            {
                scriptResult = await Cli.Wrap("powershell")
                    .WithArguments([scriptPath])
                    .ExecuteBufferedAsync();

                return scriptResult.StandardOutput.Trim();
            }
            catch (CommandExecutionException ex)
            {
                if (ex.Message.Contains("scripts foi \r\ndesabilitada neste sistema."))
                    return "Execução de scripts desativada no seu sistema.";

                return "Um erro ocorreu ao tentar instalar o Docker.";
            }
        }

        public static async Task<string> StartDockerContainer(TextBlock textBlock)
        {
            string[] runArguments = [
                    "docker",
                    "run",
                    "-d",
                    $"--name={ _containerName }",
                    "-p 8080:8080",
                    "-p 1337:1337",
                    "-p 7900:7900",
                    "--shm-size=2g",
                    "-v ${PWD}/har_and_cookies:/app/har_and_cookie",
                    "-v ${PWD}/generated_images:/app/generated_images",
                    "hlohaus789/g4f:latest"
            ];
            BufferedCommandResult commandResult;

            try
            {
                commandResult = await Cli.Wrap("powershell")
                    .WithArguments(runArguments)
                    .ExecuteBufferedAsync();
                Helper.ChangeTextColor(textBlock, Brushes.Green);
                return "Container criado com sucesso!";
            }
            catch (CommandExecutionException ex)
            {
                if (ex.Message.Contains("already in use"))
                {
                    try
                    {
                        string[] startArguments = [
                        "docker",
                        "start",
                        _containerName
                        ];
                        commandResult = await Cli.Wrap("powershell")
                            .WithArguments(startArguments)
                            .ExecuteBufferedAsync();
                        Helper.ChangeTextColor(textBlock, Brushes.Green);
                        return "Container iniciado com sucesso!";
                    }
                    catch
                    {
                        return "Um erro ocorreu ao tentar iniciar o container.";
                    }
                }
                else return "Um erro ocorreu ao tentar criar/iniciar o container.";
            }
        }

        public static async Task<string> StopDockerContainer(TextBlock textBlock)
        {
            string[] arguments = [
                    "docker",
                    "stop",
                    _containerName,
            ];
            BufferedCommandResult commandResult;

            try
            {
                commandResult = await Cli.Wrap("powershell")
                    .WithArguments(arguments)
                    .ExecuteBufferedAsync();
                Helper.ChangeTextColor(textBlock, Brushes.Red);
                return "Container parado com sucesso!";
            }
            catch
            {
                return "Um erro ocorreu ao tentar finalizar o container.";
            }
        }

        public static async Task<bool> IsDockerContainerRunning()
        {
            string[] arguments = [
                    "docker",
                    "ps",
                    "-f",
                    $"name={ _containerName }"
            ];

            try
            {
                BufferedCommandResult commandResult = await
                    Cli.Wrap("powershell")
                    .WithArguments(arguments)
                    .ExecuteBufferedAsync();
                return commandResult.StandardOutput.Trim().Contains(_containerName);
            }
            catch
            {
                return false;
            }
        }
    }
}
