using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    public class CleanDataPoint
    {
        public int Age { get; private set; }
        public DateTime Birth { get; private set; }
        public int ProgrammingSkill { get; private set; }
        public int UniYears { get; private set; }
        public OS OperatingSystem { get; private set; }
        public ProgrammingLanguage[] ProgLang { get; private set; }
        public int EngSkill { get; private set; }
        public Animal Animal { get; private set; }
        public bool Mountains{ get; private set; }
        public bool WinterFedUp { get; private set; }
        public int RandomNumber { get; private set; }
        public double RandReal { get; private set; }
        public double RandRealOther { get; private set; }
        public string CanteenFood { get; private set; }
        public Colour Colour { get; private set; }
        public bool NN_SVM { get; private set; }
        public bool KnowSQL { get; private set; }
        public SQL Server{ get; private set; }
        public bool A_priori { get; private set; }
        public bool Sqrt { get; private set; }
        public string Hometown { get; private set; }
        public string Therb { get; private set; }
        public int Planets { get; private set; }
        public int NextNum { get; private set; }
        public string Fibonacci { get; private set; }
        
        public static CleanDataPoint Clean(string[] inputValue)
        {

            string[] pointCon = inputValue.ToArray();

            for (int j = 0; j < pointCon.Length; j++)
            {
                pointCon[j] = pointCon[j].ToLower();
            }

            int i = 0;
            CleanDataPoint dp = new CleanDataPoint
                                    {
                                        Age = CleanAge(pointCon[i++], pointCon[i]),
                                        Birth = CleanBirth(pointCon[i++]),
                                        ProgrammingSkill = CleanProgSkill(pointCon[i++]),
                                        UniYears = CleanUniYears(pointCon[i++]),
                                        OperatingSystem = CleanOS(pointCon[i++]),
                                        ProgLang = CleanProgLang(pointCon[i++]),
                                        EngSkill = CleanEngSkill(pointCon[i++]),
                                        Animal = CleanAnimal(pointCon[i++]),
                                        Mountains = CleanMountain(pointCon[i++]),
                                        WinterFedUp = CleanWinter(pointCon[i++]),
                                        RandomNumber = CleanRandNum(pointCon[i++]),
                                        RandReal = CleanRandReal(pointCon[i++]),
                                        RandRealOther = CleanRandReal(pointCon[i++]),
                                        CanteenFood = CleanCanteen(pointCon[i++]),
                                        Colour = CleanColour(pointCon[i++]),
                                        NN_SVM = CleanAlgo(pointCon[i++]),
                                        KnowSQL = CleanSQL(pointCon[i++]),
                                        Server = CleanSQLServer(pointCon[i++]),
                                        A_priori = CleanAPriori(pointCon[i++]),
                                        Sqrt = CleanSqrtCorrect(pointCon[i++]),
                                        Hometown = CleanHometown(pointCon[i++]),
                                        Therb = CleanTherb(pointCon[i++]),
                                        Planets = CleanPlanets(pointCon[i++]),
                                        NextNum = CleanNextNum(pointCon[i++]),
                                        Fibonacci = CleanFibo(pointCon[i])
                                    };




            return dp;
        }

        private static string CleanFibo(string s)
        {
            return s;
        }

        private static int CleanNextNum(string s)
        {
            return Convert.ToInt32(s);
        }

        private static int CleanPlanets(string s)
        {
            string[] words = s.Split(' ', '.', '-');
            string[] numWords =
                words.Where(
                    e =>
                    e.Contains("1") || e.Contains("2") || e.Contains("3") || e.Contains("4") || e.Contains("5") ||
                    e.Contains("6") || e.Contains("7") || e.Contains("8") || e.Contains("9") || e.Contains("0"))
                    .ToArray();
            return numWords.Length == 0 ? 0 : Convert.ToInt32(numWords.First());
        }

        private static string CleanTherb(string s)
        {
            return s;
        }

        private static string CleanHometown(string s)
        {
            if (s.Contains("damascus"))
            {
                return "Damascus";
            }
            if (s.Contains("syria"))
            {
                return "Syria";
            }
            if (s.Contains("copenhagen") || s.Contains("denmark"))
            {
                return "Denmark";
            }
            if (s.Contains("who is that?"))
            {
                return "A moron wrote this answer";
            }
            return "Don't know";
        }

        private static bool CleanSqrtCorrect(string s)
        {
            if (s.Contains("6672.6"))
            {
                return true;
            }
            return false;
        }

        private static bool CleanAPriori(string s)
        {
            if (s.Contains("no"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static SQL CleanSQLServer(string s)
        {
            if (s.Contains("oracle"))
            {
                return SQL.Oracle;
            }
            if (s.Contains("db2"))
            {
                return SQL.DB2;
            }
            if (s.Contains("postgres"))
            {
                return SQL.Postgres;
            }
            if (s.Contains("my"))
            {
                return SQL.MySQL;
            }
            if (s.Contains("ms") || s.Contains("microsoft"))
            {
                return SQL.MS_SQL;
            }
            return SQL.Other;
        }

        private static bool CleanSQL(string s)
        {
            //anyone who says yes or gives what SQL stands for
            if (s.Contains("yes") || s.Contains("structured query language"))
            {
                return true;
            }
            return false;
        }

        private static bool CleanAlgo(string s)
        {
            //The assumption is that people who know both algorithms are positive and the rest is negative, which splits the data into people who are well-versed in datamining, and whose who are not
            if (s.Contains("no") || s.Contains("somewhat") || s.Contains("y/n") || s.Contains("n/y"))
            {
                return false;
            }
            return true;
        }

        private static Colour CleanColour(string s)
        {
            Colour c;
            if (Colour.TryParse(s, true, out c))
            {
                return c;
            }
            //rainbow, transparent, don't have one, %
            return Colour.Other;
        }

        private static string CleanCanteen(string s)
        {
            return s;
        }

        private static double CleanRandReal(string s)
        {
            //if people used '½'
            if (s == "½")
            {
                return 0.5;
            }

            //if people write a decimal with a '/'
            if (s.Contains('/'))
            {
                string[] arr = s.Split('/');
                return (Convert.ToInt32(arr[0])/Convert.ToInt32(arr[1]));
            }
            if (s.Contains("pi"))
            {
                //Fuck this guy
                return 1;
            }

            if (s == "-") { return 1; }

            s = s.Replace(',', '.');

            double val = Convert.ToDouble(s);
            //removing negative values by inversion
            val = Math.Abs(val);
            //if above 1, reduce to 1
            if (val > 1)
            {
                val = 1;
            }
            return val;

        }

        private static int CleanRandNum(string s)
        {
            //Conversion. All decimals are dropped
            int value = Convert.ToInt32(s.Split(',').First());

            //if different than given range
            if (value > 10)
            {
                value = 10;
            }
            if (value < 1)
            {
                value = 1;
            }
            return value;
        }

        private static bool CleanWinter(string s)
        {
            //Assuming some wrote a specifying answers
            if (s.Contains("(") && s.Contains(")"))
            {
                StringBuilder stb = new StringBuilder();
                bool atStart = false;
                bool atEnd = false;
                foreach (char c in s)
                {
                    if (c == ')')
                    {
                        atEnd = true;
                    }

                    if (atStart && !atEnd)
                    {
                        stb.Append(c);
                    }

                    if (c == '(')
                    {
                        atStart = true;
                    }
                }
                string str = stb.ToString();
                if (str.Contains("yes"))
                {
                    return true;
                }
                if (str.Contains("no"))
                {
                    return false;
                }
            }

            

            //covers answers of "no" and "not"
            if (s.Contains("no"))
            {
                return false;
            }

            //default. Assumes that "variant answers" are more likely to be variants on positives rather than negatives
            return true;
        }

        private static bool CleanMountain(string s)
        {
            if (s.Contains("yes") || s.Contains("at least one") /*special case*/)
            {
                return true;
            }
            return false;
        }

        private static Animal CleanAnimal(string s)
        {
            //check if text contains the relevant word
            if (s.Contains("elephant"))
            {
                return Animal.Elephant;
            }
            if (s.Contains("zebra"))
            {
                return Animal.Zebra;
            }
            if (s.Contains("asparagus"))
            {
                return Animal.Asparagus;
            }
            return Animal.Other;
        }

        private static int CleanEngSkill(string s)
        {
            //45-69

            //If no or incoherent answer, assume too low English skill to understand the question
            if (s == "-" || s == "%")
            {
                return 45;
            }

            //Conversion. All decimals are dropped
            int value = Convert.ToInt32(s.Split(',').First());

            //If people have written something outside the given range, I assume their English skills are so bad, that they were not able to read the instructions
            if (value < 45 || value > 69)
            {
                value = 45;
            }
            return value;
        }

        private static ProgrammingLanguage[] CleanProgLang(string s)
        {
            string[] langs = s.Split(' ', ',', '(', ')').Where(e => e.Length>0).ToArray();

            ProgrammingLanguage[] progLangs = new ProgrammingLanguage[3];
            for (int i = 0; i < 3; i++)
            {
                if (langs.Length - 1 < i)
                {
                    progLangs[i] = ProgrammingLanguage.UnknownProgrammingLanguage;
                }
                else
                {
                    progLangs[i] = GetProgLang(langs[i]);
                }
                
            }
            return progLangs;
        }

        private static ProgrammingLanguage GetProgLang(string s)
        {
            switch (s)
            {
                case "c#": return ProgrammingLanguage.CSharp;
                case "java": return ProgrammingLanguage.Java;
                case "javascript": return ProgrammingLanguage.Javascript;
                case "c++": return ProgrammingLanguage.CPlusPlus;
                case "f#": return ProgrammingLanguage.FSharp;
                case "sql": return ProgrammingLanguage.SQL;
                case "python": return ProgrammingLanguage.Python;
                case "obj-c":
                case "objective-c": return ProgrammingLanguage.ObjC;
                case "vb":
                case "vb.net": return ProgrammingLanguage.VisualBasic;
                case "pascal": return ProgrammingLanguage.Pascal;
                case "scala": return ProgrammingLanguage.Scala;
                case "lisp": return ProgrammingLanguage.Lisp;
                case "ruby": return ProgrammingLanguage.Ruby;
                case "sml": return ProgrammingLanguage.SML;
                case "ml": return ProgrammingLanguage.ML;
                case "matlab": return ProgrammingLanguage.MatLab;
                case "actionscript":
                case "as3": return ProgrammingLanguage.ActionScript;
                case "cobol": return ProgrammingLanguage.Cobol;
                case "c": return ProgrammingLanguage.C;
                case "haskell": return ProgrammingLanguage.Haskell;
                case "php": return ProgrammingLanguage.PHP;
                case "prolog": return ProgrammingLanguage.Prolog;
                default:
                    return ProgrammingLanguage.UnknownProgrammingLanguage;

            }
        }

        private static OS CleanOS(string s)
        {
            if (s.Contains("win"))
            {
                return OS.Windows;
            }
            if (s.Contains("linux") || s.Contains("ubuntu"))
            {
                return OS.Linux;
            }
            if (s.Contains("osx"))
            {
                return OS.OSX;
            }
            return OS.Unknown;
        }

        private static int CleanUniYears(string s)
        {
            string number = s.Split(' ')[0];
            bool addHalf = false;
            //change ',' to '.'
            number = number.Replace('.', ',');
            if (number.EndsWith("½"))
            {
                number = number.Substring(0, number.Length - 1);
                addHalf = true;
            }
            double num = (number.Length == 0 ? 0 : Convert.ToDouble(number));
            if (addHalf){ num += 0.5; }
            return Convert.ToInt32(num);
        }

        private static int CleanProgSkill(string s)
        {
            return Convert.ToInt32(s.Substring(0, 1));
        }

        private static DateTime CleanBirth(string s)
        {
            char[] splitters = new[]{'-','.',' ', '/'};
            string[] dates = s.Split(splitters).Where(e => e.Length != 0).ToArray();

            //if month is written as text
            dates = ConvertTextMonthToNum(dates);

            //if no year is given
            if (dates.Length == 2)
            {
                dates = new[]{dates[0], dates[1], "2000"};
            }

            //if date is written as six numbers without seperators
            if (dates[0].Length == 6)
            {
                dates = new[] {dates[0].Substring(0, 2), dates[0].Substring(2, 2), dates[0].Substring(4, 2)};
            }

            //special case
            if (dates[0] == "1st")
            {
                dates[0] = "01";
            }
            
            //if the format is year-month-day
            if (dates[0].Length > 2)
            {
                string temp = dates[0];
                dates[0] = dates[2];
                dates[2] = temp;
            }



            return new DateTime(2000, Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
            //return 
            //throw new NotImplementedException();
        }

        private static int CleanAge(string s, string date)
        {
            if (s.Contains("-"))
            {
                //special case
                return 22;
                //return -1;
            }

            try
            {
                return int.Parse(s);
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        #region HelperMethods

        private static Dictionary<string, string> textMonth = new Dictionary<string, string>()
            {
                {"jan","01"},
                {"feb","02"},
                {"mar","03"},
                {"apr","04"},
                {"may","05"},
                {"jun","06"},
                {"jul","07"},
                {"aug","08"},
                {"sep","09"},
                {"oct","10"},
                {"nov","11"},
                {"dec","12"},                           
            };

        private static string[] ConvertTextMonthToNum(string[] dates)
        {
            for (int i = 0; i < dates.Length; i++)
            {
                string text = dates[i];
                if(text.Length > 2)
                {
                    if(textMonth.ContainsKey(text.Substring(0,3)))
                    {
                        string tm = textMonth[text.Substring(0, 3)];
                        //if month is not in the correct position, it is swapped with where the month was found
                        if (i != 1)
                        {
                            string temp = dates[i];
                            dates[i] = dates[1];
                            dates[1] = temp;
                        }
                        dates[1] = tm;
                    }
                    
                }
            }

            return dates;
        }

        #endregion
    }

    public enum SQL
    {
        MySQL, MS_SQL, Oracle, Postgres, DB2, Other
    }

    public enum Colour
    {
        Red, Purple, Blue, Green, Yellow, Orange, White, Grey, Black, Other
    }

    public enum Animal
    {
        Zebra, Elephant, Asparagus, Other
    }

    public enum OS
    {
        Windows, Linux, OSX, Unknown
    }

    public enum ProgrammingLanguage
    {
        CSharp,
        FSharp,
        Java,
        C,
        CPlusPlus,
        SQL,
        Lisp,
        Python,
        Ruby,
        Haskell,
        PHP,
        VisualBasic,
        SML,
        ML,
        MatLab,
        ActionScript,
        Cobol,
        Prolog,
        Javascript,
        ObjC,
        Pascal,
        Scala,
        UnknownProgrammingLanguage
    }
}
