param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/bd44aef57d6df1acb1cab45eebb983f68a218528/Version.ps1'))
