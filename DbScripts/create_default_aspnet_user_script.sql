﻿CREATE TABLE [dbo].[AdministrateLocations] (
    [UserId] [nvarchar](128) NOT NULL,
    [LocationId] [uniqueidentifier] NOT NULL,
    [IsInactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.AdministrateLocations] PRIMARY KEY ([UserId], [LocationId])
)
CREATE INDEX [IX_UserId] ON [dbo].[AdministrateLocations]([UserId])
CREATE INDEX [IX_LocationId] ON [dbo].[AdministrateLocations]([LocationId])
CREATE TABLE [dbo].[Locations] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](256),
    [Position] [geometry],
    [Street] [nvarchar](256),
    [PostalCode] [nvarchar](256),
    [Place] [nvarchar](256),
    [TotalParticipants] [int] NOT NULL,
    [IsInactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Locations] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar](128) NOT NULL,
    [BirthDate] [datetime] NOT NULL,
    [Gender] [int] NOT NULL,
    [FirstName] [nvarchar](256),
    [LastName] [nvarchar](256),
    [IsInactive] [bit] NOT NULL,
    [Email] [nvarchar](256),
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max),
    [SecurityStamp] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime],
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY ([Id])
)
CREATE UNIQUE INDEX [UserNameIndex] ON [dbo].[AspNetUsers]([UserName])
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [nvarchar](128) NOT NULL,
    [ClaimType] [nvarchar](max),
    [ClaimValue] [nvarchar](max),
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]([UserId])
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
)
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]([UserId])
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar](128) NOT NULL,
    [RoleId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId])
)
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]([UserId])
CREATE INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]([RoleId])
CREATE TABLE [dbo].[EventKeywords] (
    [EventId] [uniqueidentifier] NOT NULL,
    [KeywordId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.EventKeywords] PRIMARY KEY ([EventId], [KeywordId])
)
CREATE INDEX [IX_EventId] ON [dbo].[EventKeywords]([EventId])
CREATE INDEX [IX_KeywordId] ON [dbo].[EventKeywords]([KeywordId])
CREATE TABLE [dbo].[Events] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](256),
    [StartTime] [datetime],
    [EndTime] [datetime],
    [NfcTagId] [nvarchar](256),
    [IsInactive] [bit] NOT NULL,
    [Description] [ntext],
    [Image] [image],
    [Location_Id] [uniqueidentifier],
    CONSTRAINT [PK_dbo.Events] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Location_Id] ON [dbo].[Events]([Location_Id])
CREATE TABLE [dbo].[Keywords] (
    [Id] [uniqueidentifier] NOT NULL,
    [Label] [nvarchar](256),
    [IsInactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Keywords] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar](128) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY ([Id])
)
CREATE UNIQUE INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]([Name])
CREATE TABLE [dbo].[UserOnEvents] (
    [Id] [uniqueidentifier] NOT NULL,
    [BeginTime] [datetime] NOT NULL,
    [EndTime] [datetime],
    [IsInactive] [bit] NOT NULL,
    [Event_Id] [uniqueidentifier],
    [User_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.UserOnEvents] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Event_Id] ON [dbo].[UserOnEvents]([Event_Id])
