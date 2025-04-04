using System.ComponentModel;
using System.Runtime.InteropServices;
using CardGame;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Xunit;

namespace testmethods;

public class MethodTester()
{
    /*
    [Fact]
    public void TestAdd()
    {
        // Arrange

        // Act
        int result = Program.Add(2, 2);

        // Assert
        Assert.Equal(4, result);
    }
*/

    [Theory]
    [InlineData("Pair")]
    public void Pair_ShouldBeat_HighCard(string expectedWinnerHandType)
    {
        var Pair = new Hand(
            [
                new Card('♥', '2'),
                new Card('♥', '2'),
                new Card('♠', '8'),
                new Card('♣', '7'),
                new Card('♣', '6'),
            ]
        );

        var HighCard = new Hand(
            [
                new Card('♣', 'A'),
                new Card('♠', '5'),
                new Card('♥', '4'),
                new Card('♣', '2'),
                new Card('♠', '9'),
            ]
        );

        var (winningHand1, handType1) = CompareHands.CheckHands(Pair, HighCard);
        var (winningHand2, handType2) = CompareHands.CheckHands(HighCard, Pair);

        Assert.Equal(expectedWinnerHandType, handType1);
        Assert.Equal(expectedWinnerHandType, handType2);
        Assert.Equal(Pair, winningHand1);
        Assert.Equal(Pair, winningHand2);
    }

    [Theory]
    [InlineData("Straight")]
    public void Straight_ShouldBeat_TwoPair(string expectedWinnerHandType)
    {
        var Straight = new Hand(
            [
                new Card('♥', 'T'),
                new Card('♥', '9'),
                new Card('♠', '8'),
                new Card('♣', '7'),
                new Card('♣', '6'),
            ]
        );

        var TwoPair = new Hand(
            [
                new Card('♣', '5'),
                new Card('♠', '5'),
                new Card('♥', 'T'),
                new Card('♣', 'T'),
                new Card('♠', '9'),
            ]
        );

        var (winningHand1, handType1) = CompareHands.CheckHands(Straight, TwoPair);
        var (winningHand2, handType2) = CompareHands.CheckHands(TwoPair, Straight);

        Assert.Equal(expectedWinnerHandType, handType1);
        Assert.Equal(expectedWinnerHandType, handType2);
        Assert.Equal(Straight, winningHand1);
        Assert.Equal(Straight, winningHand2);
    }

    [Theory]
    [InlineData("Royal Straight Flush")]
    public void RoyalFlush_ShouldBeat_FourOfAKind(string expectedWinnerHandType)
    {
        var RoyalStraightFlush = new Hand(
            [
                new Card('♥', 'A'),
                new Card('♥', 'K'),
                new Card('♥', 'Q'),
                new Card('♥', 'T'),
                new Card('♥', 'J'),
            ]
        );

        var FourOfKind = new Hand(
            [
                new Card('♣', '5'),
                new Card('♠', '5'),
                new Card('♥', '5'),
                new Card('♣', '5'),
                new Card('♠', '9'),
            ]
        );

        var (winningHand1, handType1) = CompareHands.CheckHands(RoyalStraightFlush, FourOfKind);
        var (winningHand2, handType2) = CompareHands.CheckHands(FourOfKind, RoyalStraightFlush);

        Assert.Equal(expectedWinnerHandType, handType1);
        Assert.Equal(expectedWinnerHandType, handType2);
        Assert.Equal(RoyalStraightFlush, winningHand1);
        Assert.Equal(RoyalStraightFlush, winningHand2);
    }

    [Theory]
    [InlineData("Flush")]
    public void Flush_ShouldBeat_ThreeOfAKind(string expectedWinnerHandType)
    {
        var FlushHand = new Hand(
            [
                new Card('♠', '2'),
                new Card('♠', '5'),
                new Card('♠', '4'),
                new Card('♠', '7'),
                new Card('♠', '9'),
            ]
        );

        var ThreeOfKind = new Hand(
            [
                new Card('♣', '5'),
                new Card('♠', '5'),
                new Card('♠', '5'),
                new Card('♣', '7'),
                new Card('♠', '9'),
            ]
        );

        var (winningHand, handType) = CompareHands.CheckHands(FlushHand, ThreeOfKind);

        Assert.Equal(expectedWinnerHandType, handType);
        Assert.Equal(FlushHand, winningHand);
    }

