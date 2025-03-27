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

    [Theory]
    [InlineData("5H 6H 7H 8H 9H", "2S 3H 7C 2D KS", "5H 6H 7H 8H 9H", true, "Straight Flush")] // HAND 1 WIn
    [InlineData("3D 5D AS 9C 6S", "5H 6H 7H 8H 9H", "5H 6H 7H 8H 9H", true, "Straight Flush")] // Hand 2 win
    /* [InlineData("5H 6H 7H 8H 9H", "5D 6D 7D 8D 9D", null, true, "Straight Flush")] //should be tie
   
    [InlineData("5H 6H 7H 8H 9H", "TH JH QH KH AH", "TH JH QH KH AH", true, "Royal Flush")] // Hand 2 wins over straight flush
    [InlineData("5H 6H 7H 8H 9H", "6H 7H 8H 9H TH", "6H 7H 8H 9H TH", true, "Straight Flush")] // Hand 2 wins, higher straight flush wins */
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
    }

    [Theory]
    // Correct cases (shouldPass = true)
    [InlineData("2H 3D 5S 9C KD", "2C 3H 4S 8C AH", "2C 3H 4S 8C AH", true)] // Hand2 should win
    [InlineData("AS KH 5S 9C KD", "2C 3D 4S 6S 2H", "AS KH 5S 9C KD", true)] // Hand1 should win
    [InlineData("2H 3D 5S 9C KD", "2C 3H 5S 9C KD", null, true)] // Should be tie
    // Incorrect cases (shouldPass = false)
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
}
