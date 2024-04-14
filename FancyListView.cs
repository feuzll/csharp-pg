using System.Collections.Immutable;
using System.Data;
using System.Security.Cryptography;
using BrightIdeasSoftware;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;

namespace ue_pak;

public partial class Form1 : Form
{
    List<GameFile> listSource = null;
    DefaultFileProvider provider = null;

    VersionContainer version = new VersionContainer(EGame.GAME_UE4_25);

    public Form1()
    {
        InitializeComponent();
        OLVColumn titleColumn = new OLVColumn();
        //TypedColumn<GameFile> tTitleColumn = new TypedColumn<GameFile>(titleColumn);
        titleColumn.Text = "Title";
        //tTitleColumn.AspectGetter = (GameFile x) => x.Name;
        titleColumn.AspectGetter = (x) => (x as GameFile).Name;
        titleColumn.Width = 100;

        OLVColumn sizeColumn = new OLVColumn();
        sizeColumn.Text = "Size";
        sizeColumn.AspectGetter = (x) => (x as GameFile).Size;
        sizeColumn.Width = 100;

        var aesKeyFromEnv = Environment.GetEnvironmentVariable("AES");
        Label aesInputLabel = new Label() { Text = "AES" };
        TextBox aesInput = new TextBox()
        {
            Text = aesKeyFromEnv ?? "put x32b aes key here",
            Width = 400,
            Left = 100
        };

        Label pakPathLabel = new Label() { Text = "pak path", Top = 50 };
        var pakPathFromEnv = Environment.GetEnvironmentVariable("PAKPATH");
        TextBox pakPath = new TextBox()
        {
            Text = pakPathFromEnv != null ? Environment.ExpandEnvironmentVariables(pakPathFromEnv) :
                @"%USERPROFILE%\PathToSteam\steamapps\common\GUILTY GEAR STRIVE\RED\Content\Paks\",
            Width = 400,
            Left = 100,
            Top = 50
        };



        FastObjectListView olvView = new()
        {
            Size = new Size(500, 500),
            Top = 100,
            View = View.Details,
            ShowGroups = false,
            UseFiltering = false,
            VirtualMode = true,
            FullRowSelect = true,
            UseCompatibleStateImageBehavior = false,
            UseHotItem = true,
            OwnerDraw = false,
            EmptyListMsg = "This fast list is empty",
            TabIndex = 0,
            Name = "assets",
            HideSelection = true
        };

        olvView.AllColumns.Add(titleColumn);
        olvView.AllColumns.Add(sizeColumn);

        olvView.Columns.AddRange(olvView.AllColumns.ToArray());



        Button applyConfig = new Button()
        {
            Text = "Generate",
            Width = 150,
            Left = 550,
            Top = 25
        };
        applyConfig.Click += GenerateEntries;


        this.Controls.Add(aesInput);
        this.Controls.Add(aesInputLabel);
        this.Controls.Add(pakPath);
        this.Controls.Add(pakPathLabel);
        this.Controls.Add(applyConfig);
        this.Controls.Add(olvView);




        void GenerateEntries(object? sender, EventArgs e)
        {
            var paksDir = Environment.ExpandEnvironmentVariables(pakPath.Text);

            var aesKey = aesInput.Text;



            provider = new DefaultFileProvider(paksDir, SearchOption.AllDirectories, true, version);
            provider.Initialize();
            provider.SubmitKey(new FGuid(), new FAesKey(aesKey));
            provider.LoadLocalization(ELanguage.English);
            listSource = provider.Files.Values.ToList();




            olvView.SetObjects(listSource);

            olvView.VirtualListSize = listSource.Count; //the most necessary line cuz some olv/.net bugs?..

        }
    }


}
