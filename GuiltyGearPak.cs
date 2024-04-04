using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.Encryption.Aes;

Console.WriteLine("Will find the largest asset in Guilty Gear Strive .pak files, provide their dir path:");

// ..\steamapps\common\GUILTY GEAR STRIVE\RED\Content\Paks\
var paksDir = Environment.ExpandEnvironmentVariables(Console.ReadLine());

Console.WriteLine("Provide the aes key");
var aesKey = Console.ReadLine();

Console.WriteLine($"Searching {paksDir}");
var version = new VersionContainer(EGame.GAME_UE4_25);
var provider = new DefaultFileProvider(paksDir, SearchOption.AllDirectories, true, version);
provider.Initialize();
provider.SubmitKey(new FGuid(), new FAesKey(aesKey));
provider.LoadLocalization(ELanguage.English);

var largestAsset = provider.Files.Aggregate((largest, next) => largest.Value.Size > next.Value.Size ? largest: next);
Console.WriteLine($"Found {largestAsset.Key} of size {largestAsset.Value.Size}");

