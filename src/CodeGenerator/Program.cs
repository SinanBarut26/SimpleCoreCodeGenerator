using System;
using System.Collections.Generic;
using System.IO;
using CodeGenerator.Models;
using Microsoft.Extensions.Configuration;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                var codeCopy = new CodeCopy();
                config.GetSection("CodeCopy").Bind(codeCopy);

                foreach (var codeProperty in codeCopy.GetType().GetProperties())
                {
                    var classType = codeProperty.GetType();
                    if (codeProperty.PropertyType == typeof(ProjectModel))
                    {
                        var projectModelValue = codeProperty.GetValue(codeCopy) as ProjectModel;
                        foreach (var projectProperty in projectModelValue?.GetType()?.GetProperties())
                        {
                            if (projectProperty.PropertyType == typeof(ProjectInfoModel))
                            {
                                var projectInfoModelValue =
                                    projectProperty.GetValue(projectModelValue) as ProjectInfoModel;
                                var folderPath = codeCopy.MainProjectFolder + "/" + projectModelValue.ProjectName +
                                                 "/" +
                                                 projectInfoModelValue.Folder;
                                ConsoleWrite(ConsoleColor.Cyan, $"I'm in '{folderPath}' folder");

                                CreateNewFile(
                                    infoModel: projectInfoModelValue,
                                    classNames: codeCopy.ClassName,
                                    folder: folderPath,
                                    sampleClassName: codeCopy.ExampleClassName,
                                    fileFormat: codeCopy.FileFormat);
                            }
                        }
                    }
                }

                ConsoleWrite(ConsoleColor.Green, "Successfully Complete");
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                ConsoleWrite(ConsoleColor.Red, "Problem!!");
                ConsoleWrite(ConsoleColor.DarkRed, e.Message);
                Environment.Exit(0);
            }
        }

        private static void CreateNewFile(ProjectInfoModel infoModel, IEnumerable<string> classNames,
            string folder, string sampleClassName, string fileFormat)
        {
            foreach (var className in classNames)
            {
                string exampleFileRead;
                using (var exampleFile = new StreamReader(folder + "/" + infoModel.ExampleFile))
                {
                    exampleFileRead = exampleFile.ReadToEnd();
                }

                var replacedDocument = exampleFileRead.Replace(sampleClassName, className);


                var fileNewName = infoModel.Prefix + className +
                                  infoModel.Suffix + "." + fileFormat;
                using (var createNewFile =
                    new StreamWriter(folder + "/" + fileNewName))
                {
                    createNewFile.WriteAsync(replacedDocument);
                }

                ConsoleWrite(ConsoleColor.Magenta, $"'{fileNewName}' is created in {folder}");
            }
        }

        private static void ConsoleWrite(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = default;
        }
    }
}