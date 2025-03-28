namespace CardGame
{
    public static class CompareHands
    {
        private static readonly Dictionary<string, int> HandPoints = new Dictionary<string, int>
        {
            { "Royal Straight Flush", 10 },
            { "Royal Flush", 9 },
            { "Straight Flush", 8 },
            { "Four of a Kind", 7 },
            { "Full House", 6 },
            { "Flush", 5 },
            { "Straight", 4 },
            { "Three of a Kind", 3 },
            { "Two Pair", 2 },
            { "Pair", 1 },
            { "High Card", 0 },
        };

        public static (Hand winningHand, string handType) CheckHands(Hand hand1, Hand hand2)
        {
            var methods = new Func<Hand, (Hand hand, string handType)>[]
            {
                IsRoyalStraightFlush,
                IsRoyalFlush,
                IsStraightFlush,
                IsFourOfAKind,
                IsFullHouse,
                IsFlush,
                IsStraight,
                IsThreeOfAKind,
                IsTwoPair,
                IsPair,
            };

            foreach (var method in methods)
            {
                var hand1Result = method(hand1);
                var hand2Result = method(hand2);

                if (hand1Result.hand != null && hand2Result.hand != null)
                {
                    if (HandPoints[hand1Result.handType] > HandPoints[hand2Result.handType])
                    {
                        return hand1Result;
                    }
                    else if (HandPoints[hand1Result.handType] < HandPoints[hand2Result.handType])
                    {
                        return hand2Result;
                    }
                    else
                    {
                        return (CompareSameTypeHands(hand1, hand2), hand1Result.handType);
                    }
                }
                else if (hand1Result.hand != null)
                {
                    return hand1Result;
                }
                else if (hand2Result.hand != null)
                {
                    return hand2Result;
                }
            }
            return (CompareHighestCard(hand1, hand2), "High Card"); // No special hand, highest card wins
        }

        private static Hand CompareSameTypeHands(Hand hand1, Hand hand2)
        {
            var ranks = "23456789TJQKA";
            var sorted1 = hand1.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            var sorted2 = hand2.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();

            for (var i = sorted1.Count - 1; i >= 0; i--)
            {
                if (ranks.IndexOf(sorted1[i].Rank) > ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand1;
                }
                if (ranks.IndexOf(sorted1[i].Rank) < ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand2;
                }
            }
            return null!;
        }

        // This is a helper method to compare the highest card in each hand
        public static Hand CompareHighestCard(Hand hand1, Hand hand2)
        {
            var ranks = "23456789TJQKA";
            var sorted1 = hand1.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            var sorted2 = hand2.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 4; i >= 0; i--)
            {
                if (ranks.IndexOf(sorted1[i].Rank) > ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand1;
                }
                if (ranks.IndexOf(sorted1[i].Rank) < ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand2;
                }
            }
            return null!;
        }

        public static (Hand hand, string handType) IsPair(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 2)
                {
                    return (hand, "Pair");
                }
            }
            return (null!, null!);
        }

        public static (Hand hand, string handType) IsTwoPair(Hand hand)
        {
            var pair1 = 0;
            var pair2 = 0;
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 2)
                {
                    if (pair1 == 0)
                    {
                        pair1 = card.Rank;
                    }
                    else if (pair2 == 0 && card.Rank != pair1)
                    {
                        pair2 = card.Rank;
                    }

                    if (pair1 != 0 && pair2 != 0)
                    {
                        return (hand, "Two Pair");
                    }
                }
            }
            return (null!, null!);
        }

        public static (Hand hand, string handType) IsThreeOfAKind(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 3)
                {
                    return (hand, "Three of a Kind");
                }
            }
            return (null!, null!);
        }

        public static (Hand hand, string handType) IsStraight(Hand hand)
        {
            var ranks = "23456789TJQKA";
            var sorted = hand.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 0; i < sorted.Count - 1; i++)
            {
                if (ranks.IndexOf(sorted[i + 1].Rank) - ranks.IndexOf(sorted[i].Rank) != 1)
                {
                    return (null!, null!);
                }
            }
            return (hand, "Straight");
        }

        public static (Hand hand, string handType) IsFlush(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Suit == card.Suit) == 5)
                {
                    return (hand, "Flush");
                }
            }
            return (null!, null!);
        }

        public static (Hand hand, string handType) IsFullHouse(Hand hand)
        {
            return IsPair(hand).hand != null && IsThreeOfAKind(hand).hand != null
                ? (hand, "Full House")
                : (null!, null!);
        }

        public static (Hand hand, string handType) IsFourOfAKind(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 4)
                {
                    return (hand, "Four of a Kind");
                }
            }
            return (null!, null!);
        }

        public static (Hand hand, string handType) IsStraightFlush(Hand hand)
        {
            return IsStraight(hand).hand != null && IsFlush(hand).hand != null
                ? (hand, "Straight Flush")
                : (null!, null!);
        }

        public static (Hand hand, string handType) IsRoyalFlush(Hand hand)
        {
            if (IsFlush(hand).hand == null)
            {
                return (null!, null!);
            }
            var ranks = "TJQKA";
            var sorted = hand.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].Rank != ranks[i])
                {
                    return (null!, null!);
                }
            }
            return (hand, "Royal Flush");
        }

        public static (Hand hand, string handType) IsRoyalStraightFlush(Hand hand)
        {
            return IsRoyalFlush(hand).hand != null && IsStraightFlush(hand).hand != null
                ? (hand, "Royal Straight Flush")
                : (null!, null!);
        }
    }
}