    [Theory]
    [InlineData("5H 6H 7H 8H 9H", "2S 3H 7C 2D KS", "5H 6H 7H 8H 9H", true, "Straight Flush")]
    [InlineData("3D 5D AS 9C 6S", "5H 6H 7H 8H TH", "5H 6H 7H 8H TH", true, "Flush")]
    [InlineData("5H 5D 7D 7S 9S", "5C 5S 7H 7C 9H", null, true, "Two Pair")]
    [InlineData("5H 5S 5D 8C 9H", "TH JH QH KH AH", "TH JH QH KH AH", true, "Royal Straight Flush")]
    [InlineData("5H 5S 5H 8C 9H", "3S 2H 8S 9S TD", "5H 5S 5H 8C 9H", true, "Three of a Kind")]
    //Testing FALSE BELOW
    [InlineData("5H 5S 5D 8C 9H", "6H 7H 8H 9H TH", "5H 5S 5D 8C 9H", false, "Three of a kind")]
    [InlineData("5H 5D 7D 7S 9S", "KH KS KD 8C 8H", "5H 5D 7D 7S 9S", false, "Two Pair")]
    [InlineData("AH KH QS JC 9D", "2H 2D 5S 8C TH", "AH KH QS JC 9D", false, "High Card")]
    [InlineData("AH KH QH JH 9H", "5S 5H 5D 5C 9S", "AH KH QH JH 9H", false, "Flush")]
    [InlineData("5H 6D 7S 8C 9H", "2H 4H 6H 8H TH", "5H 6D 7S 8C 9H", false, "Straight")]
    public void TestMethod_AdjustedForImplementation(
        string hand1String,
        string hand2String,
        string? expectedWinnerString,
        bool shouldPass,
        string expectedWinnerHandType
    )
    {
        // Arrange
        var hand1 = ParseHand(hand1String);
        var hand2 = ParseHand(hand2String);
        var expectedWinner = expectedWinnerString != null ? ParseHand(expectedWinnerString) : null;

        // Act
        var (winningHand, handType) = CompareHands.CheckHands(hand1, hand2);

        // Assert
        if (shouldPass)
        {
            if (expectedWinner == null)
            {
                Assert.Null(winningHand);
            }
            else
            {
                Assert.NotNull(winningHand);
                Assert.Equal(expectedWinnerHandType, handType);
            }
        }
        else
        {
            if (expectedWinner == null)
            {
                Assert.NotNull(winningHand);
            }
            else if (winningHand == null)
            {
                return;
            }
            else
            {
                Assert.NotEqual(expectedWinnerHandType, handType);
            }
        }
    }

    [Theory]
    [InlineData("2H 3D 5S 9C KD", "2C 3H 4S 8C AH", "2C 3H 4S 8C AH", true)] // Hand2 should win
    [InlineData("AS KH 5S 9C KD", "2C 3D 4S 6S 2H", "AS KH 5S 9C KD", true)] // Hand1 should win
    [InlineData("2H 3D 5S 9C KD", "2C 3H 5S 9C KD", null, true)] // Should be tie
    [InlineData("AS KH 5S 9C KD", "2C 3D 4S 6S 2H", "2C 3D 4S 6S 2H", false)] // Wrong winner (should fail)
    [InlineData("2H 3D 5S 9C KD", "2C 3H 5S 9C KD", "2H 3D 5S 9C KD", false)] // Should be tie, not Hand1
    public void TestMethod_CompareHighestCardWhichIsTheWinner(
        string hand1String,
        string hand2String,
        string? expectedWinnerString,
        bool shouldPass
    )
    {
        // Arrange
        var hand1 = ParseHand(hand1String);
        var hand2 = ParseHand(hand2String);
        var expectedWinner = expectedWinnerString != null ? ParseHand(expectedWinnerString) : null;

        // Act
        var (winningHand, handType) = CompareHands.CheckHands(hand1, hand2);
        var winningHandString =
            winningHand != null
                ? ConvertSymbolFormatToLetters(string.Join(" ", winningHand.Cards))
                : null;

        // Assert
        if (shouldPass)
        {
            if (expectedWinnerString == null)
            {
                Assert.Null(winningHand);
            }
            else
            {
                Assert.Equal(expectedWinnerString, winningHandString);
            }
        }
        else
        {
            if (expectedWinnerString == null)
            {
                Assert.NotNull(winningHand);
            }
            else if (winningHand == null)
            {
                return;
            }
            else
            {
                Assert.NotEqual(expectedWinnerString, winningHandString);
            }
        }
    }

