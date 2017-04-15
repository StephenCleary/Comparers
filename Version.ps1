param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/3b91ded6dceccf15ca2f48e96339c1b076b47b72/Version.ps1'))