CREATE INDEX [IX_User_Id] ON [dbo].[UserOnEvents]([User_Id])
ALTER TABLE [dbo].[AdministrateLocations] ADD CONSTRAINT [FK_dbo.AdministrateLocations_dbo.Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AdministrateLocations] ADD CONSTRAINT [FK_dbo.AdministrateLocations_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[EventKeywords] ADD CONSTRAINT [FK_dbo.EventKeywords_dbo.Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[EventKeywords] ADD CONSTRAINT [FK_dbo.EventKeywords_dbo.Keywords_KeywordId] FOREIGN KEY ([KeywordId]) REFERENCES [dbo].[Keywords] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Events] ADD CONSTRAINT [FK_dbo.Events_dbo.Locations_Location_Id] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[Locations] ([Id])
ALTER TABLE [dbo].[UserOnEvents] ADD CONSTRAINT [FK_dbo.UserOnEvents_dbo.Events_Event_Id] FOREIGN KEY ([Event_Id]) REFERENCES [dbo].[Events] ([Id])
ALTER TABLE [dbo].[UserOnEvents] ADD CONSTRAINT [FK_dbo.UserOnEvents_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201412071651286_init', N'PartyService.Migrations.Configuration',  0x1F8B0800000000000400ED1DD96E24B7F13D40FE61304F49B0D6E8880D47906C6875D84256DAC58ED6C89B404D53A3C6F631EEEE912504F9B23CE493F20B219B7DF06EB29B7D8C2318F06AD864B158AC2A92C562D57FFFFD9F931F5FC260F60C93D48FA3D3F9C1DEFE7C06A355ECF9D1FA74BECD1EBFF97EFEE30F7FFCC3C9A517BECC7E29EB1DE17AA865949ECE9FB26C73BC58A4AB271882742FF457499CC68FD9DE2A0E17C08B1787FBFB7F5B1C1C2C20023147B066B393CFDB28F34398FF403FCFE3680537D9160437B10783B428475F9639D4D92D0861BA012B783AFF0492EC750993677F05F748F5F9EC2CF00142650983C7F90C44519C810C217AFC2585CB2C89A3F572830A4070F7BA81A8DE230852580CE0B8AE6E3A96FD433C9645DDB004B5DAA6591C5A023C382A88B3E09BB722F1BC221E22DF252273F68A479D93F0747EE6857EE4A7590232F8215E15DDF01D1F9F07096E24A5F69E0CC6BB195DF35DC52988A1F07FEF66E7DB20DB26F034825BD43040F5B70F81BFFA3B7CBD8BBFC2E834DA06018D3A421E7D630A50D1A724DE40D4CF67F8580C08CD7072EDCD678BE6AA25AE62F59305DF57D59CEB865005F114928FF9EC06BC7C80D13A7B429273F8FD7C76E5BF40AF2C2998EC4BE42371428DB2648B7EDEA27182870056DF17DA7E699C49DF3F6D7DCF1ACC757A1D8155E63FC312CCFB380E20889A21DD82677F9DE3A0406D3EFB0C83FCAFF4C9DF102194F2C87DDDE22A89C3CF71A060C8AAE2FD32DE262B8C736C52FB0E246B98998F004FAA39F6A47603E6B89219D6794D19C6278B5A6AB5B2DC567E4797D93602D85100F0FF35D27BF8ED7746D2ABEFE4539CFA64420A4C611CC22C796D6A871082980BFA472F03C139AADF7F570158F5DFCB1D62F700F3B0BFF23720CAD2B2C7EB283B3A9C849294897E2AD53995625034A9158FBEA6A07D1AAA7752414427DAA91FDC664755CF70EBFE7B3FC99E2ED044955DE3BFEFFCD01ED24F30F2F0241523C0AC468A708135B42B3F49B34194E9073050471DC49E837419023FE81DDFBC1774627AF493107A5D71FE04D2F4B738F17E06E9930675F4A703D49770B54D9006596620DCF4DEDBA7A73882B7DBF08162FF01FA72363577BFC5578831E3E432C2AD3AC3433AFF6BBCCD2E230F2B932FD94AD42D86009CA073B65AC134BD42CC0CBDF3781B65DD566FBCAC38D21703ACF6F9EEBF71A557D71256794D55DB63D17900FC508376F99DC39314CB112BBED962F2215EFB3A0296DF394C48B11C93E29B2D2618820691E23387475E2A47837CEAB4F7BAF6605E84E1E5F4D56DC46E2A93D159BAB985D95ED97A8FC0BD4A104CB40E7CDD13C0BE9B1937AE377287A61BB9A38387C7A3EFBFFD0E7847DFFD151E7D3BFCA64EA56E28322E91128668D704B160799F4096C124AA67C044358DB189CCA78F6CF37A5EFEF29E7E01C1D67557ADA4219771F7D290839DBE34E468A2E2673FDFF71B1846CBCA08BC517DB9CDB559E638CC861607669843773E8C0E68252E782D722F2D18EAF485C5E2FA000F6897AE0E4A7C27C27197CF68E211ADF061D3D66444B71DCD74942361C82B05AE6DD8A5EAA683B59BEADE068A72F79BE324DDFDD233735F54AB77C1E25761372CA962BB37AF98AA09BFAAA202C3F25F2D8EE5BF9DF6EE05A55A48C18E5A4EA77B69B3CC103D89F5C3CE1A7219796D9ADD3EAEEEC05AAB98A766D7BC80E92AF137F4D5566F2789EB10AC6B84FD08E0EB33935EAC745921C4F2133DD1425C3D4E65B09FE51A83AB636FFCD0DCAF930E6417EAEC170566DA2B73632DD672351F7B211F41937D000FB0FF9B81B6126F7D6A707F62D88DD3C2AEDC1E8E6D07C7471EB96EA5E7FBBEA856EB2EF1ABA0BF24553ADF607F8C5AEDC7A8A6FF4FBAEC3D5CFB917CE7637B5FDA6E07D5879F86FA8443CDB278C0113E4AADFD6C0D675E6A3468DE398DFFA6C5AB852BDA36A46488F627B84EAF02B0AE7D6CED44AA86E446A2904643F08257A40169633F4BF91B88AF6D4B6A475FA3F837C44AB935FB74BE2F4C1453FD06E095B0A87BA0AF7B0543BAF6A1486B4255BAF02C4DE3959F538EDB162A6E0FD9FE9184CDEC9C866A2F917A73798348ED6F1071112BE051F26AE96374010398C1D9D98A783D9F8374053C51FCD010BD960896EC492128F7656691FD8B8003D29D30C16B0876858B706B3FCA4445EB47D8A72CB0A21E07C5506163BA54FDF15F2EE0068B44945951C90411B5373446A8EA979BC026EA9D2C288ED533B2DE4558C52686FEC25DB9C4B443095B0E2237ADB8D868280330B1D11CEE040F6BDC3854FC64E2D351331359D70751C0061E24E328DF668A0DC0B3CDD431414279DD3418B316CE3B5A26E03D79C66347CE6F88C244E2FDD21FF3B104198ADBD8C1EF067B151E59DA49E5DDB3C6632FCE194CC15E853B497FECC5126428F66207BF1BEC45FCECB473CA39DD8DC75CAC8B9F82B78825B53FD662A831146731239F3C63A91FFB591D09582B4BCFE70FC66233248FBB3B77D04318EBCC41CFD9E4F954762DA9621AED1D65CD2E853D73109DA8BB12E551AA2E167B518C1AE20CC0881A3A98F4AEF6421A8E071B8D34AA3BE806CED3A941C5BDB581DD657F6F8F33CCDA0E98F56FD262A870766ACBDF5AE82AE999DE02A0467E288193CECB8EC81BEFBE66C422822F9B6316E4DDE028F80AD01363436E00433322373F26DD2B9D4A076246C9DDBD8A557417F9E241487608EA6917A2711F18FE84A6A6D200DCA8A68449E77267F8014D01EC95BCEE20AEB89F670F4D950B88B93A54DFEC372DC89D7624C2ADBF097EAAC3A983A1DB1C452D064E1C0F10A764884FAA2BF4B30D869557BE78C01FE18BCC6D07F55DB819A4852F133F120C7C0933FDBBDDDAF9417B9617C8C402D701340652B84A090008951B1A0BB67219248941DD026CF912570BB630A45A802DDED56AA11205DD0094F3E61500B27B2213604A288DCD35689862D04417239A5052AC62AD4A3770A028A115195DF5AE9D6A6416F3865F5BACBD59AA11335228AC59D64E28145CC558F92D094B300362360450136969E14ED16C5F148EEEB623B6E84242549784D4455A10A9687AA12F9AFB8D39B150E51A7A19DCC40FC0814CAC0705A564B7C9E250B8FB647B5A70D7C01400D9D2E666E4E58AA618B9ECA253449CBBEAB41F397743A9187989AB9B91178B8B62E0922BB8A64B38FB61B377678A511778F6A36AC9C6CA50CD8AFB70CBFB22F7EA95D996374D400BA2491F4989E46ABCB430BEB6A0C6506E593434D1DD36F0906AFC1D5145B74CEB2CE82636F43664186AA9953D0656504063523735AA9B4EA3A919BD91B25D29526DEE1B6822B5F39A5B7ABBD385B7ED52101DCA8AEC2990489926A3A3A9D9911A8374DD30B512F6BC16495E9BC8D7618DFDCBD002C62D0AF549B0617196DBBCFA901EF1818B9E160DCBB0D220D69512AED6DAF2B94D65F5AABE9D2C4820FCA2E064A188987F7203361B3F5A5311F48B92D99284CF3FFF66691F563E2430162B46F8781B5DD5531627600DB9AFA86B84691E3FF40264E001E0D741E75E285493DAF814968CB24B9D194F9CC3D2D051B6C67F1721820CC3DD4B6CE405B02B34F41031454E05D8B0AF13C1CC70CA031080441144E63C0EB661A4F7465143A1FDE569483A3F7A3534FA1D1E0D8D2E17A19D2C384A093708C20409D73BECEC1BF1863B7EE8C003F6F3CECF94DD0C9177C0747B52620EA10E694E43A94BCD219541CE69386599153E550C730EA3AADC021A0953CE002245E6302441C8697892CFBF5309536DCEECA44BB6861B4896BC593F5245C5E6A68150C5E6B0CAE8DC34A0B2CC1C0A15959B0644155BAC114006AA2EED9B7B55D08AE0DA34A0A2C81206159F5900467DB3D0224C086D4699305F2C74251B279B5199EC270B2CE968D80C92F48756F0141495D7B0D0AC42FC6B46B10A5FADF6417C246C6E3BC47F6E015B8233FFCD1CAA2458360D58F2D91C36D69DA2C4D7A593596124C6FEAECB8D78EB6DBFF618C0E86721727328A0A204D380A8624B58451C600158513E4976525EE7B46727E2EDD08D9D1430D4BA8709B0CBAA1E6D5460354C266A2EA3DE755183D5F0EC987602ACA1BAF06ACF19B9774637C69083E85751943E7F3414951FE06833D774D564376B8C5B90FD8CE99B2B77A9A5CB39B33D55F9A1ABE150FEC234248D1BF1B8B3E668BADACED3EE1865A890A5AC35A52AB638129531B818662B0B2D46558535654656958E756C646297D2E0980F16D891F8A40C62A46832C2E44CFBB5577CD63AAF9B4815E1355983455EF43BB5B5B9DD9374DC8FD8ED4586529E235A41AB0B4417C6D0D205B79D4D54D9BA27D3681D1B92318DD6C5C3AF4CD3146CF61258B21BA2BC880C363D546DA3CD0DBEC9966D3B0DC28119B14D0546FE4888EADD1A31E513A9D6FCACC5E73C8EBCFCB26B769DE2F0A255644BC3E1F2F7FDD6DC20710931D61A65030BD5209900A5D747CB3920301C7086D2B96552CCA11AAE53CE20FE31E68C41EABBE30B7D380F23EAE7201C73853EC2C7884CA1186C334F084E427C956AE52A4AAADF959350E1A0C3780EE5D4C07E403915D2C25988F7D82155E6B3D2F6870E9DAF6906C33D5C616FF96B701EF8B9C2292BDC80C87F84694662F2CE0FF70F0EE7B3B3C00729F1DF2A7C918EF9277C46CE490747D839097AE1826F6EEFE284A1A4A9C704E116A385373BF1384F3BA5F6CDB14D3D153D8364F5046409BBAF230FBE9CCEFF99B73B9E5DFFE39E347D37FB98A0593C9EEDCFFE2506AEB7CF3F5B8D8460B48DFC5FB7D0CFCF118FBE3495B888590D86C5AE73B6F7073F739528A10B6F740BE92E92B443EE809A63F8EC013550A3ECC895BF1081BB867108339CD9C50E4EE92FE41CBBCA77C83164E24CE416A8C4A38874E0474DFC3BB014489EA70F2001523AB7D15794430F81ECA1BFB336890E4A8F9EF6F344B9F2B8E5A6DAB1C72DDC368C2542293C7CDCA2C67BFAB4478FF5EF61B1FC53085EFE6CADE058EF1E071019EF1EB7F09C905095E4BE0D2C65827B99E01A098734E17D1BD494C9EEDB2883DA35C7766357B6CC8BDFA1F3C9977CBB703CBB43D4E0B6789C64B9CA362BF190196255F02B33882E8F77A7A4BE63EFAF8574DF9D045D4CE96D01AE43DAEE169CB16319AF9D6D503E8909AD9DC11E93B5DB67B1EEF7282E7776D9B56378398A16D890A643CCACDA7366CAC99A5BD9330A189D8C1942C2E7569854502C70B14C79FC660FE13C75BA6D50B90C7A6DC1F03989A7750894A41F2E02BF596243A716F6C90FEBF3407DCDE7C08479AF16B50E7BAA0E6A73645963D2E48EC984ED32E2EEAEA9ABDD69120F7A84D3A4F2927227785C489FDADAAAE848F93BB2D45597DC5DB742564A51BED9BEEFB0DB16BA578B96B98E769EC673CC4C9DF228A8143E2324E61C2B85C86EA4296C976AF38DD7DE78CD9ED74C236872B174C64AD535250653C43E980E73296D65534A5F39162749026853880C99AF72C8F401BA87D35364208304955360A02254BA8281FACE48393403A99E4A4F91819A53504E817F48007805FBF49C747268EE513C5F9A18F3D8A7997CDB16BD6D8B9AF9AA31C2321FE575DC0C9055105C1E8901723E0E9DE46B5712CC39360588597EDA30E10E1EDACD99ACE179DCD09905F549B4DEB4C69BD6E8909652367743A4889C060BD94CEAD04CB4A30925A7924372CC735643E886699CB7762449A4E1FAD6B0ADA16FA55BA7589CF62AA50F47A158A4C6DBE2E85303D81EB4273BFD431D8F6D675FF39CDBF9E45FB24FA24B177621E1003F7BC5C3778D89C6380B28791A7D3AF71E62C400C441C02C351B8F86A66B7D77E65D100617C017410164234937B7309327B4E0814BAE3A849E2475F4DD2A92BBE9FA2EACE4DABE8B3AFABE15E9D5747D93A55FDB35A9A2EF599E5686EF98DDC60A9DB29F651DEAD36049BB53673F5576D00C593D062DFAC698B39B45E5EC34CD8CD9AC309A5F2A6AD55759574C2029BEAFA9E45F95A6129368FC26C33205C528C7F284F3A9EE1E69341E1B2E33A40AA991A64B0C17594FED86AB5C331511C61D0DD44592D3F60365166845EC6B4703ED9ED3B4FD30E9D5461EC8B927F546B07493C3747724587621E62633A998E54E4B00D9B64C0C0BED6AC04E928EB65FBC0C0934A1ACA2539E4DF7294365982B901E65D83D670395EA5F99EE1D558B8B36CB8ED93FDB30BAE4DC22C406743B54D53AA537E0755C9EDC0ED32275A7187DEF64F1791BE1A73BE4D7054CFD750D02C7138CE08A31795575AEA3C7B834BD71189555B8F7313730031EC8C05992F98F6095A1CF3834891FADE7B33CDC030E90F300BDEBE8E336DB6C333464183E044C9C046CC1D3F59FE72765713EF9983FE04C5D0C01A1E9E3D74E1FA3F75B3FF02ABCAF248FCA1420B069B0080482E732DF83AC5F2B48B7716408A8205F65D1BC83E12640C0D28FD112E02755F6B8210EFC00D760F55A078E5001699E0896EC27173E5827204C0B18757BF413F1B017BEFCF03FFA0E312B16D60000 , N'6.1.1-30610')


INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'9cf2a04d-09f0-4536-a0fc-8ad5d5726f52', N'admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'9a44e1d1-939b-4ce2-b147-b6ba45307db0', N'user')
INSERT [dbo].[AspNetUsers] ([Id], [BirthDate], [Gender], [FirstName], [LastName], [IsInactive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'867134c4-f823-4bc4-a6f2-2826e894e22d', CAST(0x0000654C00000000 AS DateTime), 1, N'Admin', N'Strator', 0, N'party0n.adm1n@gmail.com', 1, N'AHH6cTmcmsCAtMAB+NrVKcFVYSEQtCx4bPk18QSN/Vhv6PWUG7IAZDYVxI/ovzuCqQ==', N'3702172b-7cd9-4df5-bedb-f91c4f4b56f4', NULL, 0, 0, NULL, 0, 0, N'admin')
