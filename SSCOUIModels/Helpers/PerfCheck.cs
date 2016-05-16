using System;
using RPSWNET;

namespace SSCOUIModels
{
    public enum PerfMeasureEvents { SetContext, ActionCommand, StateParamChanged, ApplicationStartup, ShowControls, ApplicationShutDown, CollectionUpdate, KeywordSearch, PickListItemsRender, QuickPickItemsRender, CartReceiptRender, PickListItemSelect, KeyInCode, ShowPopupView };

    public class PerfCheck
    {
        
        public void StartEventLog(PerfMeasureEvents measureEvent)
        {
            StartEventLog(measureEvent, string.Empty);
        }

        public void EndEventLog(PerfMeasureEvents measureEvent)
        {
            EndEventLog(measureEvent, string.Empty);
        }

        public void StartEventLog(PerfMeasureEvents measureEvent, string info)
        {
            bool logMe = false;

            switch (measureEvent)
            {
                case PerfMeasureEvents.SetContext:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskPerformance))
                    {
                        logMe = true;
                        if (SetContextStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        SetContextStarted = true;
                    }
                    break;
                case PerfMeasureEvents.ActionCommand:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskPerformance))
                    {
                        if (info.Equals("UIActivity")) return; // prevent flooding

                        string trace = measureEvent.ToString();
                        if (info.Length > 0)
                        {
                            trace += string.Format(", {0}", info);
                        }

                        CmDataCapture.CaptureFormat(CmDataCapture.MaskPerformance, trace);
                    }
                    break;
                case PerfMeasureEvents.ShowControls:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (ShowControlsStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        ShowControlsStarted = true;
                    }
                    break;
                case PerfMeasureEvents.StateParamChanged:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (StateParamChangedStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        StateParamChangedStarted = true;
                    }
                    break;
                case PerfMeasureEvents.ApplicationStartup:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (StartupStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        StartupStarted = true;
                    }
                    break;
                case PerfMeasureEvents.ApplicationShutDown:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (ShutDownStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        ShutDownStarted = true;
                    }
                    break;
                case PerfMeasureEvents.CollectionUpdate:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (CollectionUpdateStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        CollectionUpdateStarted = true;
                    }
                    break;
                case PerfMeasureEvents.KeywordSearch:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (KeywordSearchStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        KeywordSearchStarted = true;
                    }
                    break;
                case PerfMeasureEvents.PickListItemsRender:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (PickListItemsRenderStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        PickListItemsRenderStarted = true;
                    }
                    break;
                case PerfMeasureEvents.CartReceiptRender:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (CartReceiptRenderStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        CartReceiptRenderStarted = true;
                    }
                    break;
                case PerfMeasureEvents.PickListItemSelect:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (PickListItemSelectStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        PickListItemSelectStarted = true;
                    }
                    break;
                case PerfMeasureEvents.KeyInCode:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (KeyInCodeStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        KeyInCodeStarted = true;
                    }
                    break;
                case PerfMeasureEvents.ShowPopupView:
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        logMe = true;
                        if (ShowPopupViewStarted) EndEventLog(measureEvent, ThreadInterrupted);
                        ShowPopupViewStarted = true;
                    }
                    break;
            }

            if (logMe)
            {
                string trace = "Start of " + measureEvent.ToString();
                if (info.Length > 0)
                {
                    trace += string.Format(", {0}", info);
                }

                CmDataCapture.CaptureFormat(CmDataCapture.MaskPerformance, trace);
            }
        }

        public void EndEventLog(PerfMeasureEvents measureEvent, string info)
        {
            bool paired = false;
            switch (measureEvent)
            {
                case PerfMeasureEvents.SetContext:
                    if (SetContextStarted)
                    {
                        paired = true;
                        SetContextStarted = false;
                    }
                    break;
                case PerfMeasureEvents.ActionCommand:
                    if (ActionCommandStarted)
                    {
                        paired = true;
                        ActionCommandStarted = false;
                    }
                    break;
                case PerfMeasureEvents.ShowControls:
                    if (ShowControlsStarted)
                    {
                        paired = true;
                        ShowControlsStarted = false;
                    }
                    break;
                case PerfMeasureEvents.StateParamChanged:
                    if (StateParamChangedStarted)
                    {
                        paired = true;
                        StateParamChangedStarted = false;
                    }
                    break;
                case PerfMeasureEvents.ApplicationStartup:
                    if (StartupStarted)
                    {
                        paired = true;
                        StartupStarted = false;
                    }
                    break;
                case PerfMeasureEvents.ApplicationShutDown:
                    if (ShutDownStarted)
                    {
                        paired = true;
                        ShutDownStarted = false;
                    }
                    break;
                case PerfMeasureEvents.CollectionUpdate:
                    if (CollectionUpdateStarted)
                    {
                        paired = true;
                        CollectionUpdateStarted = false;
                    }
                    break;
                case PerfMeasureEvents.KeywordSearch:
                    if (KeywordSearchStarted)
                    {
                        paired = true;
                        KeywordSearchStarted = false;
                    }
                    break;
                case PerfMeasureEvents.PickListItemsRender:
                    if (PickListItemsRenderStarted)
                    {
                        paired = true;
                        PickListItemsRenderStarted = false;
                    }
                    break;
                case PerfMeasureEvents.QuickPickItemsRender:
                    if (QuickPickItemsRenderStarted)
                    {
                        paired = true;
                        QuickPickItemsRenderStarted = false;
                    }
                    break;
                case PerfMeasureEvents.CartReceiptRender:
                    if (CartReceiptRenderStarted)
                    {
                        paired = true;
                        CartReceiptRenderStarted = false;
                    }
                    break;
                case PerfMeasureEvents.PickListItemSelect:
                    if (PickListItemSelectStarted)
                    {
                        paired = true;
                        PickListItemSelectStarted = false;
                    }
                    break;
                case PerfMeasureEvents.KeyInCode:
                    if (KeyInCodeStarted)
                    {
                        paired = true;
                        KeyInCodeStarted = false;
                    }
                    break;
                case PerfMeasureEvents.ShowPopupView:
                    if (ShowPopupViewStarted)
                    {
                        paired = true;
                        ShowPopupViewStarted = false;
                    }
                    break;
            }
            if (paired)
            {
                string trace = "End of " + measureEvent.ToString();
                if (info.Length > 0)
                {
                    trace += string.Format(", {0}", info);
                }

                CmDataCapture.CaptureFormat(CmDataCapture.MaskPerformance, trace);
            }
        }

        public bool SetContextStarted { get; set; }
        public bool ActionCommandStarted { get; set; }
        public bool ShowControlsStarted { get; set; }
        public bool StateParamChangedStarted { get; set; }
        public bool StartupStarted { get; set; }
        public bool ShutDownStarted { get; set; }
        public bool CollectionUpdateStarted { get; set; }
        public bool KeywordSearchStarted { get; set; }
        public bool PickListItemsRenderStarted { get; set; }
        public bool QuickPickItemsRenderStarted { get; set; }
        public bool CartReceiptRenderStarted { get; set; }
        public bool PickListItemSelectStarted { get; set; }
        public bool KeyInCodeStarted { get; set; }
        public bool ShowPopupViewStarted { get; set; }

        private const string ThreadInterrupted = "Thread interrupted";
    }
}
