param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/18aed44c432381eaa11329d82f63dfaf835d219c/Version.ps1'))