    private string ConvertSymbolFormatToLetters(string symbolHand)
    {
        var suitMap = new Dictionary<char, char>
        {
            { '♥', 'H' },
            { '♦', 'D' },
            { '♣', 'C' },
            { '♠', 'S' },
        };

        return string.Join(
            " ",
            symbolHand.Split(' ').Select(card => $"{card[0]}{suitMap[card[1]]}")
        );
    }

    private Hand ParseHand(string handString)
    {
        // Map from test notation to deck's notation
        var suitMap = new Dictionary<char, char>
        {
            { 'H', '♥' },
            { 'D', '♦' },
            { 'C', '♣' },
            { 'S', '♠' },
        };

        var cards = new List<Card>();
        var cardStrings = handString.Split(' ');

        foreach (var cardStr in cardStrings)
        {
            // Convert rank (T stays T, JQKA remain the same)
            char rank = cardStr[0];

            // Convert suit (H→♥, D→♦, etc.)
            char inputSuit = cardStr[1];
            char actualSuit = suitMap[inputSuit];

            cards.Add(new Card(actualSuit, rank));
        }

        return new Hand(cards);
    }

    [Fact]
    public void TestMethodIsItThreeOfaKind()
    {
        // Arrange
        var ThreeOfKind = new Hand(
            [
                new Card('♣', '5'),
                new Card('♠', '5'),
                new Card('♠', '5'),
                new Card('♣', '7'),
                new Card('♠', '9'),
            ]
        );

        // Act
        var (resultHand, resultType) = CompareHands.IsThreeOfAKind(ThreeOfKind);

        // Assert
        Assert.NotNull(resultHand);
        Assert.Equal("Three of a Kind", resultType);
    }

    [Fact]
    public void TestMethodIsAPair()
    {
        // Arrange
        var hand = ParseHand("AS KH 5S 5C 3D");

        // Act
        var (resultHand, resultType) = CompareHands.IsPair(hand);

        // Assert
        Assert.NotNull(resultHand);
        Assert.Equal("Pair", resultType);
    }

    [Fact]
    public void TestMethodIsNotAPair()
    {
        // Arrange
        var hand = ParseHand("AS KH 6S 5C 3S");

        // Act
        var (resultHand, resultType) = CompareHands.IsPair(hand);

        // Assert
        Assert.NotEqual("Pair", resultType);
    }

    [Theory]
    [InlineData("2H 2D 5S 5C KD", true)] // This hand has two pair
    [InlineData("9S KH 5S 9C KD", true)] // This hand also has two pairs
    [InlineData("3S KH 5S 8C KD", false)] // This hand has one pair so false
    [InlineData("3S KH 5S 8C 2D", false)] // This hand has zero pairs
    public void TestMethodIsItTwoPair(string handString, bool shouldPass)
    {
        var hand = ParseHand(handString);

        var (resultHand, resultType) = CompareHands.IsTwoPair(hand);

        if (shouldPass)
        {
            Assert.NotNull(resultHand);

            var pairCount = hand.Cards.GroupBy(c => c.Rank).Count(g => g.Count() == 2);
            Assert.Equal(2, pairCount);
            Assert.Equal("Two Pair", resultType);
        }
        else
        {
            Assert.Null(resultHand);
            Assert.Null(resultType);
            Assert.NotEqual("Two Pair", resultType);
        }
    }

