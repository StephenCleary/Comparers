$testProjectLocations = @('test/UnitTests')
$outputLocation = 'testResults'
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/640c77d28ced405458b067de901bc611db60b880/Coverage.ps1'))
