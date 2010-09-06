using System;
using System.Text.RegularExpressions;
using iCodeGenerator.DatabaseStructure;

namespace iCodeGenerator.Generator
{
	public class TableNameExpression : Expression
	{
		
        //public TableNameExpression()
        //{

        //}

        //public override void Interpret(Context context)
        //{
        //    Table table = (Table)Parameter;
        //    context.Output = Regex.Replace(context.Input,Context.StartDelimeter + "TABLE.NAME" + Context.EndingDelimiter,table.Name);
        //    context.Input = context.Output;
        //}

        //stupid-but-working code-duplication: copied replacement-logic from column to table

        private const string TABLE_NAME = "TABLE.NAME";
        public override void Interpret(Context context)
        {
            Table table = (Table)Parameter;
            Regex regex = new Regex(InputPattern, RegexOptions.Singleline);
            string inputString = context.Input;
            MatchCollection matches = regex.Matches(inputString);
            foreach (Match match in matches)
            {
                string matchString = match.Value;
                string naming = match.Groups["naming"].ToString();
                string replacement = table.Name;
                switch (naming)
                {
                    case "CAMEL":
                        replacement = CamelReplacement(table);
                        break;
                    case "PASCAL":
                        replacement = PascalReplacement(table);
                        break;
                    case "LOWER":
                        replacement = LowerReplacement(table);
                        break;
                    case "UPPER":
                        replacement = UpperReplacement(table);
                        break;
                    case "UNDERSCORE":
                        replacement = UnderscoreReplacement(replacement);
                        break;
                    case "HUMAN":
                        replacement = HumanReplacement(replacement);
                        break;
                    case "REMOVEPREFIX_LOWER_FIRSTUPPER":
                        replacement = RemovePrefixLowerFirstUpperReplacement(table);
                        break;
                    case "REMOVEPREFIX_UPPER":
                        replacement = RemovePrefixUpperReplacement(table);
                        break;
                    case "REMOVEPREFIX_LOWER":
                        replacement = RemovePrefixLowerReplacement(table);
                        break;
                    default:
                        break;
                }
                inputString = Regex.Replace(inputString, matchString, replacement);
            }
            context.Output = inputString;
            context.Input = context.Output;

            //			Column column = (Column)Parameter;
            //			context.Output = Regex.Replace(context.Input,
            //				InputPattern,
            //			    column.Name);
        }

        private static string UnderscoreReplacement(string replacement)
        {
            return SeparatorReplacement(replacement, "_", true);
        }

        private static string HumanReplacement(string replacement)
        {
            return SeparatorReplacement(replacement, " ", false);
        }

        private static string SeparatorReplacement(string replacement, string separatorString, bool ignoreFirstChar)
        {
            if (ignoreFirstChar && Regex.IsMatch(replacement.Substring(1), separatorString))
            {
                return replacement;
            }
            string firstChar = replacement.Substring(0, 1);
            if (!ignoreFirstChar)
                firstChar = firstChar.ToUpper();
            replacement = firstChar + replacement.Substring(1).Replace("_", String.Empty);
            MatchCollection minMay = Regex.Matches(replacement, "(?<min>[a-z])(?<may>[A-Z])");
            foreach (Match mm in minMay)
            {
                replacement =
                    Regex.Replace(replacement, mm.Groups["min"].Value + mm.Groups["may"].Value, mm.Groups["min"].Value + separatorString + mm.Groups["may"].Value);
            }
            return replacement;
        }

        private static string UpperReplacement(Table column)
        {
            string replacement;
            replacement = column.Name.ToUpper();
            return replacement;
        }

        private static string LowerReplacement(Table column)
        {
            string replacement;
            replacement = column.Name.ToLower();
            return replacement;
        }

        private static string PascalReplacement(Table column)
        {
            string replacement;
            replacement = column.Name.Replace("_", String.Empty);
            replacement = replacement.Substring(0, 1).ToUpper() + replacement.Substring(1);
            return replacement;
        }

        private static string CamelReplacement(Table column)
        {
            string replacement;
            replacement = column.Name.Replace("_", String.Empty);
            replacement = replacement.Substring(0, 1).ToLower() + replacement.Substring(1);
            return replacement;
        }

        private static string RemovePrefixLowerFirstUpperReplacement(Table column)
        {
            //MD_AC_SOMETEXT -> Sometext
            string[] split = column.Name.Split('_');
            string name = split[split.Length - 1];
            name = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower();

            return name;
        }

        private static string RemovePrefixLowerReplacement(Table column)
        {
            //MD_AC_SOMETEXT -> sometext
            string[] split = column.Name.Split('_');
            string name = split[split.Length - 1].ToLower();

            return name;
        }

        private static string RemovePrefixUpperReplacement(Table column)
        {
            //MD_AC_Sometext -> SOMETEXT
            string[] split = column.Name.Split('_');
            string name = split[split.Length - 1].ToUpper();

            return name;
        }

        private static string InputPattern
        {
            get
            {
                return Context.StartDelimeter +
                            TABLE_NAME +
                            @"\s*" +
                            @"(?<naming>(CAMEL|PASCAL|HUMAN|UNDERSCORE|UPPER|LOWER|REMOVEPREFIX_LOWER_FIRSTUPPER|REMOVEPREFIX_LOWER|REMOVEPREFIX_UPPER))*" +
                            Context.EndingDelimiter;
            }
        }


	}
}