    [Theory]
    [InlineData("2H 3H 5H 8H 9H", true)] // This hand has flush
    [InlineData("9S KS 5S 9S 4S", true)] // This hand has flush
    [InlineData("3S KH 5S 8C KD", false)] // This hand don't have a flush
    [InlineData("3S KH 5S 8C 2D", false)] // This hand no flush
    public void TestMethodIsItFlush(string handString, bool isFlush)
    {
        var hand = ParseHand(handString);

        var (resultHand, resultType) = CompareHands.IsFlush(hand);

        if (isFlush)
        {
            var uniqueSuits = hand.Cards.Select(c => c.Suit).Distinct().Count();
            Assert.Equal(1, uniqueSuits);
            Assert.Equal(hand, resultHand);
            Assert.Equal("Flush", resultType);
        }
        else
        {
            Assert.NotEqual("Flush", resultType);

            var uniqueSuits2 = hand.Cards.Select(c => c.Suit).Distinct().Count();
            Assert.True(uniqueSuits2 > 1);
        }
    }

    [Theory]
    [InlineData("AH AS AC KH KC", true)] // This hand has full house
    [InlineData("KH KS KD 9S 9D", true)] // This hand has full house
    [InlineData("3S KH 5S 8C KD", false)] // This hand don't have a full house
    [InlineData("3S KH 2S 2C 2D", false)] // This hand no full house
    [InlineData("AS AD AH AC KD", false)] // Four Aces (not full house)
    [InlineData("KS KH QS QC JD", false)] // Two pair (not full house)
    public void TestMethodIsItFullHouse(string handString, bool shouldBeFullHouse)
    {
        var hand = ParseHand(handString);

        var (resultHand, resultType) = CompareHands.IsFullHouse(hand);

        if (shouldBeFullHouse)
        {
            Assert.Equal(hand, resultHand);
            Assert.Equal("Full House", resultType);

            var rankGroups = hand
                .Cards.GroupBy(c => c.Rank)
                .Select(g => g.Count())
                .OrderByDescending(count => count)
                .ToList();

            Assert.Equal(3, rankGroups[0]);
            Assert.Equal(2, rankGroups[1]);
        }
        else
        {
            Assert.Null(resultHand);
            Assert.Null(resultType);

            var rankGroups = hand
                .Cards.GroupBy(c => c.Rank)
                .Select(g => g.Count())
                .OrderByDescending(count => count)
                .ToList();

            Assert.True(rankGroups[0] != 3 || (rankGroups.Count > 1 && rankGroups[1] != 2));
            Assert.NotEqual("Full House", resultType);
        }
    }

    [Theory]
    [InlineData("AH KH QH JH TH", true)] // Royal Flush (Hearts)
    [InlineData("AD KD QD JD TD", true)] // Royal Flush (Diamonds)
    [InlineData("AS KS QS JS TS", true)] // Royal Flush (Spades)
    [InlineData("AC KC QC JC TC", true)] // Royal Flush (Clubs)
    [InlineData("AH KH QH JH 9H", false)] // Straight Flush (not royal)
    [InlineData("AH AS AC AD KH", false)] // Four of a kind
    [InlineData("AH KH QH JH TD", false)] // Mixed suits
    [InlineData("KH QH JH TH 9H", false)] // King-high straight flush
    public void TestMethodIsItRoyalFlush(string handString, bool shouldBeRoyalFlush)
    {
        var hand = ParseHand(handString);

        var (resultHand, resultType) = CompareHands.IsRoyalFlush(hand);

        if (shouldBeRoyalFlush)
        {
            Assert.Equal("Royal Flush", resultType);

            var uniqueSuits = hand.Cards.Select(c => c.Suit).Distinct().Count();
            Assert.Equal(1, uniqueSuits);

            var royalRanks = new HashSet<string> { "A", "K", "Q", "J", "T" };
            var handRanks = hand.Cards.Select(c => c.Rank.ToString()).ToHashSet();
            Assert.True(royalRanks.SetEquals(handRanks));
        }
        else
        {
            Assert.NotEqual("Royal Flush", resultType);

            var isFlush = hand.Cards.Select(c => c.Suit).Distinct().Count() == 1;
            var hasRoyalRanks = new HashSet<string> { "A", "K", "Q", "J", "T" }.SetEquals(
                hand.Cards.Select(c => c.Rank.ToString())
            );

            Assert.False(isFlush && hasRoyalRanks);
        }
    }
}
