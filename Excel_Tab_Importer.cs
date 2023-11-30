﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Importer
{
    internal class Excel_Tab_Importer
    {
        private ExcelWorksheet ws;
        private int Line_Number;
        private bool verbose;
        private List<string> columns;

        public string initialize(string name, ExcelWorksheet w, bool v)
        {
            verbose = v;
            ws = w;
            Line_Number= 0;
            return create_table(name);
        }

        private string create_table(string name)
        {
            columns= new List<string>();
            string fields = string.Empty;
            int col = 1;
            int row = 1;
            while(true)
            {
                string hold = ws.Cells[row, col].Text;
                if (hold.Equals(string.Empty)) break;
                string[] split = hold.Split(new Char[] { ' ', '\n', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                if (null == split)
                {
                    if (verbose)
                    {
                        Console.WriteLine(string.Format("row: {0}, col: {1} bad", row, col));
                    }
                    Environment.Exit(1);
                }
                else if (0 == split.Length)
                {
                    if (verbose)
                    {
                        Console.WriteLine(string.Format("row: {0}, col: {1} empty", row, col));
                    }
                    break;
                }

                if(1 < col)
                {
                    fields += " ,";
                }

                fields = fields + "`" + split[0].ToUpper() + "` ";
                columns.Add(split[0]);

                int i = 1;
                while (i < split.Length)
                {
                    fields = fields + " " + split[i].ToUpper();
                    i += 1;
                }

                col += 1;

            }

            return "CREATE TABLE IF NOT EXISTS `" + name + "` ( " + fields + " );";
        }

        public List<Dictionary<String, String>> import()
        {
            List<Dictionary<String, String>> values = new List<Dictionary<String, String>>();
            int row = 2;
            while (true)
            {
                int col = 1;
                Dictionary<String, String> value = new Dictionary<String, String>();

                foreach (string name in columns)
                {
                    string hold = ws.Cells[row, col].Text;
                    if ((1 == col) && hold.Equals(string.Empty)) goto escape;

                    if (!value.ContainsKey(name))
                    {
                        value.Add(name, hold);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Duplicate Column Name {0} found, duplicate column names are not allowed", name));
                        Environment.Exit(3);
                    }
                    col += 1;
                }
                row += 1;
                values.Add(value);
            }

escape:
            return values;
        }
    }
}
