using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading;
using FPsxWPF.Helpers;
using RPSWNET;

namespace SSCOUIModels.Commands
{
    public class ActionCommand : ICommand
    {
        public ActionCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string parameterString = null == parameter ? String.Empty : parameter.ToString();
            string token = parameterString;
            if (0 <= token.IndexOf('('))
            {
                token = token.Remove(token.IndexOf('('));
            }
            CmDataCapture.MaskedCaptureFormat(CmDataCapture.MaskExtensive, token, viewModel.StateParam, "ActionCommand, {0}", parameterString);
            string key;
            string param;
            SplitKeyParam(parameterString, out key, out param);
            if (key.Equals(ActionViewModelSet))
            {
                string[] values = param.Split(KeyValueDelimiter);
                if (2 == values.Length)
                {
                    if (values[0].Equals(ActionSetContext))
                    {
                        this.viewModel.SetPsxContext(values[1]);
                    }
                    else
                    {
                        this.viewModel.SetPropertyValue(values[0], values[1]);
                    }
                }
            }
            else if (null != Sequences)
            {
                string sequences;
                if (actionsToPsxSequences.TryGetValue(key, out sequences))
                {
                    sequences = String.Format(sequences, param);
                    token = sequences.Split(',')[0];
                    if (0 <= token.IndexOf('('))
                    {
                        token = token.Remove(token.IndexOf('('));
                    }
                    CmDataCapture.MaskedCaptureFormat(CmDataCapture.MaskExtensive, token, viewModel.StateParam, "PsxSequences.ExecuteSequence({0})", sequences);
                    Thread t = new Thread(() => Sequences.Execute(sequences));
                    t.Start();

                }
            }
        }

        /// <summary>
        /// The Actions's Sequences instance
        /// </summary>
        public PsxSequences Sequences { protected get; set; }

        public event EventHandler CanExecuteChanged;

        internal Dictionary<string, string> actionsToPsxSequences = new Dictionary<string, string>();

        protected MainViewModel viewModel;

        private static void SplitKeyParam(string str, out string key, out string param)
        {
            key = str;
            param = null;
            int startIndex = key.IndexOf(ParamDelimiterStart);
            if (startIndex > -1)
            {
                int endIndex = key.LastIndexOf(ParamDelimiterEnd);
                if (endIndex > -1)
                {
                    param = key.Substring(startIndex + 1, endIndex - startIndex - 1);
                    key = key.Remove(startIndex);
                }
            }
        }

        private const string ActionSetContext = "Context";
        private const string ActionViewModelSet = "ViewModelSet";        
        private const char KeyValueDelimiter = ';';
        private const char ParamDelimiterStart = '(';
        private const char ParamDelimiterEnd = ')';          
    }
}
