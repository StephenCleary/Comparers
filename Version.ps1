param([String]$oldVersion="", [String]$newVersion="")
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/0c3f13fd4a3e2eea3789068f5ec971722c05ec27/Version.ps1'))
