
class Program
{
    static void Main()
    {
        // Define the relevant CAN ID and UDS service number for Writing 05
        const ushort engineId = 0x7E8;
        const ushort dongleId = 0x7E0;
        const byte writeDataService = 35;

        // Initialize variables for message counting and transfer data extraction
        int totalMessages = 0;
        int relevantMessages = 0;
        ushort transferDataLength = 0;
        List<byte> transferData = new List<byte>();

        // Open the input file for reading
        using (var inputFile = new FileStream(@"../../can_bus_extract/public/mg1cs002-stockmapsflash.candata", FileMode.Open, FileAccess.Read))
        {
            using var reader = new BinaryReader(inputFile);
            // Create the output file with .bin extension for writing
            using var outputFile = new FileStream(@$"{Directory.GetCurrentDirectory()}/output.bin", FileMode.Create, FileAccess.Write);
            {
                while (true)
                {
                    try
                    {

                        // Read the CAN ID
                        ushort canId = 0x7E8;

                        // Read the CAN data
                        byte[] canData = reader.ReadBytes(17);

                        // Increment total messages counter
                        totalMessages++;

                        
                        if ((byte)inputFile.Position == writeDataService)
                        {
                            // Console.WriteLine($"CanData{canData[4]:X4}{canData[5]:X4}{canData[6]:X4}{canData[7]:X4}");
                            relevantMessages++;

                            Console.WriteLine($"Relevant message found - CAN ID: {canId:X4}, Service Number: {canData[0]}");

                            // Extract the transfer data



                            // Add the remaining bytes to the transfer data
                            transferData.AddRange(canData.Skip(4).Take(13).ToArray());

                            Console.WriteLine($"Transfer data added: {transferData.Count}");

                            // Copy the content of the input file to the output file
                            inputFile.CopyTo(outputFile);

                            // Copy the content of the transferData list to the output file
                            outputFile.Write(transferData.ToArray(), 0, transferData.Count);

                            Console.WriteLine($"Transfer data added: {transferData.Count}");

                            // Clear the transfer data list 
                            transferData.Clear();
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
            }
        }

        // Print total messages count and relevant messages count
        Console.WriteLine($"Total messages: {totalMessages}");
        Console.WriteLine($"Relevant messages (containing transfer data): {relevantMessages}");
        Console.WriteLine("Transfer data extraction completed.");
    }
}



