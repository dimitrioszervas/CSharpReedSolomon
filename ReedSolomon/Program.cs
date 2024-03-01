// See https://aka.ms/new-console-template for more information
using CSharpReedSolomon.ReedSolomon;
using System.Text;

byte[] StripPadding(byte[] paddedData)
{
    try
    {
        int padding = 1;
        for (int i = paddedData.Length - 1; i >= 0; i--)
        {
            if (paddedData[i] == 0)
            {
                padding++;
            }
            else
            {
                break;
            }
        }

        byte[] strippedData = new byte[paddedData.Length - padding];
        Array.Copy(paddedData, 0, strippedData, 0, strippedData.Length);

        return strippedData;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}

Console.WriteLine("Hello Reed-Solomon, World!");

string text = "The Thirty Years' War[m] was a conflict fought largely within the Holy Roman Empire from " +
              "1618 to 1648. Considered one of the most destructive wars in European history, estimates of" +
              " military and civilian deaths range from 4.5 to 8 million, while up to 60% of the population " +
              "may have died in some areas of Germany.[19] Related conflicts include the Eighty Years' War, " +
              "the War of the Mantuan Succession, the Franco-Spanish War, and the Portuguese Restoration War." +
              "\r\n\r\nUntil the 20th century, historians considered it a continuation of the German religious " +
              "struggle initiated by the Reformation and ended by the 1555 Peace of Augsburg. " +
              "This divided the Empire into Lutheran and Catholic states, but over the next 50 years the " +
              "expansion of Protestantism beyond these boundaries gradually destabilised Imperial authority." +
              " While a significant factor in the war that followed, it is generally agreed its scope and extent " +
              "was driven by the contest for European dominance between Habsburgs in Austria and Spain, and " +
              "the French House of Bourbon.[20]\r\n\r\nThe war began in 1618 when Ferdinand II was deposed " +
              "as King of Bohemia and replaced by Frederick V of the Palatinate. Although the Bohemian Revolt" +
              " was quickly suppressed, fighting expanded into the Palatinate, whose strategic importance drew " +
              "in the Dutch Republic and Spain, then engaged in the Eighty Years War. Since ambitious external " +
              "rulers like Christian IV of Denmark and Gustavus Adolphus also held territories within the Empire," +
              " what began as an internal dynastic dispute was transformed into a far more destructive European " +
              "conflict.\r\n\r\nThe first phase from roughly 1618 until 1635 was primarily a civil war between" +
              " Imperial states, external powers playing a supportive role. After 1635, the Empire became one " +
              "theatre in a wider struggle between France, supported by Sweden, and Spain in alliance with " +
              "Emperor Ferdinand III. This concluded with the 1648 Peace of Westphalia, whose provisions " +
              "included greater autonomy within the Empire for states like Bavaria and Saxony, as well as" +
              " acceptance of Dutch independence by Spain. By weakening the Habsburgs relative to France, " +
              "the conflict altered the European balance of power and set the stage for the wars of Louis XIV.";

Console.WriteLine("Input text:");
Console.WriteLine(text);
Console.WriteLine();

byte[] fileData = Encoding.ASCII.GetBytes(text);

CalculateReedSolomonShards reedSolomonShards = new CalculateReedSolomonShards(fileData, 3);

byte[][] shards = reedSolomonShards.Shards;
int totalNShards = reedSolomonShards.TotalNShards;
bool [] shardsPresent = new bool[totalNShards];
int shardLength = reedSolomonShards.ShardLength;
int nParityShards = reedSolomonShards.ParityNShards;

for (int i = 0; i < (totalNShards / 2 + 1); i++)
{
    shardsPresent[i] = true;
}

Console.WriteLine($"Total Shards: {totalNShards}");
Console.WriteLine($"Parity Shards: {nParityShards}");
Console.WriteLine($"Shard Length: {shardLength}");
Console.WriteLine();

var reedSolomon = new ReedSolomon(shardsPresent.Length - nParityShards, nParityShards);
reedSolomon.DecodeMissing(shards, shardsPresent, 0, shardLength);

// Write the Reed-Solomon matrix of shards to a 1D array of bytes
byte[] buffer = new byte[shards.Length * shardLength];
int offSet = 0;

for (int j = 0; j < shards.Length - nParityShards; j++)
{
    Array.Copy(shards[j], 0, buffer, offSet, shardLength);
    offSet += shardLength;
}

Console.WriteLine("Reassembled Output text:"); 
string output = Encoding.ASCII.GetString(StripPadding(buffer));
Console.WriteLine(output);
Console.WriteLine();
short num = -1;
Console.WriteLine((byte)num);
