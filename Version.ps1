param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/3b6932960a0df623dc0d2009bbdc6ebf033744d6/Version.ps1'))
