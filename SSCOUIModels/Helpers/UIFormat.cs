using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPsxWPF.Controls;
using RPSWNET;

namespace SSCOUIModels.Helpers
{
    public class UIFormat 
    {
        public static string MultiLineFormat(string[] lines)
        {
            StringBuilder sb = new StringBuilder();
            if(lines != null)
            {
                foreach (string line in lines)
                {
                    if (!line.ToString().Equals(string.Empty))
                    {
                        sb.AppendLine(line);                        
                    }
                }               
            }            
            return sb.ToString().TrimEnd();
        }                       
    }
}
