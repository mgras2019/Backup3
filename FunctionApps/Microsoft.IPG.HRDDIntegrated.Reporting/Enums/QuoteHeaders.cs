namespace Microsoft.IPG.HRDDIntegrated.Reporting.Enums
{
    public enum QuoteHeaders
    {
        AllDeals = 0,
        AllFlagggedDeals = 1,
        AnalyticallyFlagged = 2,
        ManuallyFlagged = 3,
        BothFlagged = 4,
        AllAssigned = 5,
        AllUnAssigned = 6,
        AllOpen = 7,
        AllClosed = 8,
        MyTotalDeals = 9,
        MyOpenDeals = 10,
        MyClosedDeals = 11,
        AllUnflaggedDeals = 12,
        MyAnalyticallyFlagged = 13,
        MyManuallyFlagged = 14,
        MyBothFlagged = 15,
        OnHold = 16,
        Trade = 17,
        OneVet = 18,
        PostFinalFlagged = 19,
        MyPostFinalFlagged = 20,
        FlaggedFailQuotes = 21,
        UnFlaggedFailQuotes = 22,
        MyFailedFlaggedQuotes = 23,             //This property indicate for unflagged deal those Fgot failed from MS quote with 3000 code
                                                // UnflaggedExAutoFail = 22//This property will indicate for all unflagged deals other than Error 

        CRMFailedQuotes = 24,
        MyCRMFailedQuotes = 25,
        AzureInvolvedFlagged = 26,
        AzureInvolvedUnflagged = 27,
        MyAzureInvolvedFlagged = 28,
        PilotY = 29,
        PilotN = 30,
        DNE = 31,
        PAM = 32,
        SOE = 33
    }
}
