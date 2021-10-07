using System;
using System.Collections.Generic;
using System.Text;

namespace RiskEngine
{
    public class Card
    {
        public CommonFunctions.CardType type;

        public Card(CommonFunctions.CardType cardType)
        {
            this.type = cardType;
        }
    }
}
