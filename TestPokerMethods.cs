using CardGame;
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
    /*
    
        [Theory]
        [InlineData("5H 6H 7H 8H 9H", "2S 3H 7C 2D KS", "5H 6H 7H 8H 9H", true, "Straight Flush")] // HAND 1 WIn
        [InlineData("3D 5D AS 9C 6S", "5H 6H 7H 8H 9H", "5H 6H 7H 8H 9H", true, "Straight Flush")] // Hand 2 win
         [InlineData("5H 6H 7H 8H 9H", "5D 6D 7D 8D 9D", null, true, "Straight Flush")] //should be tie
       
        [InlineData("5H 6H 7H 8H 9H", "TH JH QH KH AH", "TH JH QH KH AH", true, "Royal Flush")] // Hand 2 wins over straight flush
        [InlineData("5H 6H 7H 8H 9H", "6H 7H 8H 9H TH", "6H 7H 8H 9H TH", true, "Straight Flush")] // Hand 2 wins, higher straight flush wins
        public void TestMethod_DoesStraightFlushWin(
            string hand1String,
            string hand2String,
            string? expectedWinnerString,
            bool shouldpass,
            string expectedWinnerHandType
        )
        {
            // Arrange
            var hand1 = ParseHand(hand1String);
            var hand2 = ParseHand(hand2String);
            Hand expectedWinner = expectedWinnerString != null ? ParseHand(expectedWinnerString) : null;
    
            //Act
            var (winningHand, handType) = CompareHands.CheckHands(hand1, hand2);
    
            if (shouldpass)
            {
                if (expectedWinner == null)
                {
                    Assert.Null(winningHand);
                }
                else
                {
                    Assert.Equal(expectedWinner.Cards, winningHand.Cards);
                }
                Assert.Equal(expectedWinnerHandType, handType);
            }
            else
            {
                Assert.NotEqual(expectedWinner.Cards, winningHand.Cards);
            }
        }  */

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
        Hand expectedWinner = expectedWinnerString != null ? ParseHand(expectedWinnerString) : null;

        // Act
        var result = CompareHands.CompareHighestCard(hand1, hand2);

        // Assert
        if (shouldPass)
        {
            // Verify CORRECT behavior
            if (expectedWinner == null)
            {
                Assert.Null(result);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.Equal(expectedWinner.Cards[i].Rank, result.Cards[i].Rank);
                }
            }
        }
        else
        {
            // Verify INCORRECT behavior (test should fail)
            if (expectedWinner == null)
            {
                Assert.NotNull(result); // Should fail (expecting null)
            }
            else
            {
                if (result != null)
                {
                    bool allMatch = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if (expectedWinner.Cards[i].Rank != result.Cards[i].Rank)
                        {
                            allMatch = false;
                            break;
                        }
                    }
                    Assert.False(allMatch); // Should fail if ranks match
                }
            }
        }
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
    public void TestMethodIsAPair()
    {
        // Arrange
        var hand = ParseHand("AS KH 5S 5C 3D");

        // Act
        var result = CompareHands.IsPair(hand);
        Console.WriteLine(result);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void TestMethodIsNotAPair()
    {
        // Arrange
        var hand = ParseHand("AS KH 6S 5C 3S");

        // Act
        var result = CompareHands.IsPair(hand);
        Console.WriteLine(result);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("2H 2D 5S 5C KD", true)] // This hand has two pair
    [InlineData("9S KH 5S 9C KD", true)] // This hand also has two pairs
    [InlineData("3S KH 5S 8C KD", false)] // This hand has one pair so false
    [InlineData("3S KH 5S 8C 2D", false)] // This hand has zero pairs
    public void TestMethodIsItTwoPair(string handString, bool shouldPass)
    {
        var hand = ParseHand(handString);

        var result = CompareHands.IsTwoPair(hand);

        if (shouldPass)
        {
            Assert.NotNull(result);

            var pairCount = hand.Cards.GroupBy(c => c.Rank).Count(g => g.Count() == 2);
            Assert.Equal(2, pairCount);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [InlineData("2H 3H 5H 8H 9H", true)] // This hand has flush
    [InlineData("9S KS 5S 9S 4S", true)] // This hand has flush
    [InlineData("3S KH 5S 8C KD", false)] // This hand don't have a flush
    [InlineData("3S KH 5S 8C 2D", false)] // This hand no flush
    public void TestMethodIsItFlush(string handString, bool shouldPass)
    {
        var hand = ParseHand(handString);

        var result = CompareHands.IsFlush(hand);

        if (shouldPass)
        {
            Assert.NotNull(result);

            var uniqueSuits = hand.Cards.Select(c => c.Suit).Distinct().Count();
            Assert.Equal(1, uniqueSuits);
        }
        else
        {
            Assert.Null(result);

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

        var result = CompareHands.IsFullHouse(hand);

        if (shouldBeFullHouse)
        {
            Assert.NotNull(result);

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
            Assert.Null(result);

            var rankGroups = hand
                .Cards.GroupBy(c => c.Rank)
                .Select(g => g.Count())
                .OrderByDescending(count => count)
                .ToList();

            Assert.True(rankGroups[0] != 3 || (rankGroups.Count > 1 && rankGroups[1] != 2));
        }
    }
}
