namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    /// <summary>
    /// Filter Input Model
    /// </summary>
    public class FilterInput
    {
        /// <summary>
        /// The quote identifier
        /// </summary>
        private long? quoteId;

        /// <summary>
        /// The state identifier
        /// </summary>
        private long? stateId;

        /// <summary>
        /// The selected area
        /// </summary>
        private string selectedArea;

        /// <summary>
        /// The selected sales location
        /// </summary>
        private string selectedSalesLocation;

        /// <summary>
        /// The customer name
        /// </summary>
        private string customerName;

        /// <summary>
        /// The partner name
        /// </summary>
        private string partnerName;

        /// <summary>
        /// The selected state
        /// </summary>
        private string selectedState;

        /// <summary>
        /// The assigned to
        /// </summary>
        private string assignedTo;

        /// <summary>
        /// The trade screening status
        /// </summary>
        private string tradeScreenStatus;

        /// <summary>
        /// The ml prediction flag
        /// </summary>
        private string mlPrediction;

        /// <summary>
        /// The first flagged quarter
        /// </summary>
        private string quarterFirstFlagged;

        /// <summary>
        /// The manual flag category
        /// </summary>
        private string manuallyFlaggedCategory;

        /// <summary>
        /// The total score
        /// </summary>
        private string riskScoreFbi;

        /// <summary>
        /// The ust total score
        /// </summary>
        private string riskScoreUst;

        /// <summary>
        /// The landing probability
        /// </summary>
        private string landingProbability;

        /// <summary>
        /// The channel Model
        /// </summary>
        private string channelModel;

        /// <summary>
        /// Gets or sets the total score (risk score fbi).
        /// </summary>
        /// <value>
        /// The total score (risk score fbi).
        /// </value>
        public string RiskScoreFbi
        {
            get
            {
                if (!string.IsNullOrEmpty(riskScoreFbi))
                    return riskScoreFbi;
                else
                    return null;
            }
            set
            {
                riskScoreFbi = value;
            }
        }

        /// <summary>
        /// Gets or sets the Channel Model.
        /// </summary>
        /// <value>
        /// The Channel Model.
        /// </value>
        public string ChannelModel
        {
            get
            {
                if (!string.IsNullOrEmpty(channelModel))
                    return channelModel;
                else
                    return null;
            }
            set
            {
                channelModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the total ust score
        /// </summary>
        /// <value>
        /// The total ust score
        /// </value>
        public string RiskScoreUst
        {
            get
            {
                if (!string.IsNullOrEmpty(riskScoreUst))
                    return riskScoreUst;
                else
                    return null;
            }
            set
            {
                riskScoreUst = value;
            }
        }
        /// <summary>
        /// Gets or sets the landing probability(deal landing prediction).
        /// </summary>
        /// <value>
        /// The landing probability(deal landing prediction).
        /// </value>
        public string LandingProbability
        {
            get
            {
                if (!string.IsNullOrEmpty(landingProbability))
                    return landingProbability;
                else
                    return null;
            }
            set
            {
                landingProbability = value;
            }
        }
        /// <summary>
        /// Gets or sets the quote identifier.
        /// </summary>
        /// <value>
        /// The quote identifier.
        /// </value>
        public long? QuoteID
        {
            get
            {
                if (quoteId == null)
                {
                    return 0;
                }

                return quoteId;
            }

            set
            {
                quoteId = value;
            }
        }
        /// <summary>
        /// Gets or sets the selected area.
        /// </summary>
        /// <value>
        /// The selected area.
        /// </value>
        public string SelectedArea
        {
            get
            {
                if (!string.IsNullOrEmpty(selectedArea))
                    return selectedArea;
                else
                    return null;
            }
            set
            {
                selectedArea = value;
            }
        }
        /// <summary>
        /// Gets or sets the selected sales location.
        /// </summary>
        /// <value>
        /// The selected sales location.
        /// </value>
        public string SelectedSalesLocation
        {
            get
            {
                if (!string.IsNullOrEmpty(selectedSalesLocation))
                    return selectedSalesLocation;
                else
                    return null;
            }
            set
            {
                selectedSalesLocation = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        public string CustomerName
        {
            get
            {
                if (!string.IsNullOrEmpty(customerName))
                    return customerName;
                else
                    return null;
            }
            set
            {
                customerName = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the partner.
        /// </summary>
        /// <value>
        /// The name of the partner.
        /// </value>
        public string PartnerName
        {
            get
            {
                if (!string.IsNullOrEmpty(partnerName))
                    return partnerName;
                else
                    return null;
            }
            set
            {
                partnerName = value;
            }
        }
        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        public long? StateID
        {
            get
            {
                if (stateId == null)
                {
                    return 0;
                }

                return stateId;
            }

            set
            {
                stateId = value;
            }
        }
        /// <summary>
        /// Gets or sets the state of the selected.
        /// </summary>
        /// <value>
        /// The state of the selected.
        /// </value>
        public string SelectedState
        {
            get
            {
                if (!string.IsNullOrEmpty(selectedState))
                    return selectedState;
                else
                    return null;
            }
            set
            {
                selectedState = value;
            }
        }
        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        /// <value>
        /// The assigned to.
        /// </value>
        public string AssignedTo
        {
            get
            {
                if (!string.IsNullOrEmpty(assignedTo))
                    return assignedTo;
                else
                    return null;
            }
            set
            {
                assignedTo = value;
            }
        }


        /// <summary>
        /// Gets or sets the manually flagged category.
        /// </summary>
        /// <value>
        /// The manually flagged category.
        /// </value>
        public string ManuallyFlaggedCategory
        {
            get
            {
                if (!string.IsNullOrEmpty(manuallyFlaggedCategory))
                    return manuallyFlaggedCategory;
                else
                    return null;
            }
            set
            {
                manuallyFlaggedCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets the trade screening status.
        /// </summary>
        /// <value>
        /// The trade screening status.
        /// </value>
        public string TradeScreenStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(tradeScreenStatus))
                    return tradeScreenStatus;
                else
                    return null;
            }
            set
            {
                tradeScreenStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets the ml prediction flag.
        /// </summary>
        /// <value>
        /// The ml prediction flag.
        /// </value>
        public string MlPrediction
        {
            get
            {
                if (!string.IsNullOrEmpty(mlPrediction))
                    return mlPrediction;
                else
                    return null;
            }
            set
            {
                mlPrediction = value;
            }
        }

        /// <summary>
        /// Gets or sets the quarter first flagged.
        /// </summary>
        /// <value>
        /// The quarter first flagged.
        /// </value>
        public string QuarterFirstFlagged
        {
            get
            {
                if (!string.IsNullOrEmpty(quarterFirstFlagged))
                    return quarterFirstFlagged;
                else
                    return null;
            }
            set
            {
                quarterFirstFlagged = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the filter.
        /// </summary>
        /// <value>
        /// The type of the filter.
        /// </value>
        public int FilterType { get; set; }


        /// <summary>
        /// Segmentation
        /// </summary>
        public string Segmentation { get; set; }

        /// <summary>
        /// InScope
        /// </summary>
        public string InScope { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }
    }
}
