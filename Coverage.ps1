$testProjectLocations = @('test/UnitTests', 'test/Linq.UnitTests', 'test/Ix.UnitTests', 'test/Rx.UnitTests')
$outputLocation = 'testResults'
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/3b91ded6dceccf15ca2f48e96339c1b076b47b72/Coverage.ps1'))
