param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/0297d9e34cc1ff5391b474b4fa29d6be9b236b83/Version.ps1'))
