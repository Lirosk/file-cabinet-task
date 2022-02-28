using FileCabinetApp;
using FileCabinetApp.Services;
using System.Text;

namespace FileCabinetGenerator
{
    public static class Program
    {
        private static string OutputType = string.Empty;
        private static string OutputFile = string.Empty;
        private static int RecordsAmount;
        private static int StartId;
        private static Random random = new ();

        private static Tuple<string[], Action<string>>[] args = new Tuple<string[], Action<string>>[]
        {
            new (new[] { "-t", "--output-type" }, SetOutputType),
            new (new[] { "-o", "--output" }, SetOutputFile),
            new (new[] { "-a", "--records-amount"}, SetRecordsAmount),
            new (new[] { "-i", "--start-id"}, SetStartId),
        };

        private static string[] SupportableFileExtensions = new[] { "csv", "xml" };

        public static void Main(string[] consoleArgs)
        {
            try
            {
                ProceedArgs(consoleArgs);
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

            SaveRecords(GenerateRecords());
        }

        private static IEnumerable<FileCabinetRecord> GenerateRecords()
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

            for (int id = StartId; id < StartId + RecordsAmount; id++)
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

                dateOfBirth = new(year, month, day);

                schoolGrade = (short)random.Next(1, 12);
                averageMark = (decimal)random.NextDouble() * 10;
                classLetter = (char)random.Next((int)'A', (int)'E');

                personalData = new()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    SchoolGrade = schoolGrade,
                    AverageMark = averageMark,
                    ClassLetter = classLetter,
                };

                yield return new(id, personalData);
            }
        }

        private static void SaveRecords(IEnumerable<FileCabinetRecord> records)
        {
            throw new NotImplementedException();
        }

        private static void SetOutputType(string parameters)
        {
            int index;
            if ((index = Array.FindIndex(SupportableFileExtensions, 0, SupportableFileExtensions.Length, x => x.Equals(parameters, StringComparison.InvariantCultureIgnoreCase))) == -1)
            {
                throw new ArgumentException($"Unsupportable output file type: {parameters}.");
            }

            OutputFile = SupportableFileExtensions[index];
        }

        private static void SetOutputFile(string parameters)
        {
            OutputFile = parameters;
        }

        private static void SetRecordsAmount(string parameters)
        {
            if (!int.TryParse(parameters, out RecordsAmount))
            {
                throw new ArgumentException($"Cannot parse records amount: {parameters}.");
            }
        }

        private static void SetStartId(string parameters)
        {
            if (!int.TryParse(parameters, out StartId))
            {
                throw new ArgumentException("Cannot parse start id.");
            }
        }

        private static void ProceedArgs(string[] consoleArgs)
        {
            Stack<string> paramsAndArgs = new();
            int index;
            foreach (var consoleArg in consoleArgs.Reverse())
            {
                if ((index = consoleArg.IndexOf('=', StringComparison.Ordinal)) != -1)
                {
                    paramsAndArgs.Push(consoleArg[(index + 1)..]);
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