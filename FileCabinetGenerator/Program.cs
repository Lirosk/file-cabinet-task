using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Services;
using Models;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Generates records and saves them to file.
    /// </summary>
    public static class Program
    {
        private static int outputTypeIndex = -1;
        private static string outputFile = string.Empty;
        private static int recordsAmount = -1;
        private static int startId = -1;
        private static Random random = new ();

        private static Tuple<string[], Action<string>>[] args = new Tuple<string[], Action<string>>[]
        {
            new (new[] { "-t", "--output-type" }, SetOutputType),
            new (new[] { "-o", "--output" }, SetOutputFile),
            new (new[] { "-a", "--records-amount" }, SetRecordsAmount),
            new (new[] { "-i", "--start-id" }, SetStartId),
        };

        private static Tuple<string, Action<StreamWriter, List<FileCabinetRecord>>>[] outputTypes = new Tuple<string, Action<StreamWriter, List<FileCabinetRecord>>>[]
        {
            new ("csv", ExportRecordsToCsv),
            new ("xml", ExportRecordsToXml),
        };

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="consoleArgs">Arguments passed via console.</param>
        public static void Main(string[] consoleArgs)
        {
            try
            {
                ProceedArgs(consoleArgs);

                if (outputTypeIndex == -1)
                {
                    throw new ArgumentException("Define output type.");
                }

                if (outputFile.Equals(string.Empty, StringComparison.Ordinal))
                {
                    throw new ArgumentException("Define output file.");
                }

                if (recordsAmount == -1)
                {
                    throw new ArgumentException("Define records amount.");
                }

                if (startId == -1)
                {
                    throw new ArgumentException("Define start id.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error: Invalid args input.{Environment.NewLine}");
                return;
            }

            ExportRecords(GenerateRecords());
        }

        private static List<FileCabinetRecord> GenerateRecords()
        {
            var sb = new StringBuilder();
            char symb;
            int firstNameLen;
            int lastNameLen;
            string firstName;
            string lastName;
            int year, month, day;
            DateTime dateOfBirth;
            short schoolGrade;
            decimal averageMark;
            char classLetter;
            PersonalData personalData;
            List<FileCabinetRecord> records = new ();

            for (int id = (int)startId!; id < startId + recordsAmount; id++)
            {
                firstNameLen = random.Next(2, 31);
                for (int i = 0; i < firstNameLen; i++)
                {
                    if (random.NextSingle() < .5)
                    {
                        symb = (char)random.Next((int)'a', (int)'z');
                    }
                    else
                    {
                        symb = (char)random.Next((int)'A', (int)'Z');
                    }

                    sb.Append(symb);
                }

                firstName = sb.ToString();
                sb.Clear();

                lastNameLen = random.Next(2, 31);
                for (int i = 0; i < lastNameLen; i++)
                {
                    if (random.NextSingle() < .5)
                    {
                        symb = (char)random.Next((int)'a', (int)'z');
                    }
                    else
                    {
                        symb = (char)random.Next((int)'A', (int)'Z');
                    }

                    sb.Append(symb);
                }

                lastName = sb.ToString();
                sb.Clear();

                year = random.Next(1950, 2022);
                month = random.Next(1, 13);
                day = random.Next(1, 29);

                dateOfBirth = new (year, month, day);

                schoolGrade = (short)random.Next(1, 12);
                averageMark = (decimal)random.NextDouble() * 10;
                classLetter = (char)random.Next((int)'A', (int)'E');

                personalData = new ()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    SchoolGrade = schoolGrade,
                    AverageMark = averageMark,
                    ClassLetter = classLetter,
                };

                records.Add(new (id, personalData));
            }

            return records;
        }

        private static void ExportRecords(List<FileCabinetRecord> records)
        {
            using var streamWriter = new StreamWriter(outputFile, false, Encoding.UTF8);
            outputTypes[outputTypeIndex].Item2(streamWriter, records);
        }

        private static void ExportRecordsToXml(StreamWriter streamWriter, List<FileCabinetRecord> records)
        {
            var res = records.Select(record => new RecordForXml(record)).ToList<RecordForXml>();

            var type = typeof(List<RecordForXml>);

            var ignoreAttrs = new XmlAttributes();
            ignoreAttrs.XmlIgnore = true;

            var overrides = new XmlAttributeOverrides();
            overrides.Add(typeof(FileCabinetRecord), "FirstName", ignoreAttrs);
            overrides.Add(typeof(FileCabinetRecord), "LastName", ignoreAttrs);

            var serializer = new XmlSerializer(type, overrides, null, new ("records"), null);
            using var xmlWriter = XmlWriter.Create(streamWriter, new () { Indent = true });

            serializer.Serialize(xmlWriter, res);
        }

        private static void ExportRecordsToCsv(StreamWriter streamWriter, List<FileCabinetRecord> records)
        {
            using var writer = new FileCabinetRecordCsvWriter(streamWriter);
            foreach (var record in records)
            {
                writer!.Write(record);
            }
        }

        private static void SetOutputType(string parameters)
        {
            int index;
            if ((index = Array.FindIndex(outputTypes, 0, outputTypes.Length, x => x.Item1.Equals(parameters, StringComparison.InvariantCultureIgnoreCase))) == -1)
            {
                throw new ArgumentException($"Unsupportable output file type: {parameters}.");
            }

            outputTypeIndex = index;
        }

        private static void SetOutputFile(string parameters)
        {
            outputFile = parameters;
        }

        private static void SetRecordsAmount(string parameters)
        {
            if (!int.TryParse(parameters, out recordsAmount))
            {
                throw new ArgumentException($"Cannot parse records amount: {parameters}.");
            }
        }

        private static void SetStartId(string parameters)
        {
            if (!int.TryParse(parameters, out startId))
            {
                throw new ArgumentException("Cannot parse start id.");
            }
        }

        private static void ProceedArgs(string[] consoleArgs)
        {
            Stack<string> paramsAndArgs = new ();
            int index;
            foreach (var consoleArg in consoleArgs.Reverse())
            {
                if ((index = consoleArg.IndexOf('=', StringComparison.Ordinal)) != -1)
                {
                    paramsAndArgs.Push(consoleArg[(index + 1) ..]);
                    paramsAndArgs.Push(consoleArg[..index]);
                    continue;
                }

                paramsAndArgs.Push(consoleArg);
            }

            string param, arg;
            while (paramsAndArgs.TryPop(out param!))
            {
                if ((index = Array.FindIndex(args, 0, i => i.Item1.Contains(param))) != -1)
                {
                    if (paramsAndArgs.TryPop(out arg!))
                    {
                        args[index].Item2(arg);
                    }
                    else
                    {
                        throw new ArgumentException($"No argument for \'{param}\' parameter");
                    }
                }
                else
                {
                    throw new ArgumentException($"No defined parameter \'{param}\'.");
                }
            }
        }
    }
}